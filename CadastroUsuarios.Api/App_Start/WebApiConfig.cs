using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CadastroUsuarios.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }



            );

            // ==== FORÇAR JSON E REMOVER XML ====

            // Remove o formatter de XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Ajusta o formatter JSON
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Formatting.Indented;

            // Opcional: responder JSON mesmo quando o browser manda Accept: text/html
            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
