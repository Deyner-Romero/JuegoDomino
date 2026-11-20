using JuegoDomino.Aplicacion.Estrategia;
using JuegoDomino.Aplicacion.Interface;

namespace JuegoDomino.Aplicacion.Fabricas
{
    public class CreadorBotFacil : CreadorBot
    {
        private readonly Random? azar;

        public CreadorBotFacil(Random? azar = null)
        {
            this.azar = azar;
        }

        protected override IEstrategiaJugador CrearEstrategia() => new EstrategiaAleatorea(azar);
    }
}
