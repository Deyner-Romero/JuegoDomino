using JuegoDominio.Dominio.Interfaces;

namespace JuegoDominio.Dominio.Entidades
{
    public class Ronda : IRonda
    {
        private IReadOnlyList<Jugador> jugadores = new List<Jugador>();
        private IReadOnlyList<Pareja> parejas = new List<Pareja>();
        private IReglas? reglas;
        private List<IObservadorRonda> observadores = new();

        public Tablero Tablero { get; private set; } = new();

        public void Suscribir(IObservadorRonda o)
        {
            observadores.Add(o);
        }

        public void Desuscribir(IObservadorRonda o)
        {
            observadores.Remove(o);
        }

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
