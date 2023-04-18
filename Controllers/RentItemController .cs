using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SweperBackend.Controllers.UIData;
using SweperBackend.Data;
using System.Drawing;

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
                rentItemToDb.RentItemImages = ImageHandler.AddImages(rentItemToDb, rentItem.Images);
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

            var rentItemFromDb = _context.RentItem.Include(x => x.RentItemImages).FirstOrDefault(x => x.Id == rentItem.Id);
            rentItemToEdit.RentItemImages = rentItemFromDb.RentItemImages;
            if (rentItemFromDb == null)
                return NotFound();

            if (user != null)
            {
                rentItemToEdit.User = user;
                rentItemToEdit.DateCreated = DateTime.UtcNow;
                rentItemToEdit.DateLastModified = DateTime.UtcNow;
                rentItemToEdit.RentItemImages = ImageHandler.UpdateImages(rentItemToEdit, rentItem.Images);
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
            rentItemFromDb.RentItemImages = rentItemToEdit.RentItemImages;
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