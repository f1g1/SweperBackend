using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using SweperBackend.Data;

namespace SweperBackend.Controllers
{
    [DisableRequestSizeLimit]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InitialFormController : ControllerBase
    {
        private readonly SweperBackendContext _context;

        public InitialFormController(SweperBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> CheckHasForm()
        {
            string email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            return user.InitialForm != null;
        }
        [HttpPost]
        public async Task<ActionResult<User>> PostForm(InitialFormUi initialFormUI)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);


            if (user != null && user.InitialForm == null)
            {
                _context.InitialForm.Add(new InitialForm()
                {
                    User = user,
                    PriceCategory = initialFormUI.PriceCategory,
                    SpaceCategory = initialFormUI.SizeCategory,
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    
                });
                var d = JsonConvert.DeserializeObject<List<LocationUi>>(initialFormUI.Locations);

                _context.AddRange(d.Select(x => new UserPreferredLocation()
                {
                    User = user,
                    Location = new Point(x.Latitude, x.Longitude) { SRID = 4326 },
                    Radius = x.Radius,
                    DateCreated = DateTime.UtcNow
                }));
                await _context.SaveChangesAsync();
                return Ok();

            }
            else
            {
                return BadRequest();
            }
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
