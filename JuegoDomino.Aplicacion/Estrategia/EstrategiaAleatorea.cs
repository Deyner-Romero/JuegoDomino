using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Estrategia
{
    public class EstrategiaAleatorea : IEstrategiaJugador
    {
        private readonly Random _random;

        public EstrategiaAleatorea(Random? random = null) => _random = random ?? new Random();

        public Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano) =>
           posibles[_random.Next(posibles.Count)];
    }
}
