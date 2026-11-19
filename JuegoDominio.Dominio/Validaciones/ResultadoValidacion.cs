namespace JuegoDominio.Dominio.Validaciones
{
    public class ResultadoValidacion
    {
        private ResultadoValidacion(bool esValida, string mensaje)
        {
            EsValida = esValida;
            Mensaje = mensaje;
        }

        public bool EsValida { get; }
        public string Mensaje { get; }

        public static ResultadoValidacion Ok() => new(true, string.Empty);

        public static ResultadoValidacion Error(string mensaje) => new(false, mensaje);
    }
}
