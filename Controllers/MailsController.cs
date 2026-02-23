using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace MvcNetCoreUtilidades.Controllers
{
    public class MailsController : Controller
    {
        private IConfiguration configuration;

        public MailsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string to, string asunto, string mensaje)
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");

            // Objeto para la informacion del email
            MailMessage email = new MailMessage();
            email.From = new MailAddress(user);
            email.To.Add(to);
            email.Subject = asunto;
            email.Body = mensaje;
            //<h1>Hola</h1>
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            // Recuperamos los datos para el objeto que manda el propio email
            string password = this.configuration.GetValue<string>(
                "MailSettings:Credentials:Password"
            );
            string host = this.configuration.GetValue<string>("MailSettings:Server:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Server:Port");
            bool ssl = this.configuration.GetValue<bool>("MailSettings:Server:Ssl");
            bool defaultCredentials = this.configuration.GetValue<bool>(
                "MailSettings:Server:DefautCredentials"
            );

            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.EnableSsl = ssl;
            client.UseDefaultCredentials = defaultCredentials;
            // Las credenciales para el mail cambian según cada caso.
            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;

            await client.SendMailAsync(email);
            ViewData["mensaje"] = "Mensaje enviado con éxito.";

            return View();
        }
    }
}
