namespace JuegoDominio.Dominio.Entidades
{
    public class Tablero
    {
        private List<Ficha> linea = new();

        public bool EstaVacia { get; private set; }
        public int ExtremoIzquierdo { get; private set; }
        public int ExtremoDerecho { get; private set; }

        public bool JugadaValida(Ficha ficha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Jugada> PosiblesJugadas(Jugada jugada)
        {
            throw new NotImplementedException();
        }

        public void Registrar(Jugada jugada)
        {
            throw new NotImplementedException();
        }
    }
}
