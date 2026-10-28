using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Estrategia
{
    public class EstrategiaDescartarPuntos : IEstrategiaJugador
    {
        public Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano) =>
        posibles.OrderByDescending(x => x.Ficha?.Puntos).First();
    }
}