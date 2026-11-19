using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;
using JuegoDomino.Aplicacion.Juego;

namespace JuegoDomino.Consola.Vistas
{
    public class VistaConsola : IObservadorRonda
    {
        private readonly Juego juego;
        private readonly TextWriter salida;
        private readonly TimeSpan pausaEntreTurnos;

        public VistaConsola(Juego juego, TextWriter salida, TimeSpan pausaEntreTurnos = default)
        {
            this.juego = juego;
            this.salida = salida;
            this.pausaEntreTurnos = pausaEntreTurnos;
        }

        public void FichaJugada(Jugador j, Jugada jugada)
        {
            string lado = jugada.Extremo == Extremo.Izquierdo ? "izquierda" : "derecha";
            string detalle = juego.Mesa!.Linea.Count == 1 ? "abre" : $"juega por la {lado}";

            salida.WriteLine($"{j.Nombre} {detalle} con {jugada.Ficha}");
            salida.WriteLine($"   Mesa: {juego.Mesa}");
            Esperar();
        }

        public void TurnoPasado(Jugador j)
        {
            salida.WriteLine($"{j.Nombre} pasa.");
            Esperar();
        }

        public void RondaFinalizada(Resultado r)
        {
            salida.WriteLine();
            salida.WriteLine(DescribirFin(r));

            salida.WriteLine("Fichas que quedaron:");
            foreach (var jugador in juego.Partida.Jugadores)
            {
                string fichas = jugador.SinFichas ? "ninguna" : string.Concat(jugador.Fichas);
                salida.WriteLine($"  {jugador.Nombre}: {fichas} ({jugador.PuntosEnMano} puntos)");
            }

            MostrarMarcador();
        }

        public void PartidaFinalizada(Partida partida)
        {
            salida.WriteLine();
            salida.WriteLine("==============================");
            string ganador = partida.ParejaGanadora is not null ? $"La pareja {partida.ParejaGanadora.Nombre}" : partida.Ganador!.Nombre;
            salida.WriteLine($"  ¡{ganador} gana la partida!");
            salida.WriteLine("==============================");
        }

        private static string DescribirFin(Resultado r)
        {
            string fin = r.Fin == TipoFin.Domino ? $"¡Dominó de {r.Ganador!.Nombre}!" : "¡Tranque!";

            if (r.HuboEmpate)
                return $"{fin} Hay empate en puntos y nadie anota.";

            string ganador = r.ParejaGanadora is not null ? $"la pareja {r.ParejaGanadora.Nombre}" : r.Ganador!.Nombre;
            return $"{fin} Gana {ganador} y suma {r.Puntos} puntos.";
        }

        private void MostrarMarcador()
        {
            salida.WriteLine($"Marcador (se juega a {juego.Partida.Reglas.PuntosParaGanar}):");

            foreach (var (nombre, puntos) in juego.Marcador())
                salida.WriteLine($"  {nombre}: {puntos}");
        }

        private void Esperar()
        {
            if (pausaEntreTurnos > TimeSpan.Zero)
                Thread.Sleep(pausaEntreTurnos);
        }
    }
}
