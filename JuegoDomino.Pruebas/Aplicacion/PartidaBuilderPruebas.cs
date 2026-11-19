using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Reglas;
using JuegoDomino.Aplicacion.Builder;
using JuegoDomino.Pruebas.Dominio;

namespace JuegoDomino.Pruebas.Aplicacion
{
    public class PartidaBuilderPruebas
    {
        private static ReglasTradicionales Individual(int jugadores) => new(TipoJuego.Individual, jugadores);
        private static ReglasTradicionales Parejas() => new(TipoJuego.Parejas, 4);

        [Fact]
        public void SinReglas_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .AgregarJugador(new JugadorDePrueba("Ana"))
                .AgregarJugador(new JugadorDePrueba("Luis"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void ConUnSoloJugador_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Individual(2))
                .AgregarJugador(new JugadorDePrueba("Ana"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void ConMasJugadoresDeLosQueDicenLasReglas_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Individual(2))
                .AgregarJugador(new JugadorDePrueba("Ana"))
                .AgregarJugador(new JugadorDePrueba("Luis"))
                .AgregarJugador(new JugadorDePrueba("Sara"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void ConNombresRepetidos_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Individual(2))
                .AgregarJugador(new JugadorDePrueba("Ana"))
                .AgregarJugador(new JugadorDePrueba(" ana "));

            var error = Assert.Throws<InvalidOperationException>(() => builder.Build());
            Assert.Contains("repetido", error.Message);
        }

        [Fact]
        public void EnParejas_ConUnaSolaPareja_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Parejas())
                .AgregarPareja("A", new JugadorDePrueba("A1"), new JugadorDePrueba("A2"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void EnParejas_ConJugadoresSueltos_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Parejas())
                .AgregarPareja("A", new JugadorDePrueba("A1"), new JugadorDePrueba("A2"))
                .AgregarPareja("B", new JugadorDePrueba("B1"), new JugadorDePrueba("B2"))
                .AgregarJugador(new JugadorDePrueba("Suelto"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void Individual_ConParejas_NoConstruye()
        {
            var builder = new PartidaBuilder()
                .ConReglas(Individual(4))
                .AgregarPareja("A", new JugadorDePrueba("A1"), new JugadorDePrueba("A2"))
                .AgregarPareja("B", new JugadorDePrueba("B1"), new JugadorDePrueba("B2"));

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void Individual_RespetaElOrdenEnQueSeAgreganLosJugadores()
        {
            var partida = new PartidaBuilder()
                .ConReglas(Individual(3))
                .AgregarJugador(new JugadorDePrueba("Ana"))
                .AgregarJugador(new JugadorDePrueba("Luis"))
                .AgregarJugador(new JugadorDePrueba("Sara"))
                .Build();

            Assert.Equal(["Ana", "Luis", "Sara"], partida.Jugadores.Select(j => j.Nombre));
            Assert.Empty(partida.Parejas);
        }

        [Fact]
        public void EnParejas_SientaALosCompanerosEnfrentados()
        {
            var partida = new PartidaBuilder()
                .ConReglas(Parejas())
                .AgregarPareja("A", new JugadorDePrueba("A1"), new JugadorDePrueba("A2"))
                .AgregarPareja("B", new JugadorDePrueba("B1"), new JugadorDePrueba("B2"))
                .Build();

            Assert.Equal(["A1", "B1", "A2", "B2"], partida.Jugadores.Select(j => j.Nombre));
            Assert.Equal(2, partida.Parejas.Count);
        }

        [Fact]
        public void LaPartidaConstruida_SeJuegaCompletaYAvisaALosObservadores()
        {
            var observador = new ObservadorDePrueba();
            var partida = new PartidaBuilder()
                .ConReglas(Individual(2))
                .AgregarJugador(new JugadorDePrueba("Ana"))
                .AgregarJugador(new JugadorDePrueba("Luis"))
                .ConObservador(observador)
                .ConAzar(new Random(7))
                .Build();

            partida.JugarHastaTerminar();

            Assert.True(partida.Terminada);
            Assert.Equal(partida.Resultados.Count, observador.Resultados.Count);
            Assert.Equal(1, observador.PartidasFinalizadas);
        }
    }
}
