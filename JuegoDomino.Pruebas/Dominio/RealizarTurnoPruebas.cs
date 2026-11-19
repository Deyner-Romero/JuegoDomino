using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDomino.Pruebas.Dominio
{
    public class RealizarTurnoPruebas
    {
        private readonly Tablero tablero = new();

        [Fact]
        public void SinJugadasPosibles_ElJugadorPasa()
        {
            tablero.Registrar(new Jugada(new Ficha(6, 6), Extremo.Derecho));
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(1, 2)]);

            var jugada = jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador);

            Assert.Null(jugada);
            Assert.Equal(1, jugador.FichasRestantes);
        }

        [Fact]
        public void ConJugadaValida_LaRegistraEnElTableroYLaQuitaDeLaMano()
        {
            tablero.Registrar(new Jugada(new Ficha(6, 6), Extremo.Derecho));
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(6, 3), new Ficha(1, 1)]);

            var jugada = jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador);

            Assert.Equal(new Ficha(6, 3), jugada!.Ficha);
            Assert.False(jugador.Tiene(new Ficha(6, 3)));
            Assert.Equal(2, tablero.Linea.Count);
        }

        [Fact]
        public void EnLaSalida_SoloOfreceLaFichaDeSalida()
        {
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(1, 2), new Ficha(6, 6)]);

            var jugada = jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador, fichaDeSalida: new Ficha(6, 6));

            Assert.Equal(new Ficha(6, 6), jugada!.Ficha);
        }

        [Fact]
        public void UnBotQueEligeUnaJugadaInvalida_LanzaExcepcion()
        {
            var jugador = new JugadorTramposo();
            jugador.RecibirFichas([new Ficha(6, 6)]);

            Assert.Throws<InvalidOperationException>(() => jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador));
            Assert.True(tablero.EstaVacia);
        }

        [Fact]
        public void UnJugadorQueSobrescribeElRechazo_VuelveAElegirHastaQueLaJugadaEsValida()
        {
            var jugador = new JugadorQueSeEquivoca();
            jugador.RecibirFichas([new Ficha(6, 6)]);

            var jugada = jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador);

            Assert.Equal(new Ficha(6, 6), jugada!.Ficha);
            Assert.Single(jugador.MensajesRecibidos);
        }

        [Fact]
        public void SiNoEsSuTurno_LaJugadaSeRechaza()
        {
            var jugador = new JugadorDePrueba("Ana");
            var otro = new JugadorDePrueba("Luis");
            jugador.RecibirFichas([new Ficha(6, 6)]);

            var error = Assert.Throws<InvalidOperationException>(() => jugador.RealizarTurno(tablero, Cadena.DeValidacion(), otro));

            Assert.Contains("Luis", error.Message);
        }
    }
}
