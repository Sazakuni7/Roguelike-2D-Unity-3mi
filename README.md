# 🎮 Unity 2D Roguelike Project by 3mi (nombre pendiente)

Basado en **[Create a 2D Roguelike Game](https://learn.unity.com/project/2d-roguelike-tutorial)** de *Unity Learn*.

📌 Este proyecto se trabajará durante todo el cursado de **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.0f1.2588.6057)

---
# Generación Procedural de Terreno con Cuevas

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
   - Con la tecla **Espacio** se vuelve a generar el terreno con una nueva semilla.

## 🎮 Resultado
- Se obtiene un terreno irregular y natural con colinas y valles.
- El sistema de cuevas aparece de manera aleatoria en el interior del suelo.
- Al estar separado en dos Tilemaps, se puede aplicar un tratamiento visual distinto para suelo y cuevas.

---

## 📂 Assets utilizados
- 

---

✍️ **Por Emiliano Arias (3mi)**
