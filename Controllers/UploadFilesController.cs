using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MvcNetCoreUtilidades.Controllers
{
    public class UploadFilesController : Controller
    {
        private IWebHostEnvironment hostEnvironment;

        public UploadFilesController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult SubirFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubirFile(IFormFile file)
        {
            // De momento solo se sube el fichero a los elementos temporales
            //string tempFolder = Path.GetTempPath();

            // Necesitamos la ruta hacia la carpeta wwwroot
            string rootFolder = this.hostEnvironment.WebRootPath;
            string fileName = file.FileName;

            // Cuando pensamos en ficheros y sus rutas, se suele pensar en C:\ficheros\carpeta\file.txt
            // .NET Core no usa ese tipo de rutas de Windows, ya que las de Linux o MacOs pueden ser distintas.
            // Debemos crear rutas con herramientas de .NET Core.
            string path = Path.Combine(rootFolder, "uploads", fileName);

            // Para subir ficheros, se usa Stream.
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewData["mensaje"] = "Archivo subido con éxito en " + path;
            ViewData["filename"] = fileName;
            return View();
        }
    }
}
