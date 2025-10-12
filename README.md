# 🎮 Unity 2D Roguelike Project by 3mi (nombre pendiente)  
Basado en *2D Beginner: Adventure Game* de Unity Learn.  

📌 Este proyecto se trabajará durante todo el cursado de **Programación de Videojuegos II**.  
🔧 **Engine:** Unity 6.2 (6000.2.7f2)

---

**🕹️CONTROLES:** 
- WASD o Flechas de dirección para moverse.
- SPACE para saltar / usar Jetpack.
- Q para disparar.
- R reiniciar nivel
- ESC para Pausa

---

📜 [Ver Changelog](./CHANGELOG.md)

---

## 🚨 Problemas conocidos 
- ❗ Al iniciar el juego, empieza acelerado por un breve momento o lo hace luego de un tiempo (IMPORTANTE).
- El **jugador** a veces spawnea fuera del mapa.
- El **jugador** a veces se frena al moverse sobre ciertos pisos y plataformas (problema de colliders, parcialmente arreglado).  

---

## ⚔️ Sistema de Enemigos  
Actualmente se añadieron scripts que permiten la generación, persecución y daño al jugador.  

### 🧩 Funcionamiento  

#### Spawner
- `Spawner.cs` genera enemigos tipo **Ground** y **Air** en posiciones válidas del terreno (superficie del landscape y cuevas).  
- Se asegura que el terreno esté generado antes de instanciar enemigos.  
- Se generan hasta **12 enemigos** por nivel (configurable).  
- Los enemigos pueden reaparecer indefinidamente hasta implementar un **endgame / boss**.  

#### IA de Enemigos
- **Ground (EnemyGroundPathing.cs):** enemigos con gravedad que persiguen al jugador caminando sobre el terreno, necesitan mejoras de pathing para un chasing más fluido.  
- **Air (EnemyAirPathing.cs):** enemigos que vuelan hacia el jugador sin verse afectados por la gravedad.  

#### Daño y Vidas
- `Hurt.cs`: permite que los enemigos inflijan daño al jugador al colisionar.  
- `Enemy.cs`: script base que gestiona la vida del enemigo y su destrucción al llegar a 0.  

---

### 🧑‍🎮 Jugador, Proyectiles y UI

**Jugador**  
- `Player.cs` controla la vida, progresión y disparos del jugador.  
- Gestiona experiencia, niveles y daño dinámico mediante `PlayerProgressionData`.  
- Detecta muerte y pausa el juego con `Time.timeScale = 0`.  
- Puede disparar proyectiles con la tecla Q, con cooldown configurable en el Inspector.  
- El disparo responde a la dirección en la que el jugador se mueve (izquierda/derecha).  
- Se implementó **Jetpack**: mantener Space presionado permite elevarse, con fuel limitado y regeneración.
  - Fuel máximo y regeneración aumentan con cada nivel del jugador.

**Proyectiles**  
- `Projectile.cs` se instancia desde el punto de disparo del jugador.  
- Se desplaza horizontalmente según dirección asignada.  
- Aplica daño a los enemigos mediante `Enemy.RecibirDaño`.  
- Tras impactar:  
  - El proyectil gana gravedad.  
  - Se vuelve inutilizado (cambia de color y se convierte en objeto físico).  
  - Se destruye luego de un tiempo.  

**UI del Juego**  
- `GameUI.cs` muestra en pantalla:  
  - Vida del jugador en porcentaje.  
  - Nivel del jugador.  
  - Daño actual del jugador.
  - Controles
  - Barra de experiencia (actual/experiencia necesaria).  
  - Barra de **Fuel / Jetpack** (actual / máximo).  
  - Cantidad de enemigos restantes (contador dinámico).  
  - Detecta condiciones de derrota (vida = 0) y muestra **"¡Has muerto!"**.  
  - Detecta condiciones de victoria (enemigos restantes = 0) y muestra **"¡Has ganado!"**.  

---

## 🏆 Condiciones de Victoria y Derrota  

### ✔️ Victoria (POR IMPLEMENTAR)
~~- Se alcanza cuando se derrota al boss final, invocado al destruir todos los Spawners del nivel.~~
~~- La UI muestra el mensaje: **"¡Has ganado!"**.~~

### ❌ Derrota  
- Se alcanza cuando la **vida del jugador llega a 0**.  
- La UI muestra el mensaje: **"¡Has muerto!"**.  
- El juego se detiene con `Time.timeScale = 0`.  

---

## 🚀 Mejoras Pendientes  
- Implementar **frames de invulnerabilidad** tras recibir daño (evitar múltiples tics de daño por colisión).  
- Agregar **animaciones al jugador** (idle, run, jump, shoot).  
- Agregar **sprites y animaciones para los enemigos**.  
- Mejorar **chasing de enemigos de suelo** con un sistema de pathing.
- Implementar menús.
- Implementar endgame / boss.

---

## ✅ Avances para el Desafío 3 y mejoras de gameplay

### Progresión y Player
- Creación de un **TDA (`PlayerProgressionData`)** que encapsula nivel, experiencia, vida, daño del jugador.  
- Implementación de Singleton y Scriptable Objects
- Tilemap para creación de mapa.

## 🆕 ✅ Avances para el Desafío 4 y mejoras de gameplay
En esta etapa del proyecto se incorporaron múltiples sistemas y mecánicas:

### Generación Procedural de Terreno con Cuevas
- Sistema de **generación procedural** en 2D utilizando Perlin Noise (`GeneracionProcedural.cs`).  
- **Parámetros configurables:** width, height, smoothness, seed, groundTile, caveTile, groundTilemap, caveTilemap.  
- Renderizado separado de **suelo** y **cuevas** en distintos Tilemaps.  
- **R** vuelve a generar el terreno con nueva semilla.

### Spawners y enemigos
- Se generaron **6 spawner tiles** que definen puntos de aparición de enemigos.  
- Se generan hasta **12 enemigos en pantalla** (6 Ground y 6 Air), configurable.  
- Los enemigos reaparecen indefinidamente hasta implementar endgame / boss.  
- Permite destruir bloques del terreno para alcanzar otras zonas.

### Invocación de objetos, patrones y Object Pool
- Codificación de invocación de objetos mediante procedimientos (spawners, proyectiles).  
- Ejecución de situaciones de juego mediante **corrutinas** (vida de proyectiles, temporizadores, regeneración de combustible, evento día-noche).  
- Objetos generados y tiempos de ejecución totalmente configurables.  
- Implementación de **Object Pool** para:
  - Enemigos en pantalla (destruidos y reutilizados).  
  - Proyectiles del jugador (vuelven al pool al impactar o expirar).

---

## 🎮 Resultado
- Terreno irregular y natural con colinas y valles, separado en suelo y cuevas.  
- Mecánicas de combate dinámico: disparos, destrucción de bloques y jetpack.  
- UI completa y dinámica mostrando vida, nivel, daño, experiencia, combustible y controlnes..  

---

## 📂 Assets utilizados  
(por completar)  De momento mayormente generado por IA

---

✍️ Por **Emiliano Arias (3mi)**
