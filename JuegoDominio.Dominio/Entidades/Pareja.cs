namespace JuegoDominio.Dominio.Entidades
{
    public class Pareja
    {
        private readonly List<Jugador> jugadores = new();

        public Pareja(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }
        public IReadOnlyList<Jugador> Jugadores => jugadores;
        public int PuntosEnMano => jugadores.Sum(jugador => jugador.PuntosEnMano);

        public void Agregar(Jugador jugador)
        {
            if (jugadores.Count == 2)
                throw new InvalidOperationException($"La pareja {Nombre} ya tiene dos jugadores.");

            if (Incluye(jugador))
                throw new InvalidOperationException($"{jugador.Nombre} ya pertenece a la pareja {Nombre}.");

            jugadores.Add(jugador);
        }

        public bool Incluye(Jugador jugador) => jugadores.Contains(jugador);

        public override string ToString() => Nombre;
    }
}
