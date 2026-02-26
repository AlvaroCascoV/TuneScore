using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TuneScore.Models;

namespace TuneScore.Helpers
{
    public static class AlbumHelper
    {
        public static bool ValidateCreateImage(ModelStateDictionary modelState, IFormFile? file)
        {
            if (file == null || !file.ContentType.StartsWith("image/"))
            {
                modelState.AddModelError(string.Empty, "Debe subir una imagen válida.");
                return false;
            }

            return true;
        }

        public static bool ValidateEditImage(ModelStateDictionary modelState, IFormFile file)
        {
            if (!file.ContentType.StartsWith("image/"))
            {
                modelState.AddModelError(string.Empty, "Solo se permiten imágenes.");
                return false;
            }

            return true;
        }

        public static void UpdateEditableFields(Album target, Album source)
        {
            target.Title = source.Title;
            target.ReleaseYear = source.ReleaseYear;
            target.ArtistId = source.ArtistId;
        }
    }
}

