using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Reglas;

namespace JuegoDomino.Pruebas.Dominio
{
    public class ReglasTradicionalesPruebas
    {
        private readonly ReglasTradicionales individual = new(TipoJuego.Individual, 3);
        private readonly ReglasTradicionales parejas = new(TipoJuego.Parejas, 4);

        [Fact]
        public void Constructor_UsaLosValoresPorDefecto()
        {
            Assert.Equal(7, individual.FichasPorJugador);
            Assert.Equal(80, individual.PuntosParaGanar);
        }

        [Theory]
        [InlineData(TipoJuego.Parejas, 3)]
        [InlineData(TipoJuego.Individual, 1)]
        [InlineData(TipoJuego.Individual, 5)]
        public void Constructor_RechazaUnNumeroDeJugadoresInvalido(TipoJuego tipo, int jugadores)
        {
            Assert.Throws<ArgumentException>(() => new ReglasTradicionales(tipo, jugadores));
        }

        [Fact]
        public void FichaDeSalida_EsElDobleSeisCuandoEstaEnJuego()
        {
            var jugadores = JugadoresCon([new Ficha(5, 5)], [new Ficha(6, 6)]);

            Assert.Equal(new Ficha(6, 6), individual.FichaDeSalida(jugadores));
        }

        [Fact]
        public void FichaDeSalida_SinDobleSeis_EsElDobleMasAlto()
        {
            var jugadores = JugadoresCon([new Ficha(2, 2), new Ficha(6, 5)], [new Ficha(4, 4)]);

            Assert.Equal(new Ficha(4, 4), individual.FichaDeSalida(jugadores));
        }

        [Fact]
        public void FichaDeSalida_SinDobles_EsLaFichaDeMasPuntos()
        {
            var jugadores = JugadoresCon([new Ficha(1, 2)], [new Ficha(6, 5)]);

            Assert.Equal(new Ficha(6, 5), individual.FichaDeSalida(jugadores));
        }

        [Fact]
        public void JugadorQueSale_EnLaPrimeraRonda_EsQuienTieneLaFichaDeSalida()
        {
            var jugadores = JugadoresCon([new Ficha(1, 2)], [new Ficha(6, 6)]);

            Assert.Same(jugadores[1], individual.JugadorQueSale(jugadores, null));
        }

        [Fact]
        public void JugadorQueSale_DespuesDeUnaRonda_EsElGanadorAnterior()
        {
            var jugadores = JugadoresCon([new Ficha(6, 6)], [new Ficha(1, 2)], [new Ficha(3, 3)]);
            var anterior = new Resultado { Ganador = jugadores[2], AbrioLaRonda = jugadores[0] };

            Assert.Same(jugadores[2], individual.JugadorQueSale(jugadores, anterior));
        }

        [Fact]
        public void JugadorQueSale_DespuesDeUnEmpate_EsElSiguienteAlQueAbrio()
        {
            var jugadores = JugadoresCon([new Ficha(6, 6)], [new Ficha(1, 2)], [new Ficha(3, 3)]);
            var anterior = new Resultado { Fin = TipoFin.Tranque, AbrioLaRonda = jugadores[2] };

            Assert.Same(jugadores[0], individual.JugadorQueSale(jugadores, anterior));
        }

        [Fact]
        public void EsTranque_CuandoTodosLosJugadoresPasanSeguidos()
        {
            Assert.False(individual.EsTranque(2));
            Assert.True(individual.EsTranque(3));
        }

        [Fact]
        public void ResolverDomino_ElGanadorSumaLosPuntosDeTodasLasManos()
        {
            var jugadores = JugadoresCon([], [new Ficha(6, 6)], [new Ficha(1, 2)]);

            var resultado = individual.ResolverDomino(jugadores[0], jugadores, []);

            Assert.Equal(TipoFin.Domino, resultado.Fin);
            Assert.Same(jugadores[0], resultado.Ganador);
            Assert.Equal(15, resultado.Puntos);
        }

        [Fact]
        public void ResolverDomino_EnParejas_IndicaLaParejaGanadora()
        {
            var jugadores = JugadoresCon([], [new Ficha(1, 1)], [new Ficha(2, 2)], [new Ficha(3, 3)]);
            var equipos = ParejasDe(jugadores);

            var resultado = parejas.ResolverDomino(jugadores[0], jugadores, equipos);

            Assert.Same(equipos[0], resultado.ParejaGanadora);
            Assert.Equal(12, resultado.Puntos);
        }

        [Fact]
        public void ResolverTranque_GanaElJugadorConMenosPuntos()
        {
            var jugadores = JugadoresCon([new Ficha(6, 6)], [new Ficha(1, 2)], [new Ficha(4, 4)]);

            var resultado = individual.ResolverTranque(jugadores, []);

            Assert.Equal(TipoFin.Tranque, resultado.Fin);
            Assert.Same(jugadores[1], resultado.Ganador);
            Assert.Equal(23, resultado.Puntos);
        }

        [Fact]
        public void ResolverTranque_ConEmpate_NadieAnota()
        {
            var jugadores = JugadoresCon([new Ficha(1, 2)], [new Ficha(0, 3)], [new Ficha(4, 4)]);

            var resultado = individual.ResolverTranque(jugadores, []);

            Assert.True(resultado.HuboEmpate);
            Assert.Equal(0, resultado.Puntos);
        }

        [Fact]
        public void ResolverTranque_EnParejas_GanaLaParejaConMenosPuntos()
        {
            var jugadores = JugadoresCon([new Ficha(6, 6)], [new Ficha(1, 2)], [new Ficha(5, 5)], [new Ficha(0, 1)]);
            var equipos = ParejasDe(jugadores);

            var resultado = parejas.ResolverTranque(jugadores, equipos);

            Assert.Same(equipos[1], resultado.ParejaGanadora);
            Assert.Same(jugadores[3], resultado.Ganador);
            Assert.Equal(26, resultado.Puntos);
        }

        [Fact]
        public void AlcanzoLaMeta_CuandoLlegaAlPuntajeObjetivo()
        {
            Assert.False(individual.AlcanzoLaMeta(79));
            Assert.True(individual.AlcanzoLaMeta(80));
        }

        private static List<Jugador> JugadoresCon(params Ficha[][] manos)
        {
            var jugadores = new List<Jugador>();

            for (int i = 0; i < manos.Length; i++)
            {
                var jugador = new JugadorDePrueba($"Jugador {i + 1}");
                jugador.RecibirFichas(manos[i]);
                jugadores.Add(jugador);
            }

            return jugadores;
        }

        private static List<Pareja> ParejasDe(List<Jugador> jugadores)
        {
            var parejaA = new Pareja("Pareja A");
            parejaA.Agregar(jugadores[0]);
            parejaA.Agregar(jugadores[2]);

            var parejaB = new Pareja("Pareja B");
            parejaB.Agregar(jugadores[1]);
            parejaB.Agregar(jugadores[3]);

            return [parejaA, parejaB];
        }
    }
}
