using System;
using TareaApp.Helpers;
using TareaApp.Models;
using TareaApp.Services;

namespace TareaApp
{
    class Program
    {
        static readonly TareaService _servicio = new TareaService();

        static void Main(string[] args)
        {
            // Patrón .NET 4: ConfigurationManager para obtener nombre de la app
            Console.Title = ConfigHelper.AppNombre;
            Console.WriteLine($"=== {ConfigHelper.AppNombre} ===\n");

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1": ListarTareas();    break;
                    case "2": AgregarTarea();    break;
                    case "3": CompletarTarea();  break;
                    case "4": EliminarTarea();   break;
                    case "5": salir = true;      break;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }

            Console.WriteLine("¡Hasta luego!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("1. Ver tareas pendientes");
            Console.WriteLine("2. Agregar tarea");
            Console.WriteLine("3. Completar tarea");
            Console.WriteLine("4. Eliminar tarea");
            Console.WriteLine("5. Salir");
            Console.Write("\nElige una opción: ");
        }

        static void ListarTareas()
        {
            var tareas = _servicio.ObtenerPendientes();
            Console.WriteLine($"\n--- Tareas pendientes ({tareas.Count}) ---");
            if (tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas pendientes.\n");
                return;
            }
            foreach (var t in tareas)
            {
                var limite = t.FechaLimite.HasValue
                    ? t.FechaLimite.Value.ToString("dd/MM/yyyy")
                    : "Sin límite";
                Console.WriteLine($"[{t.Id}] {t.Titulo} | {t.Prioridad} | Límite: {limite}");
            }
            Console.WriteLine();
        }

        static void AgregarTarea()
        {
            Console.Write("Título: ");
            var titulo = Console.ReadLine();

            Console.Write("Descripción: ");
            var desc = Console.ReadLine();

            Console.Write("Prioridad (0=Baja, 1=Media, 2=Alta): ");
            Enum.TryParse(Console.ReadLine(), out Prioridad prioridad);

            Console.Write("Fecha límite (dd/MM/yyyy, Enter para omitir): ");
            var fechaStr = Console.ReadLine();
            DateTime? fechaLimite = null;
            if (!string.IsNullOrWhiteSpace(fechaStr) &&
                DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out var fecha))
                fechaLimite = fecha;

            try
            {
                var tarea = _servicio.Agregar(titulo, desc, prioridad, fechaLimite);
                Console.WriteLine($"\n✓ Tarea #{tarea.Id} agregada.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}\n");
            }
        }

        static void CompletarTarea()
        {
            Console.Write("ID de la tarea a completar: ");
            if (int.TryParse(Console.ReadLine(), out int id) && _servicio.Completar(id))
                Console.WriteLine($"\n✓ Tarea #{id} marcada como completada.\n");
            else
                Console.WriteLine("\n✗ Tarea no encontrada.\n");
        }

        static void EliminarTarea()
        {
            Console.Write("ID de la tarea a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id) && _servicio.Eliminar(id))
                Console.WriteLine($"\n✓ Tarea #{id} eliminada.\n");
            else
                Console.WriteLine("\n✗ Tarea no encontrada.\n");
        }
    }
}
