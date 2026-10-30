using JuegoDominio.Dominio.Entidades;

namespace JuegoDominio.Dominio.Interfaces
{
    public interface IObservadorRonda
    {
        void FichaJugada(Jugador j, Jugada jugada);
        void TurnoPasado(Jugador j);
        void RondaFinalizada(Resultado r);
    }
}
