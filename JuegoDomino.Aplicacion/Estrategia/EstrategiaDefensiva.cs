using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Estrategia
{
    public class EstrategiaDefensiva : IEstrategiaJugador
    {
        public Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano) =>
        posibles.OrderByDescending(x => CuantasTieneDelNumero(x, mano)).First();

        private static int CuantasTieneDelNumero(Jugada jugada, IReadOnlyList<Ficha> mano) =>
            mano.Count(ficha => ficha.ValorDerecha == jugada.Ficha?.ValorDerecha || ficha.ValorIzquierda == jugada.Ficha?.ValorIzquierda);
    }
}