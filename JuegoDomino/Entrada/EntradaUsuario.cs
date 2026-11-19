namespace JuegoDomino.Consola.Entrada
{
    public class EntradaUsuario
    {
        private readonly TextReader entrada;

        public EntradaUsuario(TextReader entrada, TextWriter salida)
        {
            this.entrada = entrada;
            Salida = salida;
        }

        public TextWriter Salida { get; }

        public int LeerOpcion(string pregunta, int minimo, int maximo)
        {
            while (true)
            {
                Salida.Write($"{pregunta} ({minimo}-{maximo}): ");

                if (int.TryParse(Leer(), out int opcion) && opcion >= minimo && opcion <= maximo)
                    return opcion;

                Salida.WriteLine($"Escribe un número entre {minimo} y {maximo}.");
            }
        }

        public string LeerTexto(string pregunta)
        {
            while (true)
            {
                Salida.Write($"{pregunta}: ");
                string texto = Leer().Trim();

                if (texto.Length > 0)
                    return texto;

                Salida.WriteLine("No puede quedar vacío.");
            }
        }

        public void Pausa(string mensaje)
        {
            Salida.Write(mensaje);
            Leer();
        }

        private string Leer() => entrada.ReadLine() ?? throw new EndOfStreamException("Se cerró la entrada.");
    }
}
