using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Net.Http.Headers;
using NetTopologySuite.Geometries;
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

        [HttpGet]
        public ActionResult<RentItemUI> GetRentItems(RentItemUI rentItem)
        {
            return _mapper.Map<RentItemUI>(_context.RentItem.Include(x => x.RentItemImages).ToList());


        }
        [HttpPost]
        public async Task<ActionResult<RentItemUI>> PostRentItem(RentItemUI rentItem)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            //todo this would be better into a transaction
            var rentItemToDb = _mapper.Map<RentItem>(rentItem);
            if (user != null)
            {
                rentItemToDb.User = user;
                rentItemToDb.DateCreated = DateTime.UtcNow;
                rentItemToDb.DateLastModified = DateTime.UtcNow;
                rentItemToDb.DateLastModified = DateTime.UtcNow;
                rentItemToDb.RentItemImages = GetImages(rentItemToDb, rentItem.Images);
                rentItemToDb.Location = new Point(rentItem.Location.Longitude, rentItem.Location.Longitude) { SRID = 4326 };
                rentItemToDb.Radius = rentItem.Location.Radius;
                _context.RentItem.Add(rentItemToDb);
            }

            _context.SaveChanges();
            var z= _mapper.Map<RentItemUI>(rentItemToDb);
            return z;
        }

        private List<RentItemImage> GetImages(RentItem rentItemToDb, List<ImageUi> images)
        {

            var imagesList = new List<RentItemImage>();
            for (int i = 0; i < images.Count; i++)
            {
                imagesList.Add(new()
                {
                    DateCreated = DateTime.UtcNow,
                    Base64 = images[i].base64,
                    Index = i,
                    Timestamp = images[i].timestamp,
                    RentItem = rentItemToDb
                });
            }
            return imagesList;
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
        public List<ImageUi> Images { get; set; }


        public LocationUi Location { get; set; } = new LocationUi();
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime? DateLastModified { get; set; }
    }

    public class ImageUi
    {
        public int index { get; set; }
        public string base64 { get; set; }
        public string timestamp { get; set; }
    }

}