using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Net.Http.Headers;
using NetTopologySuite.Geometries;
using SweperBackend.Data;
using System.Drawing;

namespace SweperBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentItemController : ControllerBase
    {
        private const string fileType = ".jpg";
        private readonly SweperBackendContext _context;
        private readonly IMapper _mapper;

        public RentItemController(SweperBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<RentItemUI>>> GetMyRentItemsAsync(/*int skip = 0, int take = 10*/)
        {
            try
            {
                var email = await GetEmail();
                var z = _mapper.Map<List<RentItemUI>>(_context.RentItem.Include(x => x.RentItemImages)/*.Where(x => x.User.Email == email)*/
                    .OrderBy(x => x.DateCreated)
                    .Skip(0)
                    .Take(5)
                    .ToList());

                return z;
            }
            catch (Exception ex)
            {

            }
            return null;

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
                rentItemToDb.Location = new NetTopologySuite.Geometries.Point(rentItem.Location.Latitude, rentItem.Location.Longitude) { SRID = 4326 };
                rentItemToDb.Radius = rentItem.Location.Radius;
                _context.RentItem.Add(rentItemToDb);
            }

            _context.SaveChanges();
            var z = _mapper.Map<RentItemUI>(rentItemToDb);
            return z;
        }

        private List<RentItemImage> GetImages(RentItem rentItemToDb, List<ImageUi> images)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var folderName = Path.Combine("RentItems", rentItemToDb.User.Id, unixTimestamp.ToString());
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);


            bool exists = Directory.Exists(pathToSave);

            if (!exists)
                Directory.CreateDirectory(pathToSave);

            var imagesList = new List<RentItemImage>();
            for (int i = 0; i < images.Count; i++)
            {
                var fullPath = Path.Combine(pathToSave, i.ToString());
                var dbPath = Path.Combine(folderName, i.ToString() + fileType);
                using (var stream = new MemoryStream())
                {
                    stream.Write(Convert.FromBase64String(images[i].base64));

                    using (Bitmap bm2 = new Bitmap(stream))
                    {
                        bm2.Save(fullPath + fileType);
                    }

                }

                imagesList.Add(new()
                {
                    DateCreated = DateTime.UtcNow,
                    Path = dbPath,
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
        public string path { get; set; }
        public string timestamp { get; set; }
    }

}