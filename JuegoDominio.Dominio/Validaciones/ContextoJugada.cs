using JuegoDominio.Dominio.Entidades;

namespace JuegoDominio.Dominio.Validaciones
{
    public class ContextoJugada
    {
        public ContextoJugada(Jugador jugador, Jugada jugada, Jugador jugadorEnTurno, Tablero tablero, Ficha? fichaDeSalida = null)
        {
            Jugador = jugador;
            Jugada = jugada;
            JugadorEnTurno = jugadorEnTurno;
            Tablero = tablero;
            FichaDeSalida = fichaDeSalida;
        }

        public Jugador Jugador { get; }
        public Jugada Jugada { get; }
        public Jugador JugadorEnTurno { get; }
        public Tablero Tablero { get; }
        public Ficha? FichaDeSalida { get; }
    }
}
