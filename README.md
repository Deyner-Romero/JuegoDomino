# JuegoDomino

Proyecto final de la asignatura **Diseño basado en patrones**: Maestría en Arquitectura de Software, Universidad Antonio Nariño.

**Autor:** Deyner Antonio Romero Palomino

Sistema de juego de dominó escrito en **C# 13 sobre .NET 9**. Se aplican patrones de diseño para que el sistema admita distintas variantes del juego, tipos de jugador y sistemas de puntuación sin modificar los componentes que ya existen. El proyecto se construye en tres entregas.

---

## Contexto y problema

Una partida de dominó involucra de 2 a 4 jugadores, que juegan de forma individual o en parejas, y un juego de 28 fichas (55 en la variante doble nueve). Cada variante define sus propias reglas: quién sale, cuántas fichas se reparten, cuántos puntos se necesitan para ganar, etc.

En un diseño ingenuo esas reglas quedan repartidas por el código en condicionales, y agregar un jugador o una regla obliga a modificar clases que ya funcionan. El reto de diseño es construir un modelo donde fichas, jugadores y reglas se puedan **extender sin afectar el resto del sistema**.

### Objetivos

- **General:** diseñar un juego de dominó en C# con .NET 9 aplicando patrones de diseño, de modo que soporte distintas variantes, tipos de jugador y puntajes sin modificar los componentes existentes.
- **Específicos:**
  - Modelar el dominio del negocio mediante un diagrama de clases UML.
  - Identificar las reglas de dominio a implementar.
  - Identificar qué recursos y acciones críticas se medirán.

---

## Estructura de la solución

| Proyecto | Tipo | Responsabilidad |
|---|---|---|
| `JuegoDominio.Dominio` | Biblioteca de clases | Modelo de dominio: entidades, enumeraciones, interfaces y validaciones de jugada |
| `JuegoDomino.Aplicacion` | Biblioteca de clases | Puente entre dominio y consola: builder, estrategias, jugador automático, observadores |
| `JuegoDomino` (`JuegoDomino.Consola`) | Aplicación de consola | Interacción con el usuario: `Program`, `JugadorConsola`, `VistaConsola` |

Dirección de dependencias: **Consola → Aplicación → Dominio**, nunca al revés.

---

## Reglas del juego que se implementan

Se implementa el **dominó tradicional**, individual o en parejas:

| Regla | Decisión |
|---|---|
| Cantidad de fichas | 28 fichas, de `[0\|0]` a `[6\|6]` |
| Tipo de juego | Individual o en parejas |
| Número de jugadores | De 2 a 4 en modalidad individual; 4 en parejas (los compañeros se sientan enfrentados y el turno alterna entre parejas) |
| Reparto | Se revuelven las 28 fichas y cada jugador recibe 7. Con menos de 4 jugadores, las fichas sobrantes quedan en un pozo |
| Salida | En la primera ronda sale quien tenga el `[6\|6]`; en las siguientes sale quien ganó la ronda anterior |
| Jugada | Se coloca una ficha cuyo número coincida con un extremo de la cadena |
| Pase | Si un jugador no tiene ficha válida, pasa. **No se toman fichas del pozo** |
| Fin por dominó | La ronda termina cuando un jugador coloca todas sus fichas (en parejas, gana la pareja) |
| Fin por tranque | La ronda termina cuando los cuatro jugadores pasan seguidos. Gana la pareja con menos puntos en mano; si hay empate, nadie anota |
| Puntuación | El ganador suma los puntos de todas las fichas que no se colocaron |
| Fin de la partida | Termina cuando un jugador o una pareja alcanza el puntaje objetivo (por defecto **80**) |

### Reglas que no se implementan

| Regla | Motivo |
|---|---|
| Variante doble nueve (55 fichas) | Fuera del alcance; prevista como extensión con una nueva implementación de `IReglas` |
| Juego individual de 2 o 3 jugadores con robo del pozo | Requiere la mecánica de robo, que no existe en la modalidad por parejas |
| Dejar ver fichas o hacer señas entre compañeros | No aplica en esta versión |
| Juego en red, persistencia y deshacer jugadas | Fuera del alcance o previstos para la entrega 3 |

---

## Modelo de dominio

| Concepto | Descripción | Atributos clave |
|---|---|---|
| Ficha | Pieza con dos números del 0 al 6 | valor izquierdo, valor derecho, es doble, puntos |
| Juego de fichas | Las 28 fichas de la variante doble seis | fichas |
| Jugador | Participante con fichas en la mano que decide qué jugar | nombre, fichas en mano, puntos en mano |
| Pareja | Dos jugadores que comparten puntaje | nombre, jugadores |
| Tablero | Línea de fichas jugadas con dos extremos abiertos | fichas colocadas, extremo izquierdo, extremo derecho |
| Jugada | Colocar una ficha en uno de los extremos | ficha, extremo |
| Ronda | Reparto y secuencia de turnos hasta dominó o tranque | turno actual, pases seguidos, tipo de fin |
| Partida | Serie de rondas hasta que se alcanza la meta | marcador, historial de rondas |
| Reglas | Normas de la variante que se juega | fichas por jugador, puntos para ganar |

### Asignación de responsabilidades (GRASP / SOLID)

Cada responsabilidad va a la clase que tiene la información para cumplirla, y todo lo que cambia entre variantes queda detrás de una interfaz.

| Clase | Responsabilidad | Justificación |
|---|---|---|
| `Ficha` | Conocer sus valores, si es doble, sus puntos y si contiene un número | Es la única que tiene los dos valores; es inmutable y se puede compartir sin riesgo |
| `JuegoDeFichas` | Crear, barajar y repartir las 28 fichas | Contiene las fichas, así que es quien las crea |
| `Tablero` | Validar y registrar una jugada; calcular las jugadas posibles | Es la única clase que conoce los dos extremos abiertos |
| `Jugador` (abstracta) | Guardar sus fichas y elegir una jugada | El humano y el automático deciden distinto; la ronda los trata igual |
| `JugadorAutomatico` | Elegir una jugada válida | Las formas de jugar nuevas se agregan sin tocar la ronda |
| `JugadorConsola` | Pedir la jugada al usuario por consola | Vive en el proyecto de consola para que el dominio no dependa de la interfaz |
| `Pareja` | Agrupar dos jugadores y sumar sus puntos en mano | Concentra lo que necesita el cálculo del tranque |
| `IReglas` | Definir la ficha y el jugador de salida, la puntuación y la meta | Una variante nueva es una implementación nueva, sin modificar clases existentes |
| `Ronda` | Iniciar y finalizar un tablero | Coordina la ronda sin hacer el trabajo de la mesa ni de las reglas |
| `Partida` | Crear rondas, llevar el marcador y detectar al ganador | Agrega las rondas y registra sus resultados |
| `Program` (consola) | Armar la partida y mostrar los resultados | La interfaz depende de la aplicación y la aplicación del dominio |

---

## Patrones candidatos

| Patrón | Categoría | Problema en el diagrama | Alternativa considerada | Por qué el patrón y no la alternativa |
|---|---|---|---|---|
| **Builder** | Creacional | `Partida` requiere jugadores, parejas y reglas coherentes entre sí; un constructor extenso permite estados inválidos | Constructor con muchos parámetros / Factory Method | Valida en `Build()` y permite configurar paso a paso; Factory Method no maneja bien la construcción incremental |
| **Facade** | Estructural | La UI tendría que conocer `Partida`, `Ronda`, `Tablero`, `JuegoDeFichas` y el orden de sus llamadas | UI invocando directamente el dominio | Reduce el acoplamiento UI–dominio y permite cambiar la consola por Web sin tocar el núcleo |
| **Strategy** | Comportamiento | `JugadorAutomatico` tiene un solo algoritmo de decisión; los niveles de dificultad generarían subclases o `if` | Subclases por nivel / Bridge | Composición sobre herencia: el algoritmo se cambia en ejecución. Bridge exige dos jerarquías que varíen de forma independiente, y aquí solo varía una |
| **Observer** | Comportamiento | La vista, la bitácora y el marcador deben reaccionar a jugadas, pases y fin de ronda sin que `Ronda` los conozca | Llamadas directas desde `Ronda` a la vista | Desacopla emisor y receptores y permite agregar observadores sin modificar `Ronda` |
| **Chain of Responsibility** | Comportamiento | `Tablero.JugadaValida` acumularía muchos `if` (turno, ficha en mano, salida, extremo) | Método monolítico de validación / Specification | Cada regla queda aislada y la cadena se reconfigura por variante (OCP). Specification serviría, pero no da el corte temprano con mensaje de error |
| **Template Method** | Comportamiento | El flujo del turno no está fijado; cada subclase de `Jugador` podría alterarlo u omitir la validación | Que cada subclase implemente el turno completo | Garantiza el mismo flujo para humano y bot y concentra la variación en un solo paso. Se complementa con Strategy |
| **Bridge** | Estructural | Separar la abstracción `Jugador` de su forma de decidir | Strategy | No hay dos dimensiones que varíen de forma independiente; agrega complejidad sin beneficio (**descartado**) |
| **State** | Comportamiento | `EstadoPartida` (EnCurso, Pausada, Finalizada) condiciona las operaciones válidas | Enum + `switch` | Solo hay 3 estados con transiciones simples y el enum basta (**descartado por ahora**; se reconsidera si crecen las reglas por estado) |

---

## Patrones implementados

### 1. Observer (comportamiento)

**Problema:** cuando en la ronda se juega una ficha, un jugador pasa o la ronda termina, varias partes deben reaccionar: la vista redibuja el tablero y el historial guarda la jugada. Si `Ronda` llamara directamente a cada interesado, quedaría acoplada a la presentación y habría que modificarla con cada interesado nuevo.

**Participantes:**

| Rol | Clase |
|---|---|
| Sujeto | `Ronda` (suscribir, desuscribir, notificar) |
| Observador | `IObservadorRonda` (`FichaJugada`, `TurnoPasado`, `RondaFinalizada`) |
| Observadores concretos | `VistaConsola` (consola), `HistorialPartida` (aplicación) |
| Quien suscribe | `Partida` |

**Consecuencias:**
- ✅ `Ronda` no sabe quién la escucha. Los observadores se agregan o quitan sin tocarla, y los eventos quedan tipados por nombre.
- ⚠️ El flujo es menos visible al leer el código. Hay que desuscribir los observadores al terminar la ronda para no dejar referencias colgadas, y el orden de notificación no está garantizado.

**Alternativas descartadas:** llamadas directas (acoplan el dominio a la UI) y Mediator (aquí la relación es de uno a muchos en una sola dirección, el caso típico de Observer).

**Prueba de que sirve:** para guardar un historial y repetir la partida (replay) antes había que modificar `Ronda` en cada punto donde ocurre una jugada. Ahora basta con `HistorialPartida`, que se suscribe sin tocar `Ronda`.

### 2. Chain of Responsibility (comportamiento)

**Problema:** antes de colocar una ficha hay que comprobar que sea el turno del jugador, que la ficha esté en su mano, que en la primera jugada se respete la regla de salida (p. ej. el doble seis) y que la ficha coincida con el extremo elegido. Todo eso terminaba en `Tablero.JugadaValida` como una cadena de `if`.

**Participantes:**

| Rol | Clase |
|---|---|
| Manejador | `ValidadorJugada` (`Enlazar`, `Validar`) |
| Manejadores concretos | `ValidarTurno`, `ValidarFichaEnMano`, `ValidarSalida`, `ValidarExtremo` |
| Petición | `ContextoJugada` (ronda, jugador, jugada) → `ResultadoValidacion` |
| Cliente | `Jugador.RealizarTurno` |
| Ensamblador | `PartidaBuilder` arma la cadena |

```csharp
var validarTurno = new ValidarTurno();
validarTurno
    .Enlazar(new ValidarFichaEnMano())
    .Enlazar(new ValidarSalida())
    .Enlazar(new ValidarExtremo());
```

