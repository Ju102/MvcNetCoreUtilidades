using Microsoft.AspNetCore.Mvc;
using MvcNetCoreUtilidades.Helpers;

namespace MvcNetCoreUtilidades.Controllers
{
    public class CifradoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CifradoBase()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CifradoBase(string contenido, string resultado, string accion)
        {
            // Ciframos el contenido
            string response = HelperCryptography.EncriptarTextoBasico(contenido);

            if (accion.ToLower() == "cifrar")
            {
                ViewData["cifrado"] = response;
                return View();
            }
            else
            {
                // Si el usuario quiere comparar, tambien nos enviara el texto para comparar en resultado
                if (response != resultado)
                {
                    ViewData["mensaje"] = "Los datos no coinciden";
                }
                else
                {
                    ViewData["mensaje"] = "Los datos coinciden";
                }
                return View();
            }
        }

        public IActionResult CifradoSalt()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CifradoSalt(string contenido, string resultado, string accion)
        {
            if (accion == "cifrar")
            {
                string response = HelperCryptography.CifrarContenidoSalt(contenido, false);
                ViewData["cifradoSalt"] = response;
                ViewData["salt"] = HelperCryptography.Salt;
            }
            else
            {
                string response = HelperCryptography.CifrarContenidoSalt(contenido, true);
                if (response != resultado)
                {
                    ViewData["mensaje"] = "Los datos no coinciden.";
                }
                else
                {
                    ViewData["mensaje"] = "Los datos coinciden.";
                }
            }

            return View();
        }
    }
}
