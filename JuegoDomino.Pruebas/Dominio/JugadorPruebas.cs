using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class JugadorPruebas
    {
        [Fact]
        public void RecibirFichas_ReemplazaLaManoAnterior()
        {
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(1, 1)]);

            jugador.RecibirFichas([new Ficha(2, 3), new Ficha(4, 4)]);

            Assert.Equal(2, jugador.FichasRestantes);
            Assert.False(jugador.Tiene(new Ficha(1, 1)));
        }

        [Fact]
        public void PuntosEnMano_SumaLosPuntosDeSusFichas()
        {
            var jugador = new JugadorDePrueba();

            jugador.RecibirFichas([new Ficha(2, 3), new Ficha(6, 6)]);

            Assert.Equal(17, jugador.PuntosEnMano);
        }

        [Fact]
        public void Jugar_QuitaLaFichaDeLaMano()
        {
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(2, 3)]);

            jugador.Jugar(new Ficha(3, 2));

            Assert.True(jugador.SinFichas);
        }

        [Fact]
        public void Jugar_UnaFichaQueNoTiene_LanzaExcepcion()
        {
            var jugador = new JugadorDePrueba();
            jugador.RecibirFichas([new Ficha(2, 3)]);

            Assert.Throws<InvalidOperationException>(() => jugador.Jugar(new Ficha(5, 5)));
        }
    }
}
