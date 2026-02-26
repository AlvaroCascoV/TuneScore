using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace TuneScore.Helpers
{
    public enum Folders { Albums, Artists }
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private IServer server;
        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IServer server)
        {
            this.hostEnvironment = hostEnvironment;
            this.server = server;
        }
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Albums)
            {
                carpeta = "images/albums";
            }
            else if (folder == Folders.Artists)
            {
                carpeta = "images/artists";
            }
            string rootPath = hostEnvironment.WebRootPath;

            return Path.Combine(rootPath, carpeta, fileName);
        }

        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Albums)
            {
                carpeta = "images/albums";
            }
            else if (folder == Folders.Artists)
            {
                carpeta = "images/artists";
            }
            var addresses = this.server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + carpeta + "/" + fileName;
            return urlPath;
        }
    }
}
