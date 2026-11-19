using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class JuegoDeFichasPruebas
    {
        [Fact]
        public void Constructor_CreaLas28FichasDistintas()
        {
            var juego = new JuegoDeFichas();

            Assert.Equal(28, juego.Cantidad);
            Assert.Equal(28, juego.Fichas.Distinct().Count());
            Assert.Equal(168, juego.Fichas.Sum(ficha => ficha.Puntos));
            Assert.Equal(7, juego.Fichas.Count(ficha => ficha.EsDoble));
        }

        [Fact]
        public void Repartir_EntregaSieteFichasACadaJugadorSinRepetir()
        {
            var juego = new JuegoDeFichas();
            var jugadores = CrearJugadores(4);

            juego.Repartir(jugadores, 7);

            Assert.All(jugadores, jugador => Assert.Equal(7, jugador.FichasRestantes));
            Assert.Equal(28, jugadores.SelectMany(jugador => jugador.Fichas).Distinct().Count());
            Assert.Empty(juego.Pozo);
        }

        [Fact]
        public void Repartir_ConMenosDeCuatroJugadores_DejaFichasEnElPozo()
        {
            var juego = new JuegoDeFichas();
            var jugadores = CrearJugadores(2);

            juego.Repartir(jugadores, 7);

            Assert.Equal(14, juego.Pozo.Count);
            Assert.Empty(juego.Pozo.Intersect(jugadores.SelectMany(jugador => jugador.Fichas)));
        }

        [Fact]
        public void Repartir_ConLaMismaSemilla_ProduceElMismoReparto()
        {
            var primeros = CrearJugadores(4);
            var segundos = CrearJugadores(4);

            new JuegoDeFichas(azar: new Random(42)).Repartir(primeros, 7);
            new JuegoDeFichas(azar: new Random(42)).Repartir(segundos, 7);

            Assert.Equal(primeros[0].Fichas, segundos[0].Fichas);
        }

        [Fact]
        public void Repartir_SinFichasSuficientes_LanzaExcepcion()
        {
            var juego = new JuegoDeFichas();

            Assert.Throws<InvalidOperationException>(() => juego.Repartir(CrearJugadores(5), 7));
        }

        private static List<Jugador> CrearJugadores(int cantidad) =>
            Enumerable.Range(1, cantidad).Select(numero => (Jugador)new JugadorDePrueba($"Jugador {numero}")).ToList();
    }
}
