using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Aplicacion.Jugadores
{
    public class JugadorAutomatico : Jugador
    {
        public override Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles)
        {
            throw new NotImplementedException();
        }
    }
}
