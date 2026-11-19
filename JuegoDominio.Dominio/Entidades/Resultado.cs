using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public record Resultado
    {
        public TipoFin Fin { get; init; }
        public Jugador? Ganador { get; init; }
        public Pareja? ParejaGanadora { get; init; }
        public int Puntos { get; init; }
        public Jugador? AbrioLaRonda { get; init; }
        public bool HuboEmpate => Ganador is null;
    }
}
