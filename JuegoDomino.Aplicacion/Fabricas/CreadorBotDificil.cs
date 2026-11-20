using JuegoDomino.Aplicacion.Estrategia;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Fabricas
{
    public class CreadorBotDificil : CreadorBot
    {
        protected override IEstrategiaJugador CrearEstrategia() => new EstrategiaDefensiva();
    }
}
