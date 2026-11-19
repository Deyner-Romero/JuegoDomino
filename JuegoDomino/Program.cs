using System.Text;
using JuegoDomino.Aplicacion.Juego;
using JuegoDomino.Consola.Entrada;
using JuegoDomino.Consola.Jugadores;
using JuegoDomino.Consola.Vistas;

Console.OutputEncoding = Encoding.UTF8;

var entrada = new EntradaUsuario(Console.In, Console.Out);
var juego = new Juego();
juego.Suscribir(new VistaConsola(juego, Console.Out, TimeSpan.FromMilliseconds(400)));

try
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("===== DOMINÓ =====");
        Console.WriteLine("1) Partida individual");
        Console.WriteLine("2) Partida en parejas");
        Console.WriteLine("3) Salir");

        int opcion = entrada.LeerOpcion("Elige una opción", 1, 3);

        if (opcion == 3)
            break;

        try
        {
            CrearPartida(opcion == 2);
            JugarPartida();
        }
        catch (InvalidOperationException error)
        {
            Console.WriteLine($"No se pudo crear la partida: {error.Message}");
        }
    }
}
catch (EndOfStreamException)
{
}

Console.WriteLine("¡Hasta la próxima!");

void CrearPartida(bool enParejas)
{
    int numeroDeJugadores = enParejas ? 4 : entrada.LeerOpcion("¿Cuántos jugadores en la mesa, contándote?", 2, 4);
    string nombre = entrada.LeerTexto("Tu nombre");

    Console.WriteLine("Dificultad de los bots: 1) Fácil  2) Normal  3) Difícil");
    var dificultad = (Dificultad)(entrada.LeerOpcion("Elige la dificultad", 1, 3) - 1);

    var humano = new JugadorConsola(nombre, entrada);

    if (enParejas)
        juego.NuevaPartidaParejas(humano, dificultad);
    else
        juego.NuevaPartidaIndividual(humano, numeroDeJugadores, dificultad);

    Console.WriteLine();
    Console.WriteLine("En la mesa: " + string.Join(", ", juego.Partida.Jugadores.Select(jugador => jugador.Nombre)));

    if (enParejas)
        foreach (var pareja in juego.Partida.Parejas)
            Console.WriteLine($"  Pareja {pareja.Nombre}: {string.Join(" y ", pareja.Jugadores.Select(jugador => jugador.Nombre))}");
}

void JugarPartida()
{
    while (!juego.Terminada)
    {
        Console.WriteLine();
        Console.WriteLine($"===== Ronda {juego.RondasJugadas + 1} =====");

        juego.JugarRonda();

        if (!juego.Terminada)
            entrada.Pausa("Presiona Enter para la siguiente ronda...");
    }

    Console.WriteLine();
    Console.WriteLine("Resumen de la partida:");
    Console.WriteLine(juego.Historial.Resumen());
}
