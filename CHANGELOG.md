# 📌 Changelog

## [0.1 Beta] - 2025-09-06

### ✨ Nuevas características

- **Jugador**
  - Sistema de vida y condición de muerte.
  - Capacidad de disparar proyectiles en ambas direcciones (izquierda/derecha).

- **Proyectiles**
  - Impactan contra enemigos, les quitan vida y pueden destruirlos.
  - Al impactar, ganan gravedad y quedan inutilizados como objetos en el escenario.

- **Enemigos**
  - Enemigos terrestres con persecución por suelo.
  - Enemigos aéreos que vuelan hacia el jugador.
  - Sistema de vida y destrucción al recibir daño suficiente.

- **HUD / UI**
  - Indicador de vida del jugador.
  - Contador de enemigos restantes en tiempo real.
  - Mensajes de victoria y derrota.

- **Condición de victoria**
  - El juego detecta cuando todos los enemigos han sido destruidos y muestra mensaje de victoria.

### 🐞 Bugs conocidos

- El jugador a veces se frena al moverse sobre ciertos pisos y plataformas (problema de colliders en Tilemap).
- El jugador y los enemigos terrestres pueden **saltar indefinidamente** si atraviesan plataformas con `Platform Effector`.

### 🔮 Mejoras planificadas

- Frame de **invulnerabilidad** tras recibir daño, para evitar múltiples tics de daño.
- **Animaciones del jugador** para movimiento, disparo y muerte.
- **Sprites personalizados para enemigos**.

---

## [0.2 Beta] - 2025-09-24

### ✨ Nuevas características

- **Jugador**
  - Sistema de progresión mediante `PlayerProgressionData`.
    - Experiencia acumulada al derrotar enemigos.
    - Subida de nivel con incremento de daño y vida máxima.
    - Daño dinámico según nivel.
  - Gestión de eventos de experiencia, nivel y daño para UI.

- **Proyectiles**
  - Daño configurable desde la progresión del jugador.
  - Se asegura que cada proyectil solo impacte una vez por enemigo.

- **Enemigos**
  - Al morir, otorgan experiencia al jugador.
  - Eliminación automática de la lista de enemigos activos en `GameManager` para victoria automática.

- **HUD / UI**
  - Indicador de nivel del jugador.
  - Indicador del daño actual del jugador.
  - Barra de experiencia con progreso hacia el siguiente nivel.
  - Actualización en tiempo real de experiencia y estadísticas al subir de nivel.
  - Contador dinámico de enemigos restantes.
  - Mensajes de victoria y derrota.

### 🐞 Bugs conocidos

- El juego inicia acelerado durante un breve momento (o lo hace tras un tiempo en ejecución).
- El jugador puede posicionarse dentro de las plataformas luego de saltar.

### 🔮 Mejoras planificadas

- Ajustar balance del sistema de progresión (curva de experiencia y daño).
- Spawner de enemigos reactivado con lógica mejorada para cuevas y terreno.
- Animaciones y sprites para reflejar progresión del jugador y enemigos.

---

## [0.3 Beta] - 2025-10-12

### ✨ Nuevas características

- **Jugador**
  - Se implementó **Jetpack**:
    - Se activa tras un salto y manteniendo Space presionado.
    - Movimiento horizontal y vertical controlable mientras se usa.
    - Consumo de fuel limitado, con regeneración cuando está en el suelo.
    - Fuel máximo y regeneración aumentan al subir de nivel.
  - Mejor integración con la UI: barra de fuel visible en tiempo real.
  - Experiencia, nivel y daño continúan actualizándose en tiempo real.

- **Generación Procedural**
  - Terreno generado dinámicamente usando Perlin Noise.
  - Cuevas y suelo separados en distintos Tilemaps.
  - Tecla **R** regenera el terreno con nueva semilla.
  - Bloques destructibles que permiten al jugador acceder a nuevas zonas.

- **Spawners y enemigos**
  - Se generan 6 spawner tiles para aparición de enemigos.
  - Máximo 12 enemigos activos en pantalla (6 Ground, 6 Air), configurable.
  - Los enemigos reaparecen indefinidamente hasta implementar endgame / boss.
  - IA mejorada para seguimiento y persecución del jugador (mejorar pathing pendiente).

- **Object Pool**
  - Se implementa **Object Pool** para:
    - Proyectiles del jugador.
    - Enemigos generados y reutilizados al morir.
  - Configuración de objetos generados y tiempo de ejecución completamente editable desde Inspector.

- **HUD / UI**
  - Barra de fuel para el Jetpack.
  - Indicadores de vida, daño, nivel y experiencia actualizados dinámicamente.

### 🐞 Bugs conocidos

- El jugador puede saltar levemente con fuel bajo antes de activar el jetpack, causando pequeños saltos no deseados.
- Mejorar el chasing de enemigos terrestres mediante pathing más inteligente.

### 🔮 Mejoras planificadas

- Añadir animaciones de Jetpack al jugador.
- Mejorar IA de enemigos de suelo para evitar bloqueos o movimientos erráticos.
- Implementar endgame / boss tras destruir spawner tiles.
- Ajustar balance de fuel, regeneración y fuerza del jetpack.
