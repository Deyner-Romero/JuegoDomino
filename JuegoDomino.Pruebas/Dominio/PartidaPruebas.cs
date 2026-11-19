using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Reglas;

namespace JuegoDomino.Pruebas.Dominio
{
    public class PartidaPruebas
    {
        private const int Semilla = 2026;

        private static Partida PartidaIndividual(int semilla = Semilla) =>
            new(
                [new JugadorDePrueba("Ana"), new JugadorDePrueba("Luis"), new JugadorDePrueba("Sara")],
                [],
                new ReglasTradicionales(TipoJuego.Individual, numeroDeJugadores: 3),
                Cadena.DeValidacion(),
                new Random(semilla));

        private static Partida PartidaEnParejas()
        {
            var a1 = new JugadorDePrueba("A1");
            var b1 = new JugadorDePrueba("B1");
            var a2 = new JugadorDePrueba("A2");
            var b2 = new JugadorDePrueba("B2");

            var parejaA = new Pareja("A");
            parejaA.Agregar(a1);
            parejaA.Agregar(a2);

            var parejaB = new Pareja("B");
            parejaB.Agregar(b1);
            parejaB.Agregar(b2);

            return new Partida(
                [a1, b1, a2, b2],
                [parejaA, parejaB],
                new ReglasTradicionales(TipoJuego.Parejas, numeroDeJugadores: 4),
                Cadena.DeValidacion(),
                new Random(Semilla));
        }

        [Fact]
        public void PartidaIndividualEntreBots_TerminaConUnGanadorQueAlcanzaLaMeta()
        {
            var partida = PartidaIndividual();

            partida.JugarHastaTerminar();

            Assert.Equal(EstadoPartida.Finalizada, partida.Estado);
            Assert.NotNull(partida.Ganador);
            Assert.True(partida.PuntosDe(partida.Ganador!) >= partida.Reglas.PuntosParaGanar);
            Assert.Equal(partida.Resultados.Where(r => r.Ganador == partida.Ganador).Sum(r => r.Puntos), partida.PuntosDe(partida.Ganador!));
        }

        [Fact]
        public void PartidaEnParejasEntreBots_LaGanaUnaPareja()
        {
            var partida = PartidaEnParejas();

            partida.JugarHastaTerminar();

            Assert.Equal(EstadoPartida.Finalizada, partida.Estado);
            Assert.NotNull(partida.ParejaGanadora);
            Assert.Null(partida.Ganador);
            Assert.True(partida.PuntosDe(partida.ParejaGanadora!) >= partida.Reglas.PuntosParaGanar);
        }

        [Fact]
        public void ConLaMismaSemilla_LaPartidaSeRepiteIgual()
        {
            var primera = PartidaIndividual();
            var segunda = PartidaIndividual();

            primera.JugarHastaTerminar();
            segunda.JugarHastaTerminar();

            Assert.Equal(primera.Resultados.Count, segunda.Resultados.Count);
            Assert.Equal(primera.Ganador!.Nombre, segunda.Ganador!.Nombre);
            Assert.Equal(primera.Resultados.Select(r => r.Puntos), segunda.Resultados.Select(r => r.Puntos));
        }

        [Fact]
        public void LosObservadores_RecibenCadaRondaYElFinDeLaPartida()
        {
            var partida = PartidaIndividual();
            var observador = new ObservadorDePrueba();
            partida.Suscribir(observador);

            partida.JugarHastaTerminar();

            Assert.Equal(partida.Resultados.Count, observador.Resultados.Count);
            Assert.Equal(1, observador.PartidasFinalizadas);
        }

        [Fact]
        public void CadaRonda_ReparteFichasNuevas()
        {
            var partida = PartidaIndividual();

            var ronda = partida.CrearRonda();

            Assert.All(partida.Jugadores, jugador => Assert.Equal(7, jugador.FichasRestantes));
            Assert.True(ronda.Tablero.EstaVacia);
        }

        [Fact]
        public void UnaPartidaTerminada_NoCreaMasRondas()
        {
            var partida = PartidaIndividual();
            partida.JugarHastaTerminar();

            Assert.Throws<InvalidOperationException>(() => partida.CrearRonda());
        }

        [Fact]
        public void ConUnNumeroDeJugadoresDistintoAlDeLasReglas_LanzaExcepcion()
        {
            Assert.Throws<ArgumentException>(() => new Partida(
                [new JugadorDePrueba("Ana")],
                [],
                new ReglasTradicionales(TipoJuego.Individual, numeroDeJugadores: 2),
                Cadena.DeValidacion()));
        }
    }
}
