namespace JuegoDominio.Dominio.Validaciones
{
    public class ValidarExtremo : ValidadorJugada
    {
        protected override ResultadoValidacion Revisar(ContextoJugada contexto)
        {
            if (!contexto.Tablero.JugadaValida(contexto.Jugada))
                return ResultadoValidacion.Error($"La ficha {contexto.Jugada.Ficha} no coincide con el extremo {contexto.Jugada.Extremo}.");

            return ResultadoValidacion.Ok();
        }
    }
}
