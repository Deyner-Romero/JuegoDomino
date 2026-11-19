using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;
using JuegoDominio.Dominio.Validaciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Partida : IObservadorRonda
    {
        private readonly Dictionary<Jugador, int> marcador = new();
        private readonly Dictionary<Pareja, int> marcadorPorPareja = new();
        private readonly List<Resultado> resultados = new();
        private readonly List<IObservadorRonda> observadores = new();
        private readonly ValidadorJugada validador;
        private readonly Random? azar;

        public Partida(IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas, IReglas reglas, ValidadorJugada validador, Random? azar = null)
        {
            if (jugadores.Count != reglas.NumeroDeJugadores)
                throw new ArgumentException($"Las reglas exigen {reglas.NumeroDeJugadores} jugadores y se recibieron {jugadores.Count}.", nameof(jugadores));

            if (reglas.TipoJuego == TipoJuego.Parejas && parejas.Count != 2)
                throw new ArgumentException("El juego en parejas requiere exactamente 2 parejas.", nameof(parejas));

            Jugadores = jugadores;
            Parejas = parejas;
            Reglas = reglas;
            this.validador = validador;
            this.azar = azar;

            foreach (var jugador in jugadores)
                marcador[jugador] = 0;

            foreach (var pareja in parejas)
                marcadorPorPareja[pareja] = 0;
        }

        public EstadoPartida Estado { get; private set; } = EstadoPartida.EnCurso;
        public IReadOnlyList<Jugador> Jugadores { get; }
        public IReadOnlyList<Pareja> Parejas { get; }
        public IReglas Reglas { get; }
        public IReadOnlyList<Resultado> Resultados => resultados;
        public Jugador? Ganador { get; private set; }
        public Pareja? ParejaGanadora { get; private set; }
        public bool Terminada => Estado == EstadoPartida.Finalizada;
        public Ronda? RondaActual { get; private set; }

        public int PuntosDe(Jugador jugador) => marcador.GetValueOrDefault(jugador);

        public int PuntosDe(Pareja pareja) => marcadorPorPareja.GetValueOrDefault(pareja);

        public void Suscribir(IObservadorRonda observador)
        {
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void Desuscribir(IObservadorRonda observador) => observadores.Remove(observador);

        public Ronda CrearRonda()
        {
            if (Terminada)
                throw new InvalidOperationException("La partida ya terminó.");

            new JuegoDeFichas(azar: azar).Repartir(Jugadores, Reglas.FichasPorJugador);

            var ronda = new Ronda(Jugadores, Parejas, Reglas, validador, resultados.LastOrDefault());
            RondaActual = ronda;

            // La partida se suscribe primero para que el marcador ya esté al día
            // cuando los demás observadores reciban RondaFinalizada.
            ronda.Suscribir(this);
            foreach (var observador in observadores)
                ronda.Suscribir(observador);

            return ronda;
        }

        public Resultado JugarRonda()
        {
            var ronda = CrearRonda();
            var resultado = ronda.Jugar();

            ronda.Desuscribir(this);
            foreach (var observador in observadores)
                ronda.Desuscribir(observador);

            if (Terminada)
                Notificar(o => o.PartidaFinalizada(this));

            return resultado;
        }

        public void JugarHastaTerminar()
        {
            while (!Terminada)
                JugarRonda();
        }

        public void FichaJugada(Jugador j, Jugada jugada)
        {
        }

        public void TurnoPasado(Jugador j)
        {
        }

        public void RondaFinalizada(Resultado r)
        {
            resultados.Add(r);

            if (r.HuboEmpate)
                return;

            if (Reglas.TipoJuego == TipoJuego.Parejas)
                AnotarPareja(r.ParejaGanadora!, r.Puntos);
            else
                AnotarJugador(r.Ganador!, r.Puntos);
        }

        public void PartidaFinalizada(Partida partida)
        {
        }

        private void AnotarJugador(Jugador jugador, int puntos)
        {
            marcador[jugador] += puntos;

            if (Reglas.AlcanzoLaMeta(marcador[jugador]))
            {
                Ganador = jugador;
                Estado = EstadoPartida.Finalizada;
            }
        }

        private void AnotarPareja(Pareja pareja, int puntos)
        {
            marcadorPorPareja[pareja] += puntos;

            if (Reglas.AlcanzoLaMeta(marcadorPorPareja[pareja]))
            {
                ParejaGanadora = pareja;
                Estado = EstadoPartida.Finalizada;
            }
        }

        private void Notificar(Action<IObservadorRonda> evento)
        {
            foreach (var observador in observadores.ToList())
                evento(observador);
        }
    }
}
