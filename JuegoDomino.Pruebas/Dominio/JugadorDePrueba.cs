using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class JugadorDePrueba : Jugador
    {
        public JugadorDePrueba(string nombre = "Prueba") : base(nombre)
        {
        }

        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles) => posibles.First();
    }
}
