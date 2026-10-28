using JuegoDominio.Dominio.Enumeraciones;

namespace JuegoDominio.Dominio.Entidades
{
    public class Resultado
    {
        public Jugador? Ganador { get; set; }
        public Pareja? ParejaGanadora { get; set; }
        public int Puntos { get; set; }
        public Jugador? UltimoEnJugar { get; set; }
        public TipoFin Fin { get; set; }

        public Resultado Registrar()
        {
            throw new NotImplementedException();
        }
    }
}
