namespace JuegoDominio.Dominio.Entidades
{
    public abstract class Jugador
    {
        private List<Ficha> fichas = new();

        public string Nombre { get; set; } = string.Empty;
        public int PuntosEnMano { get; set; }

        public int FichasRestantes()
        {
            throw new NotImplementedException();
        }

        public void RecibirFichas(List<Ficha> fichas)
        {
            throw new NotImplementedException();
        }

        public void Jugar(Ficha ficha)
        {
            throw new NotImplementedException();
        }

        public bool Tiene(Ficha ficha)
        {
            throw new NotImplementedException();
        }

        public abstract Jugada ElegirJugada(Tablero tablero, IEnumerable<Jugada> posibles);
    }
}
