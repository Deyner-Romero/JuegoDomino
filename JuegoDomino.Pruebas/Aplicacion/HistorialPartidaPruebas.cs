using JuegoDomino.Aplicacion.Juego;
using JuegoDomino.Pruebas.Dominio;

namespace JuegoDomino.Pruebas.Aplicacion
{
    public class HistorialPartidaPruebas
    {
        [Fact]
        public void RegistraJugadasPasesYRondasDeLaPartida()
        {
            var juego = new Juego(new Random(9));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 3, Dificultad.Normal);

            juego.Jugar();

            var historial = juego.Historial;
            Assert.Equal(juego.RondasJugadas, historial.Resultados.Count);
            Assert.NotEmpty(historial.Jugadas);
            Assert.Equal(historial.Jugadas.Count + historial.Pases + historial.Resultados.Count + 1, historial.Eventos.Count);
        }

        [Fact]
        public void ElResumen_TieneUnaLineaPorRondaYAnunciaAlGanador()
        {
            var juego = new Juego(new Random(9));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);

            juego.Jugar();

            var resumen = juego.Historial.Resumen();
            Assert.Contains($"Ronda {juego.RondasJugadas}:", resumen);
            Assert.Contains($"Ganó {juego.NombreDelGanador()}", resumen);
        }
    }
}
