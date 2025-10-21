# 🎮 YAR-L Yet Another Rogue-Like by 3mi*  
Basado en *2D Beginner: Adventure Game* de Unity Learn.  

📌 Proyecto desarrollado durante **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.7f2)

---

## 🕹️ CONTROLES
- **WASD** o **Flechas** → Moverse  
- **SPACE** → Saltar / usar Jetpack  
- **Q** → Disparar  
- **R** → Reiniciar nivel  
- **ESC** → Pausa  

---

📜 [Ver Changelog](./CHANGELOG.md)

---

## 🚨 Problemas conocidos
- ❗ Al iniciar el juego, este puede acelerarse brevemente o hacerlo después de un tiempo (importante, parece estar relacionado con el ordenador desde el cual se ejecuta el juego, debido a que en otros ordenadores no sucede esto).  
- El **jugador** a veces se frena al moverse sobre ciertos pisos o plataformas (problema parcial de colliders).  
- Muy pocas veces el jugador puede spawnear clippeado en el suelo.
- El botón de **Bugs conocidos** del menú principal muestra la lista actualizada de errores y comportamientos pendientes de revisión.  

*(Nota: el bug de aparición fuera del mapa fue solucionado.)*

---

## ✅ Caracteristicas clave

### Progresión y Jugador  
- Creación del **TDA `PlayerProgressionData`** (nivel, experiencia, daño, vida).  
- Sistema de **persistencia de jugador** entre niveles.  
- Integración completa con la UI.  

### Generación Procedural  
- Terreno generado con **Perlin Noise**, incluyendo cuevas y superficie separadas.  
- Parámetros configurables en el Inspector (width, height, smoothness, seed).  
- Tilemaps diferenciados para suelo y cuevas.  
- Regeneración con tecla **R**.  

### Spawners y Enemigos  
- 6 spawner tiles por nivel (configurable).  
- Generación balanceada de enemigos tipo Ground y Air.  
- Enemigos reaparecen indefinidamente hasta implementar boss.  

### Menú Principal  
- Interfaz funcional con opciones de:  
  - **Iniciar partida**  
  - **Ver bugs conocidos** *(nuevo botón)*  
  - **Salir del juego**  

---

## ⚔️ Sistema de Enemigos  

### 🧩 Funcionamiento

#### Spawner
- `Spawner.cs` genera enemigos tipo **Ground** y **Air** en posiciones válidas del terreno (superficie y cuevas).  
- Se asegura que el terreno esté generado antes de instanciar enemigos.  
- Genera hasta **12 enemigos** por nivel (configurable).  
- Los enemigos reaparecen hasta implementar el **endgame / boss**.  

#### IA de Enemigos
- **Ground (EnemyGroundPathing.cs):** enemigos con gravedad que patrullan y persiguen al jugador.  
- **Air (EnemyAirPathing.cs):** enemigos voladores que ignoran la gravedad.  

#### Daño, Animaciones y Sonido
- `Hurt.cs`: inflige daño al jugador por colisión.  
- `Enemy.cs`: gestiona la vida y destrucción del enemigo.  
- **Animaciones implementadas:**
  - **Ground Enemy:** idle, run, jump, attack  
  - **Air Enemy:** idle, attack  
- **NUEVO:** animaciones de recibir daño para jugador y enemigos.  
- **Efectos de sonido:**  
  - Daño, disparos, salto, explosiones y destrucción de bloques.  
  - Música de fondo.

---

## 🧑‍🎮 Jugador, Proyectiles y UI

### Jugador  
- `Jugador.cs` controla la vida, experiencia, nivel, daño, disparos y jetpack.  
- Gestiona progresión mediante `PlayerProgressionData`.  
- Implementa Singleton y persistencia entre niveles.  
- **NUEVO:** mejorada la **posición de spawn** (ya no aparece fuera del mapa ni cayendo al vacío).  
- Dispara proyectiles con dirección dependiente del movimiento.  
- Jetpack con fuel limitado y regeneración escalable con el nivel.  
- **Animaciones implementadas:** idle, run, jump, recibir daño.  
- **Efectos de sonido:** pasos, disparo, salto, daño, y jetpack.  

### Proyectiles  
- `Projectile.cs` maneja movimiento, colisión y daño a enemigos.  
- Al impactar:
  - Gana gravedad.  
  - Se vuelve inutilizado (color y física cambian).  
  - Se destruye luego de un tiempo.  

### UI del Juego  
- `GameUI.cs` muestra:
  - Vida, nivel, daño, experiencia, combustible, enemigos y spawners restantes.  
  - Mensajes de **victoria** (“¡Ganaste!”) y **derrota** (“¡Has muerto!”).  
  - Indicador de spawners actualizado dinámicamente.  
  - Pausa automática (`Time.timeScale = 0`) en eventos de fin de nivel.  

---

## 🏆 Condiciones de Victoria y Derrota  

### ✔️ Victoria
- Ocurre al destruir todos los **SpawnerTiles** del nivel.  
- Muestra el mensaje **“¡Ganaste! Pulsa cualquier tecla para jugar otra vez”**.  
- Se pausa el juego y, al presionar una tecla, se genera un **nuevo nivel** conservando la progresión del jugador.  

### ❌ Derrota
- Se produce cuando la **vida del jugador llega a 0**.  
- Se muestra **“¡Has muerto!”** y se detiene el juego.  

---

## 🚀 Mejoras Pendientes  
- Implementar **frames de invulnerabilidad** tras recibir daño.  
- Evitar que el jugador quede atascado entre enemigos.  
- Mejorar **pathfinding** de enemigos terrestres.  
- Implementar sistema de **jefe final / endgame**.  
- Integrar efectos visuales adicionales (impactos, partículas, etc).
- Cambiar de musica por nivel generado

---

## 🎮 Resultado  
- Generación procedural completa con terreno natural y cuevas.  
- Jugabilidad fluida con jetpack, disparos, progresión y niveles encadenados.  
- Retroalimentación visual y sonora con animaciones y efectos de daño.  
- UI completa e informativa actualizada en tiempo real.  
- Sistema de spawners y enemigos con condiciones de victoria funcionales.  

---

## 📂 Assets utilizados  
- Por el momento, la mayoría de los sprites y efectos fueron generados por IA o creados específicamente para el proyecto. 
- Para animar los sprites se usó [Ludo.ai](https://app.ludo.ai/sprite-generator)
- Para generar los sprites de jugador se usó [Pixellab.ai](https://www.pixellab.ai/create-character) en sección personajes
- Para generar los sprites del enemigo se usó [Pixellab.ai](https://www.pixellab.ai/create) en sección crear
- Para la música se usó [Music Creator.ai](https://www.musiccreator.ai/ai-music-generator) y [Suno](https://suno.com/create)
- Para el tileset usado para el terreno se usó [Tileset Explorer](https://donitz.itch.io/tileset-explorer)

---

✍️ Por **Emiliano Arias (3mi)**  
