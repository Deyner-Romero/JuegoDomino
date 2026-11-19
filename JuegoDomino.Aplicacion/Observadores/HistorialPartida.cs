using System.Text;
using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;

namespace JuegoDomino.Aplicacion.Observadores
{
    public class HistorialPartida : IObservadorRonda
    {
        private readonly List<(Jugador Jugador, Jugada Jugada)> jugadas = new();
        private readonly List<string> eventos = new();
        private readonly List<Resultado> resultados = new();
        private string? cierre;

        public IReadOnlyList<(Jugador Jugador, Jugada Jugada)> Jugadas => jugadas;
        public IReadOnlyList<string> Eventos => eventos;
        public IReadOnlyList<Resultado> Resultados => resultados;
        public int Pases { get; private set; }

        public void FichaJugada(Jugador j, Jugada jugada)
        {
            jugadas.Add((j, jugada));
            eventos.Add($"{j.Nombre} juega {jugada}");
        }

        public void TurnoPasado(Jugador j)
        {
            Pases++;
            eventos.Add($"{j.Nombre} pasa");
        }

        public void RondaFinalizada(Resultado r)
        {
            resultados.Add(r);
            eventos.Add($"Ronda {resultados.Count}: {Describir(r)}");
        }

        public void PartidaFinalizada(Partida partida)
        {
            cierre = partida.ParejaGanadora is not null
                ? $"Ganó la pareja {partida.ParejaGanadora.Nombre} con {partida.PuntosDe(partida.ParejaGanadora)} puntos."
                : $"Ganó {partida.Ganador!.Nombre} con {partida.PuntosDe(partida.Ganador)} puntos.";

            eventos.Add(cierre);
        }

        public string Resumen()
        {
            var texto = new StringBuilder();

            for (int i = 0; i < resultados.Count; i++)
                texto.AppendLine($"Ronda {i + 1}: {Describir(resultados[i])}");

            texto.AppendLine($"Jugadas: {jugadas.Count}, pases: {Pases}.");

            if (cierre is not null)
                texto.AppendLine(cierre);

            return texto.ToString().TrimEnd();
        }

        private static string Describir(Resultado r)
        {
            string fin = r.Fin == TipoFin.Domino ? "dominó" : "tranque";

            if (r.HuboEmpate)
                return $"{fin} empatado, nadie anota";

            string ganador = r.ParejaGanadora is not null ? $"la pareja {r.ParejaGanadora.Nombre}" : r.Ganador!.Nombre;
            return $"{fin}, gana {ganador} (+{r.Puntos})";
        }
    }
}
