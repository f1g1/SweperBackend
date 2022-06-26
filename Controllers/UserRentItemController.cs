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
    public class UserRentItemController : ControllerBase
    {
        private readonly SweperBackendContext _context;
        private readonly IMapper _mapper;

        public UserRentItemController(SweperBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<bool>> PostUserRentItem(UserRentItemUi userRentItemUi)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            UserRentItem userRentItem = new UserRentItem()
            {
                User = user,
                DateCreated = DateTime.Now,
                DateInteraction = DateTimeOffset.FromUnixTimeMilliseconds(userRentItemUi.DateInteraction).DateTime,
                DateViewd = DateTimeOffset.FromUnixTimeMilliseconds(userRentItemUi.DateViewd).DateTime,
                RentItemId = userRentItemUi.RentItemId,
                Liked = userRentItemUi.Liked

            };

            _context.Add(userRentItem);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> RemoveUserRentItem(string id)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            var userRentItem = _context.UserRentItem.Find(id);
            if (userRentItem.UserId == user.Id)
            {
                _context.Remove(userRentItem);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<UserRentItemUi>>> GetLikedUserRentItemsAsync(int skip = 0, int take = 10)
        {
            try
            {
                var email = await GetEmail();
                var user = _context.User.FirstOrDefault(x => x.Email == email);
                var res = _mapper.Map<List<UserRentItemUi>>(_context.UserRentItem
                   .Where(x => x.Liked == true)
                   .Where(x => x.Removed == false)
                   .Include(x => x.RentItem).ThenInclude(x => x.RentItemImages)
                   .Where(x => x.UserId == user.Id)
                   .OrderByDescending(x => x.DateLastChat ?? x.DateCreated)
                   .Skip(skip)
                   .Take(take)
                   .ToList());
                return res;
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        [HttpGet("byRentItem")]
        public async Task<ActionResult<List<UserRentItemUi>>> GetLikedUserRentItemsAsync(int rentItemId, int skip = 0, int take = 10)
        {
            try
            {
                var email = await GetEmail();
                var user = _context.User.FirstOrDefault(x => x.Email == email);
                var res = _mapper.Map<List<UserRentItemUi>>(_context.UserRentItem
                   .Where(x => x.Liked == true)
                   .Where(x => x.Removed == false)
                   .Include(x => x.RentItem).ThenInclude(x => x.RentItemImages)
                   //.Include(x => x.User)
                   .Where(x => x.RentItemId == rentItemId
                    //we want to see chats only with people that are sending a message
                    && x.ChatCount > 0)
                   .OrderByDescending(x => x.DateLastChat ?? x.DateCreated)
                   .Skip(skip)
                   .Take(take)
                   .ToList());
                return res;
            }
            catch (Exception ex)
            {

            }
            return null;

        }



        private async Task<string> GetEmail()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(accessToken.Replace("Bearer ", ""));
            return ((string)decodedToken.Claims["email"]);
        }
    }
}
