namespace JuegoDominio.Dominio.Entidades
{
    public abstract class Jugador
    {
        private readonly List<Ficha> fichas = new();

        protected Jugador(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }
        public IReadOnlyList<Ficha> Fichas => fichas;
        public int FichasRestantes => fichas.Count;
        public bool SinFichas => fichas.Count == 0;
        public int PuntosEnMano => fichas.Sum(ficha => ficha.Puntos);

        public void RecibirFichas(IEnumerable<Ficha> nuevasFichas)
        {
            fichas.Clear();
            fichas.AddRange(nuevasFichas);
        }

        public void Jugar(Ficha ficha)
        {
            if (!fichas.Remove(ficha))
                throw new InvalidOperationException($"{Nombre} no tiene la ficha {ficha}.");
        }

        public bool Tiene(Ficha ficha) => fichas.Contains(ficha);

        public abstract Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles);

        public override string ToString() => Nombre;
    }
}
