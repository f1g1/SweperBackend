using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SignalRChat.Hubs;
using SweperBackend.Data;
using System.Drawing;

namespace SweperBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SweperBackendContext _context;
        private readonly IMapper _mapper;
        private readonly ChatHub _chatHub;
        private const string fileType = ".jpg";

        public MessageController(SweperBackendContext context, IMapper mapper, ChatHub chatHub)
        {
            _context = context;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        [HttpGet("GetConversation")]
        public ActionResult<List<MessageUi>> GetConversation(string userOwnerId, string userRenterId, int rentItemId, int take = 20, int skip = 0)
        {
            var messages = _context.Message.Where(x =>
               x.UserRenterId == userRenterId
            && x.UserOwnerId == userOwnerId
            && x.RentItemId == rentItemId)
                .OrderByDescending(x => x.DateServer)
                .Skip(skip)
                .Take(take + 1)
                .Reverse()
                .ToList();
            return _mapper.Map<List<MessageUi>>(messages);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostMessage(MessageUi messageUi)
        {
            _context.RentItem.FirstOrDefault(x => x.Id == messageUi.RentItemId);
            messageUi.RentItemTitle = messageUi?.RentItemTitle ?? "Not Found";
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

            string userIdToSend = messageUi.IsFromOwner ? messageUi.UserOwnerId : messageUi.UserRenterId;
            await _chatHub.SendChatMessage(userIdToSend, _mapper.Map<MessageUi>(message));

            return Ok();
        }

        private void IncrementChatCountUserRentItem(Message message)
        {
            var userRentItem = _context.UserRentItem.FirstOrDefault(x => x.RentItemId == message.RentItemId && x.UserId == message.UserRenterId);
            userRentItem.ChatCount++;
            userRentItem.DateLastChat = DateTime.UtcNow;
        }

        private Message DtoToEntity(MessageUi messageUi)
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
                Media = SaveImage(messageUi)
            };
        }

        private string? SaveImage(MessageUi messageUi)
        {
            //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var conversationFolderName = messageUi.RentItemId.ToString()
                + messageUi.UserOwnerId.ToString() + messageUi.UserRenterId.ToString();
            var folderName = Path.Combine("MessageImages", conversationFolderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);


            bool exists = Directory.Exists(pathToSave);

            if (!exists)
                Directory.CreateDirectory(pathToSave);

            if (string.IsNullOrEmpty(messageUi.Media))
                return null;

            Guid g = Guid.NewGuid();
            var fullPath = Path.Combine(pathToSave, g.ToString());
            var dbPath = Path.Combine(folderName, g.ToString() + fileType);


            try
            {
                using (var stream = new MemoryStream())
                {
                    var bytes = Convert.FromBase64String(messageUi.Media);
                    stream.Write(bytes);

                    using (Bitmap bm2 = new Bitmap(stream))
                    {
                        bm2.Save(fullPath + fileType);
                    }
                }
            }
            catch (Exception e)
            {


            }

            return dbPath;
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
