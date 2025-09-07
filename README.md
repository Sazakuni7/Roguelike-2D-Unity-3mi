# 🎮 Unity 2D Roguelike Project by 3mi (nombre pendiente)  
Basado en *2D Beginner: Adventure Game* de Unity Learn.  

📌 Este proyecto se trabajará durante todo el cursado de **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.0f1.2588.6057)  

---

**🕹️CONTROLES:** 
- WASD o Flechas de direccion para moverse.
- SPACE para saltar.
- Q para disparar.

---

📜 [Ver Changelog](./CHANGELOG.md)

---

## ⚔️ Sistema de Enemigos  
Actualmente se añadieron scripts que permiten la generación, persecución y daño al jugador.  

### 🧩 Funcionamiento  

#### Spawner (INACTIVO)  
- `Spawner.cs` genera enemigos tipo **Ground** y **Air** en posiciones válidas del terreno (superficie del landscape).  
- Se asegura que el terreno esté generado antes de instanciar enemigos.  
- En próximas versiones, los enemigos también podrán aparecer dentro de cuevas.  

#### IA de Enemigos  
- **Ground (ChaseGround.cs):** enemigos con gravedad que persiguen al jugador caminando sobre el terreno.  
- **Air (ChaseAir.cs):** enemigos que vuelan hacia el jugador sin verse afectados por la gravedad.  

#### Daño y Vidas  
- `Hurt.cs`: permite que los enemigos inflijan daño al jugador al colisionar.  
- `Enemy.cs`: script base que gestiona la vida del enemigo y su destrucción al llegar a 0.  

---

## 🧑‍🎮 Jugador, Proyectiles y UI  

### Jugador  
- `Jugador.cs` controla la vida del jugador.  
- Detecta **muerte** y pausa el juego con `Time.timeScale = 0`.  
- Puede **disparar proyectiles** con la tecla **Q**, con cooldown configurable en el Inspector.  
- El disparo responde a la dirección en la que el jugador se mueve (izquierda/derecha).  

### Proyectiles  
- `Projectile.cs` se instancia desde el punto de disparo del jugador.  
- Se desplaza horizontalmente según dirección asignada.  
- Aplica daño a los enemigos mediante `Enemy.RecibirDaño`.  
- Tras impactar:  
  - El proyectil gana **gravedad**.  
  - Se vuelve **inutilizado** (cambia de color y se convierte en objeto físico).  
  - Se destruye luego de un tiempo.  

### UI del Juego  
- `GameUI.cs` muestra en pantalla:  
  - Vida del jugador en porcentaje.  
  - Cantidad de enemigos restantes (contador dinámico).  
- Detecta condiciones de **derrota** (vida = 0) y muestra `"Has muerto"`.  
- Detecta condiciones de **victoria** (enemigos restantes = 0) y muestra `"Has ganado"`.  

---

## 🏆 Condiciones de Victoria y Derrota  

### ✔️ Victoria  
- Se alcanza cuando **no quedan enemigos vivos** en la escena.  
- La UI muestra el mensaje: **"¡Has ganado!"**.  

### ❌ Derrota  
- Se alcanza cuando la **vida del jugador llega a 0**.  
- La UI muestra el mensaje: **"¡Has muerto!"**.  
- El juego se detiene con `Time.timeScale = 0`.  

---

# Generación Procedural de Terreno con Cuevas (DESACTIVADO TEMPORALMENTE HASTA QUE SE SOLICITE EN EL DESAFIO CORRESPONDIENTE)

Este proyecto incluye un sistema de generación procedural en 2D utilizando Perlin Noise, implementado en el script `GeneracionProcedural.cs`.

## 🧩 Funcionamiento
1. **Parámetros configurables (Inspector)**
   - `width` y `height`: dimensiones del mapa.
   - `smoothness`: suavidad de las colinas generadas.
   - `seed`: semilla aleatoria que cambia la forma del terreno.
   - `modifier`: factor que controla la densidad de cuevas.
   - `groundTile` y `caveTile`: tiles para suelo y cuevas.
   - `groundTilemap` y `caveTilemap`: tilemaps donde se pintan los resultados.

2. **Proceso de generación**
   - Se crea un array bidimensional que representa el mapa.
   - Se calcula la altura del terreno columna por columna usando **Perlin Noise**.
   - Dentro de cada columna, se aplica un segundo muestreo de Perlin Noise para decidir si una celda es **suelo** (`1`) o **cueva** (`2`).
   - Se renderiza el resultado: el suelo en un Tilemap y las cuevas en otro.

3. **Regeneración en tiempo real**
   - Con la tecla **R** se vuelve a generar el terreno con una nueva semilla.

## 🎮 Resultado
- Se obtiene un terreno irregular y natural con colinas y valles.
- El sistema de cuevas aparece de manera aleatoria en el interior del suelo.
- Al estar separado en dos Tilemaps, se puede aplicar un tratamiento visual distinto para suelo y cuevas.

---

## 🚨 Bugs Conocidos  
- El **jugador** a veces se frena al moverse sobre ciertos pisos y plataformas (problema de colliders).  
- El **jugador y enemigos de suelo** pueden **saltar indefinidamente** al atravesar plataformas con `PlatformEffector2D`.  

---

## 🚀 Mejoras Pendientes  
- Implementar **frames de invulnerabilidad** tras recibir daño (evitar múltiples tics de daño por colisión).  
- Agregar **animaciones al jugador** (idle, run, jump, shoot).  
- Agregar **sprites y animaciones para los enemigos**.  

---

## 📂 Assets utilizados  
(por completar)  


✍️ Por **Emiliano Arias (3mi)**
