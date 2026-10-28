namespace JuegoDominio.Dominio.Entidades
{
    public class JuegoDeFichas
    {
        private List<Ficha> fichas = new();

        public int Cantidad { get; set; }

        private List<Ficha> Barajar(Random azar)
        {
            throw new NotImplementedException();
        }

        public void Repartir(List<Jugador> jugadores, int fichasPorJugador)
        {
            throw new NotImplementedException();
        }
    }
}
