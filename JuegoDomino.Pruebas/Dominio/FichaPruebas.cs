using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class FichaPruebas
    {
        [Fact]
        public void Puntos_SumaAmbosValores()
        {
            Assert.Equal(9, new Ficha(4, 5).Puntos);
        }

        [Theory]
        [InlineData(3, 3, true)]
        [InlineData(3, 4, false)]
        public void EsDoble_CuandoAmbosValoresSonIguales(int izquierda, int derecha, bool esperado)
        {
            Assert.Equal(esperado, new Ficha(izquierda, derecha).EsDoble);
        }

        [Fact]
        public void ContieneValor_ReconoceCualquieraDeLosDosLados()
        {
            var ficha = new Ficha(2, 6);

            Assert.True(ficha.ContieneValor(2));
            Assert.True(ficha.ContieneValor(6));
            Assert.False(ficha.ContieneValor(4));
        }

        [Fact]
        public void Invertida_IntercambiaLosValores()
        {
            var invertida = new Ficha(1, 5).Invertida();

            Assert.Equal(5, invertida.ValorIzquierda);
            Assert.Equal(1, invertida.ValorDerecha);
        }

        [Fact]
        public void Equals_NoDependeDeLaOrientacion()
        {
            Assert.Equal(new Ficha(1, 5), new Ficha(5, 1));
            Assert.Equal(new Ficha(1, 5).GetHashCode(), new Ficha(5, 1).GetHashCode());
            Assert.NotEqual(new Ficha(1, 5), new Ficha(1, 4));
        }

        [Fact]
        public void Constructor_RechazaValoresNegativos()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ficha(-1, 3));
        }

        [Fact]
        public void ToString_MuestraLaFicha()
        {
            Assert.Equal("[6|3]", new Ficha(6, 3).ToString());
        }
    }
}
