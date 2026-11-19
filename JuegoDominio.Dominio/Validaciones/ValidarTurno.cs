namespace JuegoDominio.Dominio.Validaciones
{
    public class ValidarTurno : ValidadorJugada
    {
        protected override ResultadoValidacion Revisar(ContextoJugada contexto)
        {
            if (contexto.Jugador != contexto.JugadorEnTurno)
                return ResultadoValidacion.Error($"No es el turno de {contexto.Jugador.Nombre}, le toca a {contexto.JugadorEnTurno.Nombre}.");

            return ResultadoValidacion.Ok();
        }
    }
}
