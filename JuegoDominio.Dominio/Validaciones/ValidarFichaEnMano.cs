namespace JuegoDominio.Dominio.Validaciones
{
    public class ValidarFichaEnMano : ValidadorJugada
    {
        protected override ResultadoValidacion Revisar(ContextoJugada contexto)
        {
            if (!contexto.Jugador.Tiene(contexto.Jugada.Ficha))
                return ResultadoValidacion.Error($"{contexto.Jugador.Nombre} no tiene la ficha {contexto.Jugada.Ficha}.");

            return ResultadoValidacion.Ok();
        }
    }
}