**Consecuencias:**
- ✅ Cada regla queda aislada en una clase que se prueba sola. La cadena se arma distinta por variante, y una regla nueva es un eslabón más (abierto/cerrado). El primer eslabón que falla corta la cadena con un mensaje claro.
- ⚠️ Hay 5 clases donde antes había un método. El orden de los eslabones importa y el compilador no lo protege, y para depurar hay que recorrer la cadena.

**Alternativas descartadas:** método único de validación (mezcla todas las reglas) y Specification (combina reglas booleanas, pero no da de forma natural el corte temprano con el mensaje del primer error).

**Prueba de que sirve:** en una variante donde la primera ronda la abre quien tenga el doble más alto, basta con reemplazar el eslabón de salida al armar la cadena, sin tocar los demás.

### 3. Strategy (comportamiento)

**Problema:** `JugadorAutomatico` decidía qué ficha jugar con un único algoritmo fijo. Para ofrecer bots de distinta dificultad había dos malas opciones: una subclase por nivel o un `switch` sobre el nivel. Además se quería poder cambiar la dificultad entre rondas sin recrear al jugador.

**Participantes:**

| Rol | Clase |
|---|---|
| Estrategia | `IEstrategiaJugador` — `Jugada Elegir(Tablero tablero, IReadOnlyList<Jugada> posibles, IReadOnlyList<Ficha> mano)` |
| Estrategias concretas | `EstrategiaAleatorea`, `EstrategiaDescartarPuntos`, `EstrategiaDefensiva` |
| Contexto | `JugadorAutomatico` (`CambiarEstrategia`) |

**Consecuencias:**
- ✅ Cada algoritmo vive en su propia clase y se puede probar de forma aislada. Agregar un nivel no toca `JugadorAutomatico`, y la estrategia se cambia en tiempo de ejecución.
- ⚠️ Hay una interfaz y 3 clases pequeñas. Quien crea al jugador debe conocer las estrategias disponibles, y la firma de `Elegir` queda fijada por el estado que recibe (tablero, mano).

**Alternativas descartadas:** Bridge (solo varía el algoritmo, no dos jerarquías) y subclases por nivel (multiplican clases y no permiten cambiar de nivel en ejecución).

**Prueba de que sirve:** un bot "Experto" que cuente las fichas ya jugadas es solo una clase nueva que implementa `IEstrategiaJugador`, sin cambiar ninguna existente.

### Pendientes (candidatos aún no implementados)

- **Builder:** `PartidaBuilder` existe y por ahora solo arma la cadena de validación; falta la construcción completa de `Partida`.
- **Facade** y **Template Method:** previstos para próximas entregas.

---

## Control de versiones del documento

| Versión | Entrega | Fecha | Sección | Cambio |
|---|---|---|---|---|
| 1.0 | 1 | 02/10/2026 | Todo el documento | Versión inicial |
| 2.0 | 2 | 29/10/2026 | Patrones candidatos | Nuevo |
| 2.0 | 2 | 29/10/2026 | Patrones de comportamiento aplicados | Nuevo |
| 2.0 | 2 | 29/10/2026 | Uso de asistentes de IA | Nuevo |
| 2.0 | 2 | 29/10/2026 | Estructura del código | Imágenes del código de los patrones |

## Uso de asistentes de IA

- **NotebookLM (Google):** investigación de patrones de diseño, diagramas UML y reglas del juego, con resúmenes, mapas mentales y cuestionarios.
- **Claude Code:** validación del código, detección de errores y pruebas del funcionamiento de la aplicación.

## Referencias

- Larman, C. (2004). *Applying UML and patterns: An introduction to object-oriented analysis and design and iterative development* (3rd ed.). Prentice Hall.
- Martin, R. C. (2017). *Clean architecture: A craftsman's guide to software structure and design*. Prentice Hall.
- Ludoteka. (s. f.). *Reglas de dominó*. https://www.ludoteka.com/juegos/domino/reglas
