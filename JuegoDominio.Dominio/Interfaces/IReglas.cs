using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Interfaces
{
    public interface IReglas
    {
        int FichasPorJugador { get; }
        int PuntosParaGanar { get; }
        int NumeroDeJugadores { get; }
        TipoJuego TipoJuego { get; }

        Ficha FichaDeSalida(IReadOnlyList<Jugador> jugadores);
        Jugador JugadorQueSale(IReadOnlyList<Jugador> jugadores, Resultado? rondaAnterior);
        bool EsTranque(int pasesSeguidos);
        Resultado ResolverDomino(Jugador ganador, IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas);
        Resultado ResolverTranque(IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas);
        bool AlcanzoLaMeta(int puntos);
    }
}
