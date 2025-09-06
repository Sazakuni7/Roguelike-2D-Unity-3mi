# 🎮 Unity 2D Roguelike Project by 3mi (nombre pendiente)

Basado en **[2D Beginner: Adventure Game]([(https://learn.unity.com/course/2d-beginner-adventure-game?version=2022.3)])** de *Unity Learn*.

📌 Este proyecto se trabajará durante todo el cursado de **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.0f1.2588.6057)

---

# ⚔️ Sistema de Enemigos

Actualmente se añadieron scripts que permiten la **generación, persecución y daño al jugador**.

## 🧩 Funcionamiento
1. **Spawner**
   - Script `Spawner.cs` genera enemigos tipo *Ground* en posiciones válidas del terreno (superficie del landscape).
   - Se asegura que el terreno esté generado antes de instanciar enemigos.
   - En próximas versiones, los enemigos también podrán aparecer dentro de cuevas.

2. **IA de Enemigos**
   - **Ground (`ChaseGround.cs`)**: enemigos con gravedad que persiguen al jugador caminando sobre el terreno.
   - **Air (`ChaseAir.cs`)**: enemigos que vuelan hacia el jugador sin verse afectados por la gravedad.

3. **Daño y Vidas**
   - Script `Hurt.cs` permite que los enemigos inflijan daño al jugador al colisionar.
   - Sistema de vidas implementado, aunque aún requiere ajustes para funcionar al 100%.

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

## 📂 Assets utilizados
- *(por completar)*

---

✍️ **Por Emiliano Arias (3mi)**
