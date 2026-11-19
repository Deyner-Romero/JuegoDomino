using JuegoDominio.Dominio.Entidades;

namespace JuegoDomino.Pruebas.Dominio
{
    public class ParejaPruebas
    {
        [Fact]
        public void Agregar_PermiteHastaDosJugadores()
        {
            var pareja = new Pareja("Pareja A");
            pareja.Agregar(new JugadorDePrueba("Ana"));
            pareja.Agregar(new JugadorDePrueba("Luis"));

            Assert.Throws<InvalidOperationException>(() => pareja.Agregar(new JugadorDePrueba("Otro")));
        }

        [Fact]
        public void Agregar_NoPermiteRepetirJugador()
        {
            var pareja = new Pareja("Pareja A");
            var ana = new JugadorDePrueba("Ana");
            pareja.Agregar(ana);

            Assert.Throws<InvalidOperationException>(() => pareja.Agregar(ana));
        }

        [Fact]
        public void Incluye_ReconoceASusIntegrantes()
        {
            var pareja = new Pareja("Pareja A");
            var ana = new JugadorDePrueba("Ana");
            pareja.Agregar(ana);

            Assert.True(pareja.Incluye(ana));
            Assert.False(pareja.Incluye(new JugadorDePrueba("Luis")));
        }

        [Fact]
        public void PuntosEnMano_SumaLosPuntosDeAmbosJugadores()
        {
            var ana = new JugadorDePrueba("Ana");
            var luis = new JugadorDePrueba("Luis");
            ana.RecibirFichas([new Ficha(1, 2)]);
            luis.RecibirFichas([new Ficha(5, 5)]);
            var pareja = new Pareja("Pareja A");
            pareja.Agregar(ana);
            pareja.Agregar(luis);

            Assert.Equal(13, pareja.PuntosEnMano);
        }
    }
}
