namespace JuegoDominio.Dominio.Validaciones
{
    public class ValidarFichaEnMano : ValidadorJugada
    {
        public override ValidadorJugada Enlazar(ValidadorJugada validador)
        {
            return base.Enlazar(validador);
        }

        public override ResultadoValidacion Validar(ContextoJugada contexto)
        {
            throw new NotImplementedException();
        }
    }
}
