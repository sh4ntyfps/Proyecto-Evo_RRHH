namespace Capa_Entidades
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Accion { get; set; }
        public string? Entidad { get; set; }
        public string? Detalle { get; set; }
    }
}