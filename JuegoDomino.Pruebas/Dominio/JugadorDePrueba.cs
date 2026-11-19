using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class JugadorDePrueba : Jugador
    {
        public JugadorDePrueba(string nombre = "Prueba") : base(nombre)
        {
        }

        public override Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles) => posibles.First();
    }
}
