using JuegoDominio.Dominio.Validaciones;

namespace JuegoDominio.Dominio.Entidades
{
    public abstract class Jugador
    {
        private readonly List<Ficha> fichas = new();

        protected Jugador(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }
        public IReadOnlyList<Ficha> Fichas => fichas;
        public int FichasRestantes => fichas.Count;
        public bool SinFichas => fichas.Count == 0;
        public int PuntosEnMano => fichas.Sum(ficha => ficha.Puntos);

        public void RecibirFichas(IEnumerable<Ficha> nuevasFichas)
        {
            fichas.Clear();
            fichas.AddRange(nuevasFichas);
        }

        public void Jugar(Ficha ficha)
        {
            if (!fichas.Remove(ficha))
                throw new InvalidOperationException($"{Nombre} no tiene la ficha {ficha}.");
        }

        public bool Tiene(Ficha ficha) => fichas.Contains(ficha);

        /// <summary>
        /// Template Method: el flujo del turno es el mismo para todos los jugadores.
        /// Las subclases solo deciden la jugada (<see cref="ElegirJugada"/>) y,
        /// si quieren, cómo reaccionar a una jugada rechazada (<see cref="JugadaRechazada"/>).
        /// </summary>
        /// <returns>La jugada realizada, o <c>null</c> si el jugador pasa.</returns>
        public Jugada? RealizarTurno(Tablero tablero, ValidadorJugada validador, Jugador jugadorEnTurno, Ficha? fichaDeSalida = null)
        {
            var posibles = JugadasPosibles(tablero, fichaDeSalida);

            if (posibles.Count == 0)
                return null;

            while (true)
            {
                var jugada = ElegirJugada(tablero, posibles);
                var resultado = validador.Validar(new ContextoJugada(this, jugada, jugadorEnTurno, tablero, fichaDeSalida));

                if (resultado.EsValida)
                {
                    tablero.Registrar(jugada);
                    Jugar(jugada.Ficha);
                    return jugada;
                }

                JugadaRechazada(resultado);
            }
        }

        protected abstract Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles);

        /// <summary>
        /// Gancho del Template Method. Por defecto una jugada inválida es un error del jugador
        /// automático; el jugador humano lo sobrescribe para mostrar el mensaje y volver a pedir.
        /// </summary>
        protected virtual void JugadaRechazada(ResultadoValidacion resultado) =>
            throw new InvalidOperationException($"{Nombre} eligió una jugada inválida: {resultado.Mensaje}");

        private List<Jugada> JugadasPosibles(Tablero tablero, Ficha? fichaDeSalida)
        {
            var posibles = tablero.PosiblesJugadas(fichas);

            if (tablero.EstaVacia && fichaDeSalida is not null)
                return posibles.Where(jugada => jugada.Ficha.Equals(fichaDeSalida)).ToList();

            return posibles;
        }

        public override string ToString() => Nombre;
    }
}
