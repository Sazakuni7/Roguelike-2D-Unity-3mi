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