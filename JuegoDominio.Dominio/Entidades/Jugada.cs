using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Jugada
    {
        public Jugada(Ficha ficha, Extremo extremo)
        {
            Ficha = ficha;
            Extremo = extremo;
        }

        public Ficha Ficha { get; }
        public Extremo Extremo { get; }

        public override string ToString() => $"{Ficha} por el extremo {Extremo}";
    }
}
