using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SweperBackend.Data;

namespace SweperBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentItemController : ControllerBase
    {
        private readonly SweperBackendContext _context;
        private readonly IMapper _mapper;

        public RentItemController(SweperBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<RentItemUI>> PostRentItem(RentItemUI rentItem)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);

            var rentItemToDb = _mapper.Map<RentItem>(rentItem);
            if (user != null)
            {
                rentItemToDb.User = user;
                rentItemToDb.DateCreated = DateTime.UtcNow;
                rentItemToDb.DateLastModified = DateTime.UtcNow;
                rentItemToDb.DateLastModified = DateTime.UtcNow;

                _context.RentItem.Add(rentItemToDb);

            }

            return _mapper.Map<RentItemUI>(rentItemToDb);
        }

        private async Task<string> GetEmail()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();

            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(accessToken.Replace("Bearer ", ""));
            return ((string)decodedToken.Claims["email"]);
        }
    }

    public class RentItemUI
    {
        public string City { get; set; }
        public string Currency { get; set; }
        public string Level { get; set; }
        public string Neighborhood { get; set; }
        public int Price { get; set; }
        public int Rooms { get; set; }
        public int Surface { get; set; }
        public string Type { get; set; }


        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Radius { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime? DateLastModified { get; set; }
    }
}
