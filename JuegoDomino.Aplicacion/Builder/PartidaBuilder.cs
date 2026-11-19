using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Interfaces;
using JuegoDominio.Dominio.Validaciones;

namespace JuegoDomino.Aplicacion.Builder
{
    public class PartidaBuilder
    {
        private readonly List<Jugador> jugadores = new();
        private readonly List<Pareja> parejas = new();
        private readonly List<IObservadorRonda> observadores = new();
        private IReglas? reglas;
        private Random? azar;

        public PartidaBuilder ConReglas(IReglas reglas)
        {
            this.reglas = reglas;
            return this;
        }

        public PartidaBuilder AgregarJugador(Jugador jugador)
        {
            jugadores.Add(jugador);
            return this;
        }

        public PartidaBuilder AgregarPareja(string nombre, Jugador primero, Jugador segundo)
        {
            var pareja = new Pareja(nombre);
            pareja.Agregar(primero);
            pareja.Agregar(segundo);
            parejas.Add(pareja);
            return this;
        }

        public PartidaBuilder ConObservador(IObservadorRonda observador)
        {
            observadores.Add(observador);
            return this;
        }

        public PartidaBuilder ConAzar(Random azar)
        {
            this.azar = azar;
            return this;
        }

        public Partida Build()
        {
            if (reglas is null)
                throw new InvalidOperationException("Faltan las reglas de la partida.");

            var enMesa = reglas.TipoJuego == TipoJuego.Parejas ? OrdenDeMesaPorParejas() : OrdenDeMesaIndividual(reglas);

            var repetido = enMesa
                .GroupBy(jugador => jugador.Nombre.Trim(), StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(grupo => grupo.Count() > 1);

            if (repetido is not null)
                throw new InvalidOperationException($"El nombre {repetido.Key} está repetido.");

            var partida = new Partida(enMesa, parejas.ToList(), reglas, ArmarCadenaValidacion(), azar);

            foreach (var observador in observadores)
                partida.Suscribir(observador);

            return partida;
        }

        private List<Jugador> OrdenDeMesaIndividual(IReglas reglas)
        {
            if (parejas.Count > 0)
                throw new InvalidOperationException("Una partida individual no lleva parejas.");

            if (jugadores.Count < 2 || jugadores.Count > 4)
                throw new InvalidOperationException("La partida individual admite de 2 a 4 jugadores.");

            if (jugadores.Count != reglas.NumeroDeJugadores)
                throw new InvalidOperationException($"Las reglas son para {reglas.NumeroDeJugadores} jugadores y se agregaron {jugadores.Count}.");

            return jugadores.ToList();
        }

        private List<Jugador> OrdenDeMesaPorParejas()
        {
            if (jugadores.Count > 0)
                throw new InvalidOperationException("En una partida por parejas los jugadores se agregan con AgregarPareja.");

            if (parejas.Count != 2)
                throw new InvalidOperationException("La partida por parejas requiere exactamente 2 parejas.");

            var (a, b) = (parejas[0].Jugadores, parejas[1].Jugadores);

            // A1 → B1 → A2 → B2: los compañeros quedan enfrentados.
            return [a[0], b[0], a[1], b[1]];
        }

        private static ValidadorJugada ArmarCadenaValidacion()
        {
            var validarTurno = new ValidarTurno();

            validarTurno
                .Enlazar(new ValidarFichaEnMano())
                .Enlazar(new ValidarSalida())
                .Enlazar(new ValidarExtremo());

            return validarTurno;
        }
    }
}
