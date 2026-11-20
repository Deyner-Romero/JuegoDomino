using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDomino.Aplicacion.Fabricas;
using JuegoDomino.Pruebas.Dominio;

namespace JuegoDomino.Pruebas.Aplicacion
{
    public class CreadorBotPruebas
    {
        // En la mesa está [3|5]. El bot normal descarta la ficha de más puntos ([2|5]);
        // el difícil juega [3|1] a la izquierda para dejar abiertos el 1 y el 5.
        private static Jugada? TurnoDe(CreadorBot creador)
        {
            var tablero = new Tablero();
            tablero.Registrar(new Jugada(new Ficha(3, 5), Extremo.Derecho));

            var bot = creador.Crear("Bot");
            bot.RecibirFichas([new(3, 1), new(2, 5), new(1, 4), new(1, 6), new(1, 0), new(2, 2)]);

            return bot.RealizarTurno(tablero, Cadena.DeValidacion(), bot);
        }

        [Fact]
        public void CreadorBotNormal_CreaUnBotQueDescartaLaFichaDeMasPuntos()
        {
            var jugada = TurnoDe(new CreadorBotNormal());

            Assert.Equal(new Ficha(2, 5), jugada!.Ficha);
        }

        [Fact]
        public void CreadorBotDificil_CreaUnBotQueJuegaDefensivo()
        {
            var jugada = TurnoDe(new CreadorBotDificil());

            Assert.Equal(new Ficha(3, 1), jugada!.Ficha);
            Assert.Equal(Extremo.Izquierdo, jugada.Extremo);
        }

        [Fact]
        public void CreadorBotFacil_CreaUnBotQueJuegaUnaFichaValida()
        {
            var jugada = TurnoDe(new CreadorBotFacil(new Random(7)));

            Assert.True(jugada!.Ficha.ContieneValor(3) || jugada.Ficha.ContieneValor(5));
        }

        [Fact]
        public void CadaLlamadaACrear_DevuelveUnBotNuevoConSuNombre()
        {
            var creador = new CreadorBotNormal();

            var primero = creador.Crear("Rival 1");
            var segundo = creador.Crear("Rival 2");

            Assert.NotSame(primero, segundo);
            Assert.Equal("Rival 1", primero.Nombre);
            Assert.Equal("Rival 2", segundo.Nombre);
        }
    }
}
