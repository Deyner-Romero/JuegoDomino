using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Validaciones;
using JuegoDomino.Consola.Entrada;

namespace JuegoDomino.Consola.Jugadores
{
    public class JugadorConsola : Jugador
    {
        private readonly IEntradaUsuario entrada;
        private bool reintentando;

        public JugadorConsola(string nombre, IEntradaUsuario entrada) : base(nombre)
        {
            this.entrada = entrada;
        }

        protected override Jugada ElegirJugada(Tablero tablero, IReadOnlyList<Jugada> posibles)
        {
            MostrarTurno(tablero, posibles);

            int numero = entrada.LeerOpcion("Elige una ficha", 1, Fichas.Count);
            var ficha = Fichas[numero - 1];

            var opciones = posibles.Where(jugada => jugada.Ficha.Equals(ficha)).ToList();

            // Si no cabe en ningún lado se devuelve igual: la cadena de validación
            // la rechaza con su mensaje y JugadaRechazada vuelve a pedir.
            if (opciones.Count == 0)
                return new Jugada(ficha, Extremo.Derecho);

            if (opciones.Count == 1 || tablero.ExtremoIzquierdo == tablero.ExtremoDerecho)
                return opciones[0];

            entrada.Mostrar($"  1) Izquierdo ({tablero.ExtremoIzquierdo})");
            entrada.Mostrar($"  2) Derecho ({tablero.ExtremoDerecho})");

            var extremo = entrada.LeerOpcion("¿Por qué lado?", 1, 2) == 1 ? Extremo.Izquierdo : Extremo.Derecho;
            return opciones.First(jugada => jugada.Extremo == extremo);
        }

        protected override void JugadaRechazada(ResultadoValidacion resultado)
        {
            entrada.Mostrar($"No se puede: {resultado.Mensaje}");
            reintentando = true;
        }

        private void MostrarTurno(Tablero tablero, IReadOnlyList<Jugada> posibles)
        {
            if (reintentando)
            {
                reintentando = false;
                entrada.Mostrar(tablero.EstaVacia
                    ? "Elige otra ficha para abrir."
                    : $"Elige otra ficha. Extremos: {tablero.ExtremoIzquierdo} y {tablero.ExtremoDerecho}");
                return;
            }

            entrada.Mostrar();
            entrada.Mostrar($"--- Tu turno, {Nombre} ---");
            entrada.Mostrar(tablero.EstaVacia
                ? "La mesa está vacía, abres tú."
                : $"Mesa: {tablero}   (extremos {tablero.ExtremoIzquierdo} y {tablero.ExtremoDerecho})");
            entrada.Mostrar("Tu mano (* = se puede jugar):");

            for (int i = 0; i < Fichas.Count; i++)
            {
                string marca = posibles.Any(jugada => jugada.Ficha.Equals(Fichas[i])) ? " *" : "";
                entrada.Mostrar($"  {i + 1}) {Fichas[i]}{marca}");
            }
        }
    }
}
