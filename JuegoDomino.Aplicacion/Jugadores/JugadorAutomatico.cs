using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Jugadores
{
    public class JugadorAutomatico : Jugador
    {
        private IEstrategiaJugador _estrategia;
        public JugadorAutomatico(string nombre, IEstrategiaJugador estrategia) : base(nombre) => _estrategia = estrategia;

        public void CambiarEstrategia(IEstrategiaJugador nuevaEstrategia) => _estrategia = nuevaEstrategia;
        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles) =>
            _estrategia.Elegir(tablero, posibles, Fichas);

    }
}
