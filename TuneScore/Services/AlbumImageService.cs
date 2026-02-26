using Microsoft.AspNetCore.Http;
using TuneScore.Helpers;
using TuneScore.Services.Interfaces;

namespace TuneScore.Services
{
    public class AlbumImageService : IAlbumImageService
    {
        private readonly HelperPathProvider _pathProvider;

        public AlbumImageService(HelperPathProvider pathProvider)
        {
            _pathProvider = pathProvider;
        }

        public async Task<string> SaveNewImageAsync(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            string path = _pathProvider.MapPath(fileName, Folders.Albums);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<string> ReplaceImageAsync(IFormFile file, string? existingFileName, string albumTitle)
        {
            if (!string.IsNullOrEmpty(existingFileName))
            {
                string oldPath = _pathProvider.MapPath(existingFileName, Folders.Albums);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }

            string extension = Path.GetExtension(file.FileName);

            string safeTitle = string.Concat(albumTitle.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));
            safeTitle = safeTitle.Replace(" ", "");

            string fileName = $"{safeTitle}Icon{extension}";

            string path = _pathProvider.MapPath(fileName, Folders.Albums);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}

