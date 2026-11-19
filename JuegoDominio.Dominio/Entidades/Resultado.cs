using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Resultado
    {
        public TipoFin Fin { get; init; }
        public Jugador? Ganador { get; init; }
        public Pareja? ParejaGanadora { get; init; }
        public int Puntos { get; init; }
        public Jugador? UltimoEnJugar { get; init; }
        public bool HuboEmpate => Ganador is null && ParejaGanadora is null;
    }
}
