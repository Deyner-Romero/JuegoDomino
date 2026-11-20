using JuegoDomino.Aplicacion.Estrategia;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Fabricas
{
    public class CreadorBotNormal : CreadorBot
    {
        protected override IEstrategiaJugador CrearEstrategia() => new EstrategiaDescartarPuntos();
    }
}
