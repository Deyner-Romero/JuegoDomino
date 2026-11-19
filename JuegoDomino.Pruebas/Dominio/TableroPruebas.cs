using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDomino.Pruebas.Dominio
{
    public class TableroPruebas
    {
        [Fact]
        public void TableroNuevo_EstaVacioYSinExtremos()
        {
            var tablero = new Tablero();

            Assert.True(tablero.EstaVacia);
            Assert.Null(tablero.ExtremoIzquierdo);
            Assert.Null(tablero.ExtremoDerecho);
        }

        [Fact]
        public void PosiblesJugadas_EnTableroVacio_PermiteCualquierFicha()
        {
            var tablero = new Tablero();
            var mano = new[] { new Ficha(1, 2), new Ficha(3, 4) };

            Assert.Equal(2, tablero.PosiblesJugadas(mano).Count);
        }

        [Fact]
        public void Registrar_PrimeraFicha_DefineAmbosExtremos()
        {
            var tablero = new Tablero();

            tablero.Registrar(new Jugada(new Ficha(6, 6), Extremo.Derecho));

            Assert.Equal(6, tablero.ExtremoIzquierdo);
            Assert.Equal(6, tablero.ExtremoDerecho);
        }

        [Fact]
        public void Registrar_PorLaDerecha_OrientaLaFichaYActualizaElExtremo()
        {
            var tablero = TableroCon(new Ficha(6, 3));

            tablero.Registrar(new Jugada(new Ficha(1, 3), Extremo.Derecho));

            Assert.Equal("[6|3][3|1]", tablero.ToString());
            Assert.Equal(1, tablero.ExtremoDerecho);
        }

        [Fact]
        public void Registrar_PorLaIzquierda_OrientaLaFichaYActualizaElExtremo()
        {
            var tablero = TableroCon(new Ficha(6, 3));

            tablero.Registrar(new Jugada(new Ficha(6, 2), Extremo.Izquierdo));

            Assert.Equal("[2|6][6|3]", tablero.ToString());
            Assert.Equal(2, tablero.ExtremoIzquierdo);
        }

        [Fact]
        public void Registrar_FichaQueNoCoincide_LanzaExcepcion()
        {
            var tablero = TableroCon(new Ficha(6, 3));

            Assert.Throws<InvalidOperationException>(() => tablero.Registrar(new Jugada(new Ficha(1, 2), Extremo.Derecho)));
        }

        [Fact]
        public void PosiblesJugadas_SoloIncluyeFichasQueCoincidenConUnExtremo()
        {
            var tablero = TableroCon(new Ficha(6, 3));
            var mano = new[] { new Ficha(6, 1), new Ficha(3, 2), new Ficha(4, 5) };

            var posibles = tablero.PosiblesJugadas(mano);

            Assert.Equal(2, posibles.Count);
            Assert.Contains(posibles, jugada => jugada.Ficha.Equals(new Ficha(6, 1)) && jugada.Extremo == Extremo.Izquierdo);
            Assert.Contains(posibles, jugada => jugada.Ficha.Equals(new Ficha(3, 2)) && jugada.Extremo == Extremo.Derecho);
        }

        [Fact]
        public void PosiblesJugadas_FichaQueCabeEnAmbosExtremos_OfreceLasDosOpciones()
        {
            var tablero = TableroCon(new Ficha(6, 3));

            var posibles = tablero.PosiblesJugadas([new Ficha(6, 3)]);

            Assert.Equal(2, posibles.Count);
        }

        private static Tablero TableroCon(Ficha primera)
        {
            var tablero = new Tablero();
            tablero.Registrar(new Jugada(primera, Extremo.Derecho));
            return tablero;
        }
    }
}
