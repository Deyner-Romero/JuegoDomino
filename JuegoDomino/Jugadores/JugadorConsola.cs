using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Consola.Jugadores
{
    public class JugadorConsola : Jugador
    {
        public JugadorConsola(string nombre) : base(nombre)
        {
        }

        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles)
        {
            throw new NotImplementedException();
        }
    }
}
