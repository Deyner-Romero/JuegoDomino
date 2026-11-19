using JuegoDomino.Aplicacion.Juego;
using JuegoDomino.Pruebas.Dominio;

namespace JuegoDomino.Pruebas.Aplicacion
{
    public class JuegoPruebas
    {
        [Theory]
        [InlineData(2, Dificultad.Facil)]
        [InlineData(3, Dificultad.Normal)]
        [InlineData(4, Dificultad.Dificil)]
        public void PartidaIndividualDesdeLaFachada_TerminaConUnGanador(int numeroDeJugadores, Dificultad dificultad)
        {
            var juego = new Juego(new Random(11));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), numeroDeJugadores, dificultad);

            juego.Jugar();

            Assert.True(juego.Terminada);
            Assert.Equal(numeroDeJugadores, juego.Marcador().Count);
            Assert.Equal(juego.Marcador()[0].Nombre, juego.NombreDelGanador());
            Assert.True(juego.Marcador()[0].Puntos >= 80);
        }

        [Fact]
        public void PartidaEnParejasDesdeLaFachada_ElHumanoJuegaConUnCompanero()
        {
            var ana = new JugadorDePrueba("Ana");
            var juego = new Juego(new Random(11));
            juego.NuevaPartidaParejas(ana, Dificultad.Normal);

            juego.Jugar();

            var nosotros = juego.Partida.Parejas.Single(pareja => pareja.Incluye(ana));
            Assert.Equal("Nosotros", nosotros.Nombre);
            Assert.Equal(["Ana", "Rival 1", "Compañero", "Rival 2"], juego.Partida.Jugadores.Select(j => j.Nombre));
            Assert.Contains(juego.NombreDelGanador(), new[] { "Nosotros", "Ellos" });
        }

        [Fact]
        public void JugarRondaARonda_AvanzaHastaQueLaPartidaTermina()
        {
            var juego = new Juego(new Random(3));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);

            while (!juego.Terminada)
                juego.JugarRonda();

            Assert.NotNull(juego.NombreDelGanador());
            Assert.Throws<InvalidOperationException>(() => juego.JugarRonda());
        }

        [Fact]
        public void SinPartida_NoSePuedeJugar()
        {
            var juego = new Juego();

            Assert.False(juego.HayPartida);
            Assert.Throws<InvalidOperationException>(() => juego.Jugar());
        }

        [Fact]
        public void LosObservadoresSuscritos_RecibenLosEventosDeLaPartida()
        {
            var observador = new ObservadorDePrueba();
            var juego = new Juego(new Random(5));
            juego.Suscribir(observador);
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);

            juego.Jugar();

            Assert.Equal(juego.RondasJugadas, observador.Resultados.Count);
            Assert.Equal(1, observador.PartidasFinalizadas);
        }

        [Fact]
        public void CadaPartidaNueva_EmpiezaConUnHistorialLimpio()
        {
            var juego = new Juego(new Random(5));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);
            juego.Jugar();

            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);

            Assert.Empty(juego.Historial.Eventos);
            Assert.Equal(0, juego.RondasJugadas);
        }
    }
}
