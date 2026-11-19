using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Reglas;

namespace JuegoDomino.Pruebas.Dominio
{
    public class RondaPruebas
    {
        private readonly JugadorDePrueba ana = new("Ana");
        private readonly JugadorDePrueba luis = new("Luis");
        private readonly ReglasTradicionales reglas = new(TipoJuego.Individual, numeroDeJugadores: 2);

        private Ronda NuevaRonda(Resultado? rondaAnterior = null) =>
            new([ana, luis], [], reglas, Cadena.DeValidacion(), rondaAnterior);

        [Fact]
        public void LaPrimeraRonda_LaAbreQuienTieneLaFichaDeSalida()
        {
            ana.RecibirFichas([new Ficha(1, 2)]);
            luis.RecibirFichas([new Ficha(6, 6)]);

            var ronda = NuevaRonda();

            Assert.Equal(new Ficha(6, 6), ronda.FichaDeSalida);
            Assert.Same(luis, ronda.JugadorEnTurno);
        }

        [Fact]
        public void LasRondasSiguientes_LasAbreElGanadorAnteriorSinFichaDeSalida()
        {
            ana.RecibirFichas([new Ficha(1, 2)]);
            luis.RecibirFichas([new Ficha(6, 6)]);

            var ronda = NuevaRonda(new Resultado { Fin = TipoFin.Domino, Ganador = ana, Puntos = 10, AbrioLaRonda = luis });

            Assert.Null(ronda.FichaDeSalida);
            Assert.Same(ana, ronda.JugadorEnTurno);
        }

        [Fact]
        public void TerminaPorDomino_CuandoUnJugadorSeQuedaSinFichas()
        {
            // Ana abre con [6|6], Luis pone [6|2] y Ana se queda sin fichas con [6|1].
            ana.RecibirFichas([new Ficha(6, 6), new Ficha(6, 1)]);
            luis.RecibirFichas([new Ficha(6, 2), new Ficha(0, 3)]);

            var resultado = NuevaRonda().Jugar();

            Assert.Equal(TipoFin.Domino, resultado.Fin);
            Assert.Same(ana, resultado.Ganador);
            Assert.Same(ana, resultado.AbrioLaRonda);
            Assert.Equal(3, resultado.Puntos);
        }

        [Fact]
        public void TerminaPorTranque_CuandoTodosPasanSeguidos()
        {
            // Ana abre con [6|6] y después nadie tiene un 6.
            ana.RecibirFichas([new Ficha(6, 6), new Ficha(1, 1)]);
            luis.RecibirFichas([new Ficha(2, 2), new Ficha(3, 3)]);

            var ronda = NuevaRonda();
            var resultado = ronda.Jugar();

            Assert.Equal(TipoFin.Tranque, resultado.Fin);
            Assert.Same(ana, resultado.Ganador);
            Assert.Equal(12, resultado.Puntos);
            Assert.Equal(2, ronda.PasesSeguidos);
        }

        [Fact]
        public void NotificaJugadasPasesYFinAlObservador()
        {
            ana.RecibirFichas([new Ficha(6, 6), new Ficha(1, 1)]);
            luis.RecibirFichas([new Ficha(2, 2), new Ficha(3, 3)]);
            var observador = new ObservadorDePrueba();
            var ronda = NuevaRonda();
            ronda.Suscribir(observador);

            ronda.Jugar();

            Assert.Equal(["Ana juega [6|6]", "Luis pasa", "Ana pasa", "fin por Tranque"], observador.Eventos);
        }

        [Fact]
        public void UnObservadorDesuscrito_NoRecibeEventos()
        {
            ana.RecibirFichas([new Ficha(6, 6)]);
            luis.RecibirFichas([new Ficha(1, 2)]);
            var observador = new ObservadorDePrueba();
            var ronda = NuevaRonda();
            ronda.Suscribir(observador);
            ronda.Desuscribir(observador);

            ronda.Jugar();

            Assert.Empty(observador.Eventos);
        }

        [Fact]
        public void UnaRondaTerminada_NoSePuedeVolverAJugar()
        {
            ana.RecibirFichas([new Ficha(6, 6)]);
            luis.RecibirFichas([new Ficha(1, 2)]);
            var ronda = NuevaRonda();
            ronda.Jugar();

            Assert.Throws<InvalidOperationException>(() => ronda.Jugar());
        }
    }
}
