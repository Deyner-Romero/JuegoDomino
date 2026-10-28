using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Jugadores
{
    public class JugadorAutomatico : Jugador
    {
        private IEstrategiaJugador _estrategia;
        public JugadorAutomatico(string nombre, IEstrategiaJugador estrategia) => _estrategia = estrategia;

        public void CambiarEstrategia(IEstrategiaJugador nuevaEstrategia) => _estrategia = nuevaEstrategia;
        public override Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles) =>
            _estrategia.Elegir(tablero, posibles.ToList(), Fichas);

    }
}
