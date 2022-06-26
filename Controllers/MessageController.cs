using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SweperBackend.Data;

namespace SweperBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SweperBackendContext _context;
        private readonly IMapper _mapper;

        public MessageController(SweperBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("GetConversation")]
        public ActionResult<List<MessageUi>> GetConversation(string userOwnerId, string userRenterId, int rentItemId)
        {
            var messages = _context.Message.Where(x =>
               x.UserRenterId == userRenterId
            && x.UserOwnerId == userOwnerId
            && x.RentItemId == rentItemId)

                .OrderBy(x => x.DateServer)
                .ToList();
            return _mapper.Map<List<MessageUi>>(messages);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> PostMessage(MessageUi messageUi)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            if (user.Id != messageUi.UserOwnerId && user.Id != messageUi.UserRenterId)
            {
                return false;
            }
            Message message = DtoToEntity(messageUi);

            _context.Add(message);

            IncrementChatCountUserRentItem(message);

            _context.SaveChanges();
            return Ok();
        }

        private void IncrementChatCountUserRentItem(Message message)
        {
            var userRentItem = _context.UserRentItem.FirstOrDefault(x => x.RentItemId == message.RentItemId && x.UserId == message.UserRenterId);
            userRentItem.ChatCount++;
            userRentItem.DateLastChat = DateTime.UtcNow;
        }

        private static Message DtoToEntity(MessageUi messageUi)
        {
            return new()
            {
                UserRenterId = messageUi.UserRenterId,
                UserOwnerId = messageUi.UserOwnerId,
                IsFromOwner = messageUi.IsFromOwner,

                RentItemId = messageUi.RentItemId,  
                DateCreated = messageUi.DateCreated,
                DateServer = DateTime.UtcNow,
                Text = messageUi.Text,
                Media = messageUi.Media,

            };
        }

        //[HttpGet("Conversation")]
        //public async Task<ActionResult<List<UserRentItemUi>>> GetLikedUserRentItemsAsync(int skip = 0, int take = 10)
        //{
        //    try
        //    {
        //        var email = await GetEmail();
        //        var user = _context.User.FirstOrDefault(x => x.Email == email);
        //        var res = _mapper.Map<List<UserRentItemUi>>(_context.UserRentItem
        //           .Where(x => x.Liked == true)
        //           .Where(x => x.Removed == false)
        //           .Include(x => x.RentItem).ThenInclude(x => x.RentItemImages)
        //           .Where(x => x.UserId == user.Id)
        //           .OrderBy(x => x.DateCreated)
        //           .Skip(skip)
        //           .Take(take)
        //           .ToList());
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;

        //}


        private async Task<string> GetEmail()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(accessToken.Replace("Bearer ", ""));
            return ((string)decodedToken.Claims["email"]);
        }
    }
}
