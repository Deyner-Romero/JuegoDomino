using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Jugada
    {
        public Ficha? Ficha { get; set; }
        public Extremo Extremo { get; set; }
    }
}
