# Changelog

## [0.1 Beta] - 2025-09-06

### Nuevas características
- Jugador
  - Sistema de vida y condición de muerte.
  - Capacidad de disparar proyectiles en ambas direcciones (izquierda/derecha).
- Proyectiles
  - Impactan contra enemigos, les quitan vida y pueden destruirlos.
  - Al impactar, ganan gravedad y quedan inutilizados como objetos en el escenario.
- Enemigos
  - Enemigos terrestres con persecución por suelo.
  - Enemigos aéreos que vuelan hacia el jugador.
  - Sistema de vida y destrucción al recibir daño suficiente.
- HUD / UI
  - Indicador de vida del jugador.
  - Contador de enemigos restantes en tiempo real.
  - Mensajes de victoria y derrota.
- Condición de victoria
  - El juego detecta cuando todos los enemigos han sido destruidos y muestra mensaje de victoria.

### Bugs conocidos
- El jugador a veces se frena al moverse sobre ciertos pisos y plataformas (problema de colliders en Tilemap).
- El jugador y los enemigos terrestres pueden saltar indefinidamente si atraviesan plataformas con `Platform Effector`.

### Mejoras planificadas
- Frame de invulnerabilidad tras recibir daño, para evitar múltiples tics de daño.
- Animaciones del jugador para movimiento, disparo y muerte.
- Sprites personalizados para enemigos.
