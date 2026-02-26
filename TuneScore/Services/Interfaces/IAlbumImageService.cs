using Microsoft.AspNetCore.Http;

namespace TuneScore.Services.Interfaces
{
    public interface IAlbumImageService
    {
        Task<string> SaveNewImageAsync(IFormFile file);
        Task<string> ReplaceImageAsync(IFormFile file, string? existingFileName, string albumTitle);
    }
}

