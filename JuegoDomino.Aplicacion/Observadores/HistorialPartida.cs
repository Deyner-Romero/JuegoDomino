using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Interfaces;

namespace JuegoDomino.Aplicacion.Observadores
{
    public class HistorialPartida : IObservadorRonda
    {
        private List<Jugada> historial = new();

        public void FichaJugada(Jugador j, Jugada jugada)
        {
            throw new NotImplementedException();
        }

        public void TurnoPasado(Jugador j)
        {
            throw new NotImplementedException();
        }

        public void RondaFinalizada(Resultado r)
        {
            throw new NotImplementedException();
        }
    }
}
