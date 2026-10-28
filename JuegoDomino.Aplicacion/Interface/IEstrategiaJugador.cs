using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Aplicacion.Interface{
    public interface IEstrategiaJugador {
        Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano);
    } 
}