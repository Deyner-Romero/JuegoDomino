using JuegoDominio.Dominio.Interfaces;

namespace JuegoDominio.Dominio.Entidades
{
    public class Ronda
    {
        private IReadOnlyList<Jugador> jugadores = new List<Jugador>();
        private IReadOnlyList<Pareja> parejas = new List<Pareja>();
        private IReglas? reglas;

        public Tablero Tablero { get; private set; } = new();

        public event EventHandler? TurnoJugado;

        public void Iniciar(Ronda? rondaAnterior)
        {
            throw new NotImplementedException();
        }

        public Resultado Finalizar()
        {
            throw new NotImplementedException();
        }
    }
}
