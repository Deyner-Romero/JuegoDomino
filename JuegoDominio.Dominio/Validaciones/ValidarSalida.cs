namespace JuegoDominio.Dominio.Validaciones
{
    public class ValidarSalida : ValidadorJugada
    {
        protected override ResultadoValidacion Revisar(ContextoJugada contexto)
        {
            bool esLaSalida = contexto.Tablero.EstaVacia && contexto.FichaDeSalida is not null;

            if (esLaSalida && !contexto.Jugada.Ficha.Equals(contexto.FichaDeSalida))
                return ResultadoValidacion.Error($"La partida debe abrirse con la ficha {contexto.FichaDeSalida}.");

            return ResultadoValidacion.Ok();
        }
    }
}
