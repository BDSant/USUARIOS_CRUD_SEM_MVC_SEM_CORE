using System.Web.Http;
using WebActivatorEx;
using WebUsuarios;
using Swashbuckle.Application;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebUsuarios
{
    public class SwaggerConfig
    {
        public static void Register()
        {

        }
    }
}
