using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;

namespace JuegoDominio.Dominio.Reglas
{
    public class ReglasTradicionales : IReglas
    {
        public ReglasTradicionales(TipoJuego tipoJuego, int numeroDeJugadores, int puntosParaGanar = 80)
        {
            if (tipoJuego == TipoJuego.Parejas && numeroDeJugadores != 4)
                throw new ArgumentException("El juego en parejas requiere exactamente 4 jugadores.", nameof(numeroDeJugadores));

            if (numeroDeJugadores < 2 || numeroDeJugadores > 4)
                throw new ArgumentException("El juego individual admite de 2 a 4 jugadores.", nameof(numeroDeJugadores));

            if (puntosParaGanar <= 0)
                throw new ArgumentException("Los puntos para ganar deben ser mayores que cero.", nameof(puntosParaGanar));

            TipoJuego = tipoJuego;
            NumeroDeJugadores = numeroDeJugadores;
            PuntosParaGanar = puntosParaGanar;
        }

        public int FichasPorJugador => 7;
        public int PuntosParaGanar { get; }
        public int NumeroDeJugadores { get; }
        public TipoJuego TipoJuego { get; }

        public Ficha FichaDeSalida(IReadOnlyList<Jugador> jugadores)
        {
            var enJuego = jugadores.SelectMany(jugador => jugador.Fichas).ToList();

            var dobleMasAlto = enJuego
                .Where(ficha => ficha.EsDoble)
                .OrderByDescending(ficha => ficha.Puntos)
                .FirstOrDefault();

            return dobleMasAlto ?? enJuego
                .OrderByDescending(ficha => ficha.Puntos)
                .ThenByDescending(ficha => Math.Max(ficha.ValorIzquierda, ficha.ValorDerecha))
                .First();
        }

        public Jugador JugadorQueSale(IReadOnlyList<Jugador> jugadores, Resultado? rondaAnterior)
        {
            if (rondaAnterior is null)
            {
                var fichaDeSalida = FichaDeSalida(jugadores);
                return jugadores.First(jugador => jugador.Tiene(fichaDeSalida));
            }

            if (rondaAnterior.Ganador is not null)
                return rondaAnterior.Ganador;

            int posicionDelQueAbrio = jugadores.ToList().IndexOf(rondaAnterior.AbrioLaRonda!);
            return jugadores[(posicionDelQueAbrio + 1) % jugadores.Count];
        }

        public bool EsTranque(int pasesSeguidos) => pasesSeguidos >= NumeroDeJugadores;

        public Resultado ResolverDomino(Jugador ganador, IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas) =>
            new()
            {
                Fin = TipoFin.Domino,
                Ganador = ganador,
                ParejaGanadora = parejas.FirstOrDefault(pareja => pareja.Incluye(ganador)),
                Puntos = PuntosEnMesa(jugadores)
            };

        public Resultado ResolverTranque(IReadOnlyList<Jugador> jugadores, IReadOnlyList<Pareja> parejas)
        {
            int puntos = PuntosEnMesa(jugadores);

            if (TipoJuego == TipoJuego.Parejas)
            {
                var parejaGanadora = UnicoConMenosPuntos(parejas, pareja => pareja.PuntosEnMano);

                if (parejaGanadora is null)
                    return new Resultado { Fin = TipoFin.Tranque };

                return new Resultado
                {
                    Fin = TipoFin.Tranque,
                    ParejaGanadora = parejaGanadora,
                    Ganador = parejaGanadora.Jugadores.MinBy(jugador => jugador.PuntosEnMano),
                    Puntos = puntos
                };
            }

            var ganador = UnicoConMenosPuntos(jugadores, jugador => jugador.PuntosEnMano);

            if (ganador is null)
                return new Resultado { Fin = TipoFin.Tranque };

            return new Resultado { Fin = TipoFin.Tranque, Ganador = ganador, Puntos = puntos };
        }

        public bool AlcanzoLaMeta(int puntos) => puntos >= PuntosParaGanar;

        private static int PuntosEnMesa(IReadOnlyList<Jugador> jugadores) =>
            jugadores.Sum(jugador => jugador.PuntosEnMano);

        private static T? UnicoConMenosPuntos<T>(IEnumerable<T> participantes, Func<T, int> puntosDe) where T : class
        {
            var conMenosPuntos = participantes
                .GroupBy(puntosDe)
                .OrderBy(grupo => grupo.Key)
                .First();

            return conMenosPuntos.Count() == 1 ? conMenosPuntos.Single() : null;
        }
    }
}
