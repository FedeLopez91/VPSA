namespace VPSA.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public int Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public virtual string NombreCompleto
        {
            get
            {
                return $"{Nombre} {Apellido}";
            }
        }
    }
}