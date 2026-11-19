using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDomino.Aplicacion.Juego;
using JuegoDomino.Consola.Entrada;
using JuegoDomino.Consola.Jugadores;
using JuegoDomino.Consola.Vistas;
using JuegoDomino.Pruebas.Dominio;

namespace JuegoDomino.Pruebas.Consola
{
    public class ConsolaPruebas
    {
        private readonly StringWriter salida = new();

        private EntradaUsuario Entrada(params string[] lineas) =>
            new(new StringReader(string.Join(Environment.NewLine, lineas)), salida);

        [Fact]
        public void LeerOpcion_IgnoraTextoYNumerosFueraDeRango()
        {
            var entrada = Entrada("hola", "9", "2");

            int opcion = entrada.LeerOpcion("Elige", 1, 3);

            Assert.Equal(2, opcion);
            Assert.Contains("Escribe un número entre 1 y 3.", salida.ToString());
        }

        [Fact]
        public void SiSeCierraLaEntrada_LanzaEndOfStream()
        {
            var entrada = Entrada();

            Assert.Throws<EndOfStreamException>(() => entrada.LeerTexto("Nombre"));
        }

        [Fact]
        public void JugadorConsola_SiEligeUnaFichaQueNoEsLaDeSalida_SeLeExplicaYSeLePideOtra()
        {
            var jugador = new JugadorConsola("Ana", Entrada("1", "2"));
            jugador.RecibirFichas([new Ficha(1, 2), new Ficha(6, 6)]);

            var jugada = jugador.RealizarTurno(new Tablero(), Cadena.DeValidacion(), jugador, fichaDeSalida: new Ficha(6, 6));

            Assert.Equal(new Ficha(6, 6), jugada!.Ficha);
            Assert.Contains("debe abrirse con la ficha [6|6]", salida.ToString());
        }

        [Fact]
        public void JugadorConsola_MarcaLasFichasQueSePuedenJugar()
        {
            var tablero = new Tablero();
            tablero.Registrar(new Jugada(new Ficha(6, 6), Extremo.Derecho));
            var jugador = new JugadorConsola("Ana", Entrada("2"));
            jugador.RecibirFichas([new Ficha(1, 2), new Ficha(6, 3)]);

            jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador);

            Assert.Contains("1) [1|2]" + Environment.NewLine, salida.ToString());
            Assert.Contains("2) [6|3] *", salida.ToString());
        }

        [Fact]
        public void JugadorConsola_SiLaFichaCabeEnLosDosLados_PreguntaElExtremo()
        {
            var tablero = new Tablero();
            tablero.Registrar(new Jugada(new Ficha(2, 6), Extremo.Derecho));
            tablero.Registrar(new Jugada(new Ficha(6, 5), Extremo.Derecho));
            var jugador = new JugadorConsola("Ana", Entrada("1", "1"));
            jugador.RecibirFichas([new Ficha(2, 5)]);

            var jugada = jugador.RealizarTurno(tablero, Cadena.DeValidacion(), jugador);

            Assert.Equal(Extremo.Izquierdo, jugada!.Extremo);
            Assert.Equal("[5|2][2|6][6|5]", tablero.ToString());
        }

        [Fact]
        public void VistaConsola_MuestraLasJugadasLosResultadosYElGanador()
        {
            var juego = new Juego(new Random(4));
            juego.Suscribir(new VistaConsola(juego, salida));
            juego.NuevaPartidaIndividual(new JugadorDePrueba("Ana"), 2, Dificultad.Normal);

            juego.Jugar();

            string texto = salida.ToString();
            Assert.Contains("abre con", texto);
            Assert.Contains("Marcador (se juega a 80):", texto);
            Assert.Contains($"¡{juego.NombreDelGanador()} gana la partida!", texto);
        }
    }
}
