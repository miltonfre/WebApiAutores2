namespace WebApiAutores.DTOs
{
    public class DatoHATEOAS
    {
        public string Enlance { get; private set; }
        public string Descripcion { get; private set;}
        public string Metodo { get; private set; }
        public DatoHATEOAS(string enlace, string descripcion, string metodo) {
            Enlance = enlace;
            Descripcion = descripcion;
                
            Metodo = metodo;
        }
    }
}
