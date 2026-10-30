namespace JuegoDominio.Dominio.Validaciones
{
    public abstract class ValidadorJugada
    {
        private ValidadorJugada? siguiente;

        public virtual ValidadorJugada Enlazar(ValidadorJugada validador)
        {
            siguiente = validador;
            return validador;
        }

        public virtual ResultadoValidacion Validar(ContextoJugada contexto)
        {
            throw new NotImplementedException();
        }
    }
}
