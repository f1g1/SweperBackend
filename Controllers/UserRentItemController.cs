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

        public UserRentItemController(SweperBackendContext context)
        {
            _context = context;
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
                RentItemId = userRentItemUi.RentItemId

            };

            _context.Add(userRentItem);
            _context.SaveChanges();
            return Ok();
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
