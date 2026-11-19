namespace JuegoDominio.Dominio.Validaciones
{
    public abstract class ValidadorJugada
    {
        private ValidadorJugada? siguiente;

        public ValidadorJugada Enlazar(ValidadorJugada validador)
        {
            siguiente = validador;
            return validador;
        }

        public ResultadoValidacion Validar(ContextoJugada contexto)
        {
            var resultado = Revisar(contexto);

            if (!resultado.EsValida || siguiente is null)
                return resultado;

            return siguiente.Validar(contexto);
        }

        protected abstract ResultadoValidacion Revisar(ContextoJugada contexto);
    }
}
