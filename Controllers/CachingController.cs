using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MvcNetCoreUtilidades.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache memoryCache;

        public CachingController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult MemoriaPersonalizada(int? tiempo)
        {
            if (tiempo == null)
            {
                tiempo = 60;
            }
            string fecha =
                DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();
            //COMO ESTO ES MANUAL, DEBEMOS PREGUNTAR SI EXISTE
            //ALGO EN CACHE O NO...
            if (this.memoryCache.Get("FECHA") == null)
            {
                //NO EXISTE CACHE TODAVIA
                //CREAMOS EL OBJETO ENTRY OPTIONS CON EL TIEMPO
                MemoryCacheEntryOptions options =
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                        TimeSpan.FromSeconds(tiempo.Value)
                    );
                this.memoryCache.Set("FECHA", fecha, options);
                ViewData["MENSAJE"] = "Fecha almacenada correctamente";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");
            }
            else
            {
                //EXISTE CACHE Y LO RECUPERAMOS
                fecha = this.memoryCache.Get<string>("FECHA");
                ViewData["MENSAJE"] = "Fecha recuperada correctamente";
                ViewData["FECHA"] = fecha;
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
        public IActionResult MemoriaDistribuida()
        {
            string fecha =
                DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();
            ViewData["FECHA"] = fecha;
            return View();
        }
    }
}
