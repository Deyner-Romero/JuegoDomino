using JuegoDominio.Dominio.Interfaces;
using JuegoDominio.Dominio.Validaciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Ronda : IRonda
    {
        private readonly IReadOnlyList<Jugador> jugadores;
        private readonly IReadOnlyList<Pareja> parejas;
        private readonly IReglas reglas;
        private readonly ValidadorJugada validador;
        private readonly List<IObservadorRonda> observadores = new();
        private int posicionEnTurno;

        /// <param name="jugadores">Jugadores en orden de mesa, con sus fichas ya repartidas.</param>
        /// <param name="rondaAnterior">Resultado de la ronda anterior, o <c>null</c> si es la primera.</param>
        public Ronda(IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas, IReglas reglas, ValidadorJugada validador, Resultado? rondaAnterior = null)
        {
            this.jugadores = jugadores;
            this.parejas = parejas;
            this.reglas = reglas;
            this.validador = validador;

            if (rondaAnterior is null)
                FichaDeSalida = reglas.FichaDeSalida(jugadores);

            AbrioLaRonda = reglas.JugadorQueSale(jugadores, rondaAnterior);
            posicionEnTurno = jugadores.ToList().IndexOf(AbrioLaRonda);
        }

        public Tablero Tablero { get; } = new();
        public Ficha? FichaDeSalida { get; }
        public Jugador AbrioLaRonda { get; }
        public Jugador JugadorEnTurno => jugadores[posicionEnTurno];
        public int PasesSeguidos { get; private set; }
        public Resultado? Resultado { get; private set; }
        public bool Terminada => Resultado is not null;

        public void Suscribir(IObservadorRonda o)
        {
            if (!observadores.Contains(o))
                observadores.Add(o);
        }

        public void Desuscribir(IObservadorRonda o)
        {
            observadores.Remove(o);
        }

        public Resultado Jugar()
        {
            if (Terminada)
                throw new InvalidOperationException("La ronda ya terminó.");

            Resultado? resultado = null;

            while (resultado is null)
            {
                resultado = JugarTurno();

                if (resultado is null)
                    posicionEnTurno = (posicionEnTurno + 1) % jugadores.Count;
            }

            Resultado = resultado with { AbrioLaRonda = AbrioLaRonda };
            Notificar(o => o.RondaFinalizada(Resultado));
            return Resultado;
        }

        private Resultado? JugarTurno()
        {
            var jugador = JugadorEnTurno;
            var jugada = jugador.RealizarTurno(Tablero, validador, jugador, FichaDeSalida);

            if (jugada is null)
            {
                PasesSeguidos++;
                Notificar(o => o.TurnoPasado(jugador));

                return reglas.EsTranque(PasesSeguidos) ? reglas.ResolverTranque(jugadores, parejas) : null;
            }

            PasesSeguidos = 0;
            Notificar(o => o.FichaJugada(jugador, jugada));

            return jugador.SinFichas ? reglas.ResolverDomino(jugador, jugadores, parejas) : null;
        }

        private void Notificar(Action<IObservadorRonda> evento)
        {
            foreach (var observador in observadores.ToList())
                evento(observador);
        }
    }
}
