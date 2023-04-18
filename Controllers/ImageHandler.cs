using SweperBackend.Controllers.UIData;
using SweperBackend.Data;
using System.Drawing;

namespace SweperBackend.Controllers
{
    public static class ImageHandler
    {
        private const string fileType = ".jpg";

        public static List<RentItemImage> UpdateImages(RentItem rentItemToDb, List<ImageUi> images)
        {
            var toBeDeleted = rentItemToDb.RentItemImages.Where(x => !images.Any(y => y.path == x.Path)).ToList();
            var toBeAdded = images.Where(x => string.IsNullOrEmpty(x.path) && !string.IsNullOrEmpty(x.base64)).ToList();

            // Delete the images
            foreach (var img in toBeDeleted)
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.Path);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                rentItemToDb.RentItemImages.Remove(img);
            }

            // Add new images
            var addedImages = AddImages(rentItemToDb, toBeAdded);
            rentItemToDb.RentItemImages.AddRange(addedImages);

            //Do reindexing
            images = images.OrderBy(x => x.path).ToList();
            rentItemToDb.RentItemImages = rentItemToDb.RentItemImages.OrderBy(x => x.Path).ToList();
            for (int i = 0; i < images.Count; i++)
            {
                rentItemToDb.RentItemImages[i].Index = images[i].index;
            }

            return rentItemToDb.RentItemImages.OrderBy(x => x.Index).ToList();

        }
        public static List<RentItemImage> AddImages(RentItem rentItemToDb, List<ImageUi> images)
        {
            string unixTimestamp = (rentItemToDb.DateCreated.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            var folderName = Path.Combine("RentItems", rentItemToDb.User.Id, unixTimestamp);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);


            bool exists = Directory.Exists(pathToSave);

            if (!exists)
                Directory.CreateDirectory(pathToSave);

            var imagesList = new List<RentItemImage>();
            for (int i = 0; i < images.Count; i++)
            {
                if (!string.IsNullOrEmpty(images[i].path))
                    continue;

                var guid = SaveToFile(images, i, pathToSave);

                imagesList.Add(new()
                {
                    DateCreated = DateTime.UtcNow,
                    Path = Path.Combine(folderName, guid + fileType),
                    Index = i,
                    Timestamp = images[i].timestamp,
                    RentItem = rentItemToDb
                });
            }
            return imagesList;
        }

        private static string SaveToFile(List<ImageUi> images, int i, string fullPath)
        {
            var guid = Guid.NewGuid().ToString();

            using (var stream = new MemoryStream())
            {
                stream.Write(Convert.FromBase64String(images[i].base64));

                using (Bitmap bm2 = new Bitmap(stream))
                {
                    bm2.Save(Path.Combine(fullPath, guid) + fileType);
                }
            }

            return guid;
        }
    }
}