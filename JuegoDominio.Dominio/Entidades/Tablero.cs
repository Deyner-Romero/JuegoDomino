using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Tablero
    {
        private readonly List<Ficha> linea = new();

        public IReadOnlyList<Ficha> Linea => linea;
        public bool EstaVacia => linea.Count == 0;
        public int? ExtremoIzquierdo => EstaVacia ? null : linea[0].ValorIzquierda;
        public int? ExtremoDerecho => EstaVacia ? null : linea[^1].ValorDerecha;

        public bool JugadaValida(Jugada jugada)
        {
            if (EstaVacia)
                return true;

            return jugada.Ficha.ContieneValor(ValorDelExtremo(jugada.Extremo));
        }

        public List<Jugada> PosiblesJugadas(IEnumerable<Ficha> mano)
        {
            if (EstaVacia)
                return mano.Select(ficha => new Jugada(ficha, Extremo.Derecho)).ToList();

            return mano
                .SelectMany(ficha => new[] { new Jugada(ficha, Extremo.Izquierdo), new Jugada(ficha, Extremo.Derecho) })
                .Where(JugadaValida)
                .ToList();
        }

        public void Registrar(Jugada jugada)
        {
            if (!JugadaValida(jugada))
                throw new InvalidOperationException($"La ficha {jugada.Ficha} no se puede colocar en el extremo {jugada.Extremo}.");

            if (EstaVacia)
            {
                linea.Add(jugada.Ficha);
                return;
            }

            if (jugada.Extremo == Extremo.Izquierdo)
                linea.Insert(0, Orientar(jugada.Ficha, valorHaciaAdentro: linea[0].ValorIzquierda, haciaLaIzquierda: true));
            else
                linea.Add(Orientar(jugada.Ficha, valorHaciaAdentro: linea[^1].ValorDerecha, haciaLaIzquierda: false));
        }

        private int ValorDelExtremo(Extremo extremo) =>
            extremo == Extremo.Izquierdo ? linea[0].ValorIzquierda : linea[^1].ValorDerecha;

        private static Ficha Orientar(Ficha ficha, int valorHaciaAdentro, bool haciaLaIzquierda)
        {
            int valorQueToca = haciaLaIzquierda ? ficha.ValorDerecha : ficha.ValorIzquierda;
            return valorQueToca == valorHaciaAdentro ? ficha : ficha.Invertida();
        }

        public override string ToString() => string.Concat(linea);
    }
}
