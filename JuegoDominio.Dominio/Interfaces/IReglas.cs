using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Interfaces
{
    public interface IReglas
    {
        int FichasPorJugador { get; }
        int PuntosParaGanar { get; }
        int NumeroDeJugadores { get; }
        TipoJuego TipoJuego { get; }
    }
}
