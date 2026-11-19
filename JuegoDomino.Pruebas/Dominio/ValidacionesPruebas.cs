using JuegoDominio.Dominio.Entidades;
using JuegoDominio.Dominio.Enumeraciones;
using JuegoDominio.Dominio.Validaciones;

namespace JuegoDomino.Pruebas.Dominio
{
    public class ValidacionesPruebas
    {
        private readonly JugadorDePrueba ana = new("Ana");
        private readonly JugadorDePrueba luis = new("Luis");
        private readonly Tablero tablero = new();

        public ValidacionesPruebas()
        {
            ana.RecibirFichas([new Ficha(6, 6), new Ficha(6, 2), new Ficha(1, 3)]);
        }

        [Fact]
        public void ValidarTurno_FallaSiNoEsElTurnoDelJugador()
        {
            var resultado = new ValidarTurno().Validar(Contexto(new Ficha(6, 6), jugadorEnTurno: luis));

            Assert.False(resultado.EsValida);
            Assert.Contains("Luis", resultado.Mensaje);
        }

        [Fact]
        public void ValidarFichaEnMano_FallaSiElJugadorNoTieneLaFicha()
        {
            var resultado = new ValidarFichaEnMano().Validar(Contexto(new Ficha(5, 5)));

            Assert.False(resultado.EsValida);
        }

        [Fact]
        public void ValidarSalida_ExigeLaFichaDeSalidaEnLaPrimeraJugada()
        {
            var resultado = new ValidarSalida().Validar(Contexto(new Ficha(6, 2), fichaDeSalida: new Ficha(6, 6)));

            Assert.False(resultado.EsValida);
            Assert.Contains("[6|6]", resultado.Mensaje);
        }

        [Fact]
        public void ValidarSalida_SinFichaDeSalida_PermiteCualquierFicha()
        {
            var resultado = new ValidarSalida().Validar(Contexto(new Ficha(6, 2)));

            Assert.True(resultado.EsValida);
        }

        [Fact]
        public void ValidarExtremo_FallaSiLaFichaNoCoincide()
        {
            tablero.Registrar(new Jugada(new Ficha(6, 6), Extremo.Derecho));

            var resultado = new ValidarExtremo().Validar(Contexto(new Ficha(1, 3)));

            Assert.False(resultado.EsValida);
        }

        [Fact]
        public void Cadena_ConUnaJugadaCorrecta_EsValida()
        {
            var resultado = Cadena().Validar(Contexto(new Ficha(6, 6), fichaDeSalida: new Ficha(6, 6)));

            Assert.True(resultado.EsValida);
        }

        [Fact]
        public void Cadena_SeDetieneEnElPrimerError()
        {
            var resultado = Cadena().Validar(Contexto(new Ficha(5, 5), jugadorEnTurno: luis));

            Assert.False(resultado.EsValida);
            Assert.StartsWith("No es el turno", resultado.Mensaje);
        }

        private static ValidadorJugada Cadena()
        {
            var cadena = new ValidarTurno();
            cadena
                .Enlazar(new ValidarFichaEnMano())
                .Enlazar(new ValidarSalida())
                .Enlazar(new ValidarExtremo());
            return cadena;
        }

        private ContextoJugada Contexto(Ficha ficha, Jugador? jugadorEnTurno = null, Ficha? fichaDeSalida = null) =>
            new(ana, new Jugada(ficha, Extremo.Derecho), jugadorEnTurno ?? ana, tablero, fichaDeSalida);
    }
}
