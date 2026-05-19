using System;

namespace TareaApp.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLimite { get; set; }
        public Prioridad Prioridad { get; set; }
    }

    public enum Prioridad
    {
        Baja = 0,
        Media = 1,
        Alta = 2
    }
}
