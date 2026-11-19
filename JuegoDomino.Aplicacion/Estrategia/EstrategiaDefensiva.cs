using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Estrategia
{
    public class EstrategiaDefensiva : IEstrategiaJugador
    {
        public Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano) =>
            posibles.OrderByDescending(jugada => FichasQueSiguenSirviendo(tablero, jugada, mano)).First();

        private static int FichasQueSiguenSirviendo(Tablero tablero, Jugada jugada, IReadOnlyList<Ficha> mano)
        {
            var restantes = mano.Where(ficha => !ficha.Equals(jugada.Ficha)).ToList();
            var abiertos = ValoresQueQuedanAbiertos(tablero, jugada);

            return restantes.Count(ficha => abiertos.Any(ficha.ContieneValor));
        }

        private static int[] ValoresQueQuedanAbiertos(Tablero tablero, Jugada jugada)
        {
            var ficha = jugada.Ficha;

            if (tablero.EstaVacia)
                return [ficha.ValorIzquierda, ficha.ValorDerecha];

            int extremoQueSeCubre = jugada.Extremo == Extremo.Izquierdo ? tablero.ExtremoIzquierdo!.Value : tablero.ExtremoDerecho!.Value;
            int extremoQueNoSeToca = jugada.Extremo == Extremo.Izquierdo ? tablero.ExtremoDerecho!.Value : tablero.ExtremoIzquierdo!.Value;
            int nuevoExtremo = ficha.ValorIzquierda == extremoQueSeCubre ? ficha.ValorDerecha : ficha.ValorIzquierda;

            return [nuevoExtremo, extremoQueNoSeToca];
        }
    }
}
