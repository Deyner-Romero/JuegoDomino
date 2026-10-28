using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;

namespace JuegoDominio.Dominio.Entidades
{
    public class Partida
    {
        private Dictionary<Jugador, int> marcador = new();
        private Dictionary<Pareja, int> marcadorPorPareja = new();
        private List<Resultado> resultados = new();

        public EstadoPartida Estado { get; set; }
        public List<Jugador> Jugadores { get; set; } = new();
        public List<Pareja> Parejas { get; set; } = new();
        public Jugador? Ganador { get; set; }
        public Pareja? ParejaGanadora { get; set; }
        public IReglas? Reglas { get; set; }

        public int PuntosDe(Pareja pareja)
        {
            throw new NotImplementedException();
        }

        public int PuntosDe(Jugador jugador)
        {
            throw new NotImplementedException();
        }

        public Ronda CrearRonda()
        {
            throw new NotImplementedException();
        }

        public Resultado JugarRonda(Ronda ronda)
        {
            throw new NotImplementedException();
        }
    }
}
