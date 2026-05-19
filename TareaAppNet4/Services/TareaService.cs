using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TareaApp.Helpers;
using TareaApp.Models;

namespace TareaApp.Services
{
    public class TareaService
    {
        private readonly string _archivo;
        private List<Tarea> _tareas;

        public TareaService()
        {
            // Patrón .NET 4: ConfigurationManager en vez de IConfiguration
            _archivo = ConfigHelper.ArchivoGuardado;
            _tareas = CargarDesdeDisco();
        }

        public IReadOnlyList<Tarea> ObtenerTodas()
            => _tareas.AsReadOnly();

        public IReadOnlyList<Tarea> ObtenerPendientes()
            => _tareas.Where(t => !t.Completada)
                      .OrderByDescending(t => t.Prioridad)
                      .ToList()
                      .AsReadOnly();

        public Tarea Agregar(string titulo, string descripcion,
                             Prioridad prioridad, DateTime? fechaLimite)
        {
            if (_tareas.Count >= ConfigHelper.MaxTareas)
                throw new InvalidOperationException(
                    $"Se alcanzó el máximo de {ConfigHelper.MaxTareas} tareas.");

            var tarea = new Tarea
            {
                Id           = _tareas.Count == 0 ? 1 : _tareas.Max(t => t.Id) + 1,
                Titulo       = titulo,
                Descripcion  = descripcion,
                Completada   = false,
                FechaCreacion = DateTime.Now,
                FechaLimite  = fechaLimite,
                Prioridad    = prioridad
            };

            _tareas.Add(tarea);
            GuardarEnDisco();
            return tarea;
        }

        public bool Completar(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea == null) return false;

            tarea.Completada = true;
            GuardarEnDisco();
            return true;
        }

        public bool Eliminar(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea == null) return false;

            _tareas.Remove(tarea);
            GuardarEnDisco();
            return true;
        }

        // Serialización con Newtonsoft.Json (patrón .NET 4)
        private List<Tarea> CargarDesdeDisco()
        {
            if (!File.Exists(_archivo)) return new List<Tarea>();
            var json = File.ReadAllText(_archivo);
            return JsonConvert.DeserializeObject<List<Tarea>>(json) ?? new List<Tarea>();
        }

        private void GuardarEnDisco()
        {
            var json = JsonConvert.SerializeObject(_tareas, Formatting.Indented);
            File.WriteAllText(_archivo, json);
        }
    }
}
