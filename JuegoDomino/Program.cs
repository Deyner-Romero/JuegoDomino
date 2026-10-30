// See https://aka.ms/new-console-template for more information
using JuegoDominio.Dominio.Entidades;
using JuegoDomino.Consola.Jugadores;

Console.WriteLine("Bienvenidos al juego! \n" +
    "Seleciona una opcion:  1. Nueva partida individual, 2. Nueva partida en parejas, 3. salir");
string? opcionMenu = Console.ReadLine();
int numeroJugadores = 0;
List<Pareja> parejas = new List<Pareja>();
List<JugadorConsola> jugadoresConsola = new List<JugadorConsola>();
List<Jugador> jugadores = new List<Jugador>();

if (opcionMenu != null && opcionMenu.Equals("3")) Environment.Exit(0);

if (opcionMenu != null && opcionMenu.Equals("1"))
{
    Console.WriteLine("Ingresa el numero de jugadores.");
    numeroJugadores = Convert.ToInt32(Console.ReadLine());
}