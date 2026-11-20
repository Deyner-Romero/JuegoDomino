namespace JuegoDomino.Consola.Entrada
{
    public class AdaptadorConsola : IEntradaUsuario
    {
        private readonly TextReader entrada;
        private readonly TextWriter salida;

        public AdaptadorConsola(TextReader entrada, TextWriter salida)
        {
            this.entrada = entrada;
            this.salida = salida;
        }

        public int LeerOpcion(string pregunta, int minimo, int maximo)
        {
            while (true)
            {
                salida.Write($"{pregunta} ({minimo}-{maximo}): ");

                if (int.TryParse(Leer(), out int opcion) && opcion >= minimo && opcion <= maximo)
                    return opcion;

                salida.WriteLine($"Escribe un número entre {minimo} y {maximo}.");
            }
        }

        public string LeerTexto(string pregunta)
        {
            while (true)
            {
                salida.Write($"{pregunta}: ");
                string texto = Leer().Trim();

                if (texto.Length > 0)
                    return texto;

                salida.WriteLine("No puede quedar vacío.");
            }
        }

        public void Pausa(string mensaje)
        {
            salida.Write(mensaje);
            Leer();
        }

        public void Mostrar(string linea = "") => salida.WriteLine(linea);

        private string Leer() => entrada.ReadLine() ?? throw new EndOfStreamException("Se cerró la entrada.");
    }
}
