using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Interfaces;
using JuegoDominio.Dominio.Validaciones;

namespace JuegoDomino.Pruebas.Dominio
{
    public static class Cadena
    {
        public static ValidadorJugada DeValidacion()
        {
            var validarTurno = new ValidarTurno();

            validarTurno
                .Enlazar(new ValidarFichaEnMano())
                .Enlazar(new ValidarSalida())
                .Enlazar(new ValidarExtremo());

            return validarTurno;
        }
    }

    public class ObservadorDePrueba : IObservadorRonda
    {
        public List<string> Eventos { get; } = new();
        public List<Resultado> Resultados { get; } = new();
        public int PartidasFinalizadas { get; private set; }

        public void FichaJugada(Jugador j, Jugada jugada) => Eventos.Add($"{j} juega {jugada.Ficha}");

        public void TurnoPasado(Jugador j) => Eventos.Add($"{j} pasa");

        public void RondaFinalizada(Resultado r)
        {
            Eventos.Add($"fin por {r.Fin}");
            Resultados.Add(r);
        }

        public void PartidaFinalizada(Partida partida) => PartidasFinalizadas++;
    }

    public class JugadorTramposo : Jugador
    {
        public JugadorTramposo() : base("Tramposo")
        {
        }

        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles) =>
            new(new Ficha(0, 0), JuegoDominio.Dominio.Enumeraciones.Extremo.Derecho);
    }

    public class JugadorQueSeEquivoca : Jugador
    {
        private bool yaSeEquivoco;

        public JugadorQueSeEquivoca() : base("Humano")
        {
        }

        public List<string> MensajesRecibidos { get; } = new();

        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles)
        {
            if (yaSeEquivoco)
                return posibles.First();

            yaSeEquivoco = true;
            return new Jugada(new Ficha(0, 0), JuegoDominio.Dominio.Enumeraciones.Extremo.Derecho);
        }

        protected override void JugadaRechazada(ResultadoValidacion resultado) => MensajesRecibidos.Add(resultado.Mensaje);
    }
}
