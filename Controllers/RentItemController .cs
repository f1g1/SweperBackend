using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
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
        public async Task<ActionResult<List<RentItemUI>>> GetMyRentItemsAsync(int skip = 0, int take = 10)
        {
            try
            {
                var email = await GetEmail();
                var z = _mapper.Map<List<RentItemUI>>(_context.RentItem.Include(x => x.RentItemImages)/*.Where(x => x.User.Email == email)*/
                    .OrderBy(x => x.DateCreated)
                    .Skip(skip)
                    .Take(take)
                    .ToList());

                return z;
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        [HttpDelete("my/{id}")]
        public async Task<ActionResult<RentItemUI>> DeleteItem(int id)
        {
            var email = await GetEmail();
            var rentItem = _context.RentItem.Include(x => x.User)
                .FirstOrDefault(x => x.Id == id && x.User.Email == email);
            if (rentItem == null)
            {
                return NotFound();
            }
            _context.Remove(rentItem);
            _context.SaveChanges();
            return Ok();
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


        [HttpPut]
        public async Task<ActionResult<RentItemUI>> EditRentItem(RentItemUI rentItem)
        {
            var email = await GetEmail();
            User user = _context.User.Include(x => x.InitialForm).FirstOrDefault(x => x.Email == email);
            //todo this would be better into a transaction
            var rentItemToEdit = _mapper.Map<RentItem>(rentItem);
            var rentItemFromDb = _context.RentItem.FirstOrDefault(x => x.Id == rentItem.Id);

            if (rentItemFromDb == null)
                return NotFound();

            if (user != null)
            {
                rentItemToEdit.User = user;
                rentItemToEdit.DateCreated = DateTime.UtcNow;
                rentItemToEdit.DateLastModified = DateTime.UtcNow;
                rentItemToEdit.RentItemImages = GetImages(rentItemToEdit, rentItem.Images);
                rentItemToEdit.Location = new NetTopologySuite.Geometries.Point(rentItem.Location.Latitude, rentItem.Location.Longitude) { SRID = 4326 };
                rentItemToEdit.Radius = rentItem.Location.Radius;
            }
            HandleChanges(rentItemToEdit, rentItemFromDb);

            _context.SaveChanges();
            var z = _mapper.Map<RentItemUI>(rentItemFromDb);
            return z;
        }

        private void HandleChanges(RentItem rentItemToEdit, RentItem rentItemFromDb)
        {
            rentItemFromDb.DateLastModified = DateTime.UtcNow;
            rentItemFromDb.Location = rentItemToEdit.Location;
            rentItemFromDb.Price = rentItemToEdit.Price;
            rentItemFromDb.Description = rentItemToEdit.Description;
            rentItemFromDb.Title = rentItemToEdit.Title;
            rentItemFromDb.Rooms = rentItemToEdit.Rooms;
            rentItemFromDb.Radius = rentItemToEdit.Radius;
            rentItemFromDb.Surface = rentItemToEdit.Surface;
            rentItemFromDb.City = rentItemToEdit.City;
            rentItemFromDb.Currency = rentItemToEdit.Currency;
            rentItemFromDb.Level = rentItemToEdit.Level;
            rentItemFromDb.Neighborhood = rentItemToEdit.Neighborhood;
            rentItemFromDb.RentItemImages = rentItemToEdit.RentItemImages;
            rentItemFromDb.Type = rentItemToEdit.Type;
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
                if (!string.IsNullOrEmpty(images[i].path))
                    continue;

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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
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