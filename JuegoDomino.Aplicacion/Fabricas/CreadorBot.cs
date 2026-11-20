using JuegoDomino.Aplicacion.Interface;
using JuegoDomino.Aplicacion.Jugadores;

namespace JuegoDomino.Aplicacion.Fabricas
{
    public abstract class CreadorBot
    {
        public JugadorAutomatico Crear(string nombre) => new(nombre, CrearEstrategia());

        protected abstract IEstrategiaJugador CrearEstrategia();
    }
}
