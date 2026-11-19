namespace JuegoDominio.Dominio.Entidades
{
    public class JuegoDeFichas
    {
        private readonly Random azar;
        private List<Ficha> fichas;

        public JuegoDeFichas(int valorMaximo = 6, Random? azar = null)
        {
            this.azar = azar ?? new Random();
            fichas = Generar(valorMaximo);
        }

        public int Cantidad => fichas.Count;
        public IReadOnlyList<Ficha> Fichas => fichas;
        public IReadOnlyList<Ficha> Pozo { get; private set; } = new List<Ficha>();

        public void Repartir(IReadOnlyList<Jugador> jugadores, int fichasPorJugador)
        {
            if (jugadores.Count * fichasPorJugador > Cantidad)
                throw new InvalidOperationException("No hay suficientes fichas para repartir.");

            Barajar();

            for (int i = 0; i < jugadores.Count; i++)
                jugadores[i].RecibirFichas(fichas.Skip(i * fichasPorJugador).Take(fichasPorJugador));

            Pozo = fichas.Skip(jugadores.Count * fichasPorJugador).ToList();
        }

        private void Barajar()
        {
            var mezcladas = fichas.ToArray();
            azar.Shuffle(mezcladas);
            fichas = mezcladas.ToList();
        }

        private static List<Ficha> Generar(int valorMaximo)
        {
            var nuevas = new List<Ficha>();

            for (int izquierda = 0; izquierda <= valorMaximo; izquierda++)
                for (int derecha = izquierda; derecha <= valorMaximo; derecha++)
                    nuevas.Add(new Ficha(izquierda, derecha));

            return nuevas;
        }
    }
}
