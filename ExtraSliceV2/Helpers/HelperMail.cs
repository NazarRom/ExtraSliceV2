using ExtraSliceV2.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace ExtraSliceV2.Helpers
{
    public class HelperMail
    {
        private IConfiguration configuration;
        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private MailMessage ConfigureMailMessege
            (string para, string productos, string cantidad)
        {
            List<Producto> prods = JsonConvert.DeserializeObject<List<Producto>>(productos);
            List<int> cants = JsonConvert.DeserializeObject<List<int>>(cantidad);
            string tablaHtml = "<table>";
            tablaHtml += "<th>Producto</th>";
            tablaHtml += "<th>Descripción</th>";
            tablaHtml += "<th>Precio</th>";
            tablaHtml += "<th>Cantidad</th>";
            tablaHtml += "<th>Total</th>";
            decimal total = 0;
            foreach (Producto pro in prods)
            {
                total += pro.Precio;
            }
            for (var i = 0;i < prods.Count();i++)
            {
                Producto prod = prods[i];
                int cant = cants[i];
                
                tablaHtml += "<tr>";
                tablaHtml += "<td>" + prod.Nombre_producto + "</td>";
                tablaHtml += "<td>" + prod.Descripcion + "</td>";
                tablaHtml += "<td>" + prod.Precio+ "€" +  "</td>";
                tablaHtml += "<td>" + cant + "</td>";
                total += prod.Precio * (cant-1);

            }
            tablaHtml += "<td>" + total + "€" + "</td>";
            tablaHtml += "</tr>";
            tablaHtml += "</table>";

            MailMessage mailMessage = new MailMessage();
            string email = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(para));
            mailMessage.Subject = "Datos del pedido";
            mailMessage.Body = tablaHtml;
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }

        private SmtpClient ConfigureSmtpClient()
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string host = this.configuration.GetValue<string>("MailSettings:Smtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Smtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:Smtp:EnableSSL");
            bool defaultCredetials = this.configuration.GetValue<bool>("MailSettings:Smtp:DefaultCredentials");
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.UseDefaultCredentials = defaultCredetials;

            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;
            return client;
        }

        public async Task SendMailAsync(string para, string productos, string cantidad)
        {
            MailMessage mail = this.ConfigureMailMessege(para, productos, cantidad);
            SmtpClient client = this.ConfigureSmtpClient();
            await client.SendMailAsync(mail);
        }



    }
}
