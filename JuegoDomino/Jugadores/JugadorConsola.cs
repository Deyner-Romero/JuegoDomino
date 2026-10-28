using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Consola.Jugadores
{
    public class JugadorConsola : Jugador
    {
        public override Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles)
        {
            throw new NotImplementedException();
        }
    }
}
