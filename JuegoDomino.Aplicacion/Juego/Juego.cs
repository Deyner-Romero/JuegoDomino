using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;
using JuegoDominio.Dominio.Reglas;
using JuegoDomino.Aplicacion.Builder;
using JuegoDomino.Aplicacion.Estrategia;
using JuegoDomino.Aplicacion.Interface;
using JuegoDomino.Aplicacion.Jugadores;
using JuegoDomino.Aplicacion.Observadores;

namespace JuegoDomino.Aplicacion.Juego
{
    public class Juego
    {
        private readonly List<IObservadorRonda> observadores = new();
        private readonly Random? azar;
        private Partida? partida;

        public Juego(Random? azar = null)
        {
            this.azar = azar;
        }

        public HistorialPartida Historial { get; private set; } = new();
        public bool HayPartida => partida is not null;
        public bool Terminada => partida?.Terminada ?? false;
        public int RondasJugadas => partida?.Resultados.Count ?? 0;
        public Tablero? Mesa => partida?.RondaActual?.Tablero;

        public Partida Partida => partida ?? throw new InvalidOperationException("No hay una partida creada.");

        public void Suscribir(IObservadorRonda observador)
        {
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void NuevaPartidaIndividual(Jugador humano, int numeroDeJugadores, Dificultad dificultad, int puntosParaGanar = 80)
        {
            var builder = NuevoBuilder(new ReglasTradicionales(TipoJuego.Individual, numeroDeJugadores, puntosParaGanar))
                .AgregarJugador(humano);

            for (int i = 1; i < numeroDeJugadores; i++)
                builder.AgregarJugador(CrearBot($"Bot {i}", dificultad));

            partida = builder.Build();
        }

        public void NuevaPartidaParejas(Jugador humano, Dificultad dificultad, int puntosParaGanar = 80)
        {
            partida = NuevoBuilder(new ReglasTradicionales(TipoJuego.Parejas, 4, puntosParaGanar))
                .AgregarPareja("Nosotros", humano, CrearBot("Compañero", dificultad))
                .AgregarPareja("Ellos", CrearBot("Rival 1", dificultad), CrearBot("Rival 2", dificultad))
                .Build();
        }

        public Resultado JugarRonda()
        {
            if (Terminada)
                throw new InvalidOperationException("La partida ya terminó.");

            return Partida.JugarRonda();
        }

        public void Jugar() => Partida.JugarHastaTerminar();

        public IReadOnlyList<(string Nombre, int Puntos)> Marcador()
        {
            var actual = Partida;

            var marcador = actual.Reglas.TipoJuego == TipoJuego.Parejas
                ? actual.Parejas.Select(pareja => (pareja.Nombre, actual.PuntosDe(pareja)))
                : actual.Jugadores.Select(jugador => (jugador.Nombre, actual.PuntosDe(jugador)));

            return marcador.OrderByDescending(fila => fila.Item2).ToList();
        }

        public string? NombreDelGanador() => Partida.ParejaGanadora?.Nombre ?? Partida.Ganador?.Nombre;

        private PartidaBuilder NuevoBuilder(ReglasTradicionales reglas)
        {
            Historial = new HistorialPartida();

            var builder = new PartidaBuilder()
                .ConReglas(reglas)
                .ConObservador(Historial);

            foreach (var observador in observadores)
                builder.ConObservador(observador);

            if (azar is not null)
                builder.ConAzar(azar);

            return builder;
        }

        private JugadorAutomatico CrearBot(string nombre, Dificultad dificultad) => new(nombre, CrearEstrategia(dificultad));

        private IEstrategiaJugador CrearEstrategia(Dificultad dificultad) => dificultad switch
        {
            Dificultad.Facil => new EstrategiaAleatorea(azar),
            Dificultad.Normal => new EstrategiaDescartarPuntos(),
            Dificultad.Dificil => new EstrategiaDefensiva(),
            _ => throw new ArgumentOutOfRangeException(nameof(dificultad))
        };
    }
}
