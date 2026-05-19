using System.Configuration;

namespace TareaApp.Helpers
{
    // Patrón .NET 4: acceso estático a ConfigurationManager
    public static class ConfigHelper
    {
        public static string AppNombre
            => ConfigurationManager.AppSettings["AppNombre"];

        public static int MaxTareas
            => int.Parse(ConfigurationManager.AppSettings["MaxTareas"]);

        public static string ArchivoGuardado
            => ConfigurationManager.AppSettings["ArchivoGuardado"];

        public static string ConnectionString
            => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
