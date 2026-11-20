namespace JuegoDomino.Consola.Entrada
{
    public interface IEntradaUsuario
    {
        int LeerOpcion(string pregunta, int minimo, int maximo);
        string LeerTexto(string pregunta);
        void Pausa(string mensaje);
        void Mostrar(string linea = "");
    }
}
