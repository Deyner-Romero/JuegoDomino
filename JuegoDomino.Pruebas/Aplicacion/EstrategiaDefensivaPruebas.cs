using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDomino.Aplicacion.Estrategia;

namespace JuegoDomino.Pruebas.Aplicacion
{
    public class EstrategiaDefensivaPruebas
    {
        [Fact]
        public void Elige_LaJugadaQueDejaAbiertoElNumeroDelQueMasFichasLeQuedan()
        {
            // En la mesa está [3|5]. Jugar [3|1] a la izquierda deja abiertos el 1 y el 5,
            // que sirven para cuatro fichas de la mano; jugar [2|5] a la derecha solo para dos.
            var tablero = new Tablero();
            tablero.Registrar(new Jugada(new Ficha(3, 5), Extremo.Derecho));
            Ficha[] mano = [new(3, 1), new(2, 5), new(1, 4), new(1, 6), new(1, 0), new(2, 2)];

            var jugada = new EstrategiaDefensiva().Elegir(tablero, tablero.PosiblesJugadas(mano), mano);

            Assert.Equal(new Ficha(3, 1), jugada.Ficha);
            Assert.Equal(Extremo.Izquierdo, jugada.Extremo);
        }
    }
}
