using JuegoDominio.Dominio.Validaciones;

namespace JuegoDomino.Aplicacion.Builder
{
    public class PartidaBuilder
    {
        private ValidadorJugada? cadenaValidacion;

        private void ArmarCadenaValidacion()
        {
            var validarTurno = new ValidarTurno();

            validarTurno
                .Enlazar(new ValidarFichaEnMano())
                .Enlazar(new ValidarSalida())
                .Enlazar(new ValidarExtremo());

            cadenaValidacion = validarTurno;
        }
    }
}
