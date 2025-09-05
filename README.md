# 🎮 Unity 2D Roguelike Project by 3mi (nombre pendiente)

Basado en **[Create a 2D Roguelike Game](https://learn.unity.com/project/2d-roguelike-tutorial)** de *Unity Learn*.

📌 Este proyecto se trabajará durante todo el cursado de **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.0f1.2588.6057)

---

## Generación Procedural de Terreno

El proyecto incluye un script `GeneracionProcedural.cs` que crea mapas de terreno 2D usando **Perlin Noise**.

- Los parámetros de ancho, alto, suavidad y semilla se configuran desde el Inspector.
- El terreno se representa como un array bidimensional (`int[,]`).
- Cada columna del terreno se genera según una curva de ruido Perlin.
- El resultado se dibuja en un `Tilemap` con un **Rule Tile**, que se encarga de mostrar bordes y transiciones automáticamente.
- Con la tecla **Espacio** se regenera el mapa.

Esto permite crear mundos distintos cada vez, manteniendo una apariencia coherente y suave gracias al uso de Perlin Noise.

---

## 📂 Assets utilizados
- 

---

✍️ **Por Emiliano Arias (3mi)**
