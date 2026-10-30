using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Interfaces;

namespace JuegoDomino.Consola.Vistas
{
    public class VistaConsola : IObservadorRonda
    {
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
