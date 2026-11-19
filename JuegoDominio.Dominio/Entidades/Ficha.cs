namespace JuegoDominio.Dominio.Entidades
{
    public class Ficha
    {
        public Ficha(int valorIzquierda, int valorDerecha)
        {
            if (valorIzquierda < 0 || valorDerecha < 0)
                throw new ArgumentOutOfRangeException(nameof(valorIzquierda), "Los valores de una ficha no pueden ser negativos.");

            ValorIzquierda = valorIzquierda;
            ValorDerecha = valorDerecha;
        }

        public int ValorIzquierda { get; }
        public int ValorDerecha { get; }
        public bool EsDoble => ValorIzquierda == ValorDerecha;
        public int Puntos => ValorIzquierda + ValorDerecha;

        private int Menor => Math.Min(ValorIzquierda, ValorDerecha);
        private int Mayor => Math.Max(ValorIzquierda, ValorDerecha);

        public bool ContieneValor(int valor) => ValorIzquierda == valor || ValorDerecha == valor;

        public Ficha Invertida() => new(ValorDerecha, ValorIzquierda);

        public override bool Equals(object? obj) =>
            obj is Ficha otra && Menor == otra.Menor && Mayor == otra.Mayor;

        public override int GetHashCode() => HashCode.Combine(Menor, Mayor);

        public override string ToString() => $"[{ValorIzquierda}|{ValorDerecha}]";
    }
}
