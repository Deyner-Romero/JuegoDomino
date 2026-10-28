namespace JuegoDominio.Dominio.Entidades
{
    public class Ficha
    {
        public int ValorIzquierda { get; set; }
        public int ValorDerecha { get; set; }
        public bool EsDoble { get; set; }
        public int Puntos { get; set; }

        public bool ContieneValor(int valor)
        {
            throw new NotImplementedException();
        }
    }
}
