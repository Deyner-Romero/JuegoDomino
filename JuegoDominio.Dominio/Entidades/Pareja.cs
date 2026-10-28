namespace JuegoDominio.Dominio.Entidades
{
    public class Pareja
    {
        private IReadOnlyList<Jugador> jugadores = new List<Jugador>();

        public string Nombre { get; set; } = string.Empty;
        public int PuntosEnMano { get; set; }

        public void Agregar(Jugador jugador)
        {
            throw new NotImplementedException();
        }

        public bool Incluye(Jugador jugador)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Jugador> Jugadores()
        {
            throw new NotImplementedException();
        }
    }
}
