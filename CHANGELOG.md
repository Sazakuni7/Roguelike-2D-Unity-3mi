# 📌 Changelog

## [0.1 Beta] - 2025-09-06
### ✨ Nuevas características

**Jugador**
- Sistema de vida y condición de muerte.
- Capacidad de disparar proyectiles en ambas direcciones (izquierda/derecha).

**Proyectiles**
- Impactan contra enemigos, les quitan vida y pueden destruirlos.
- Al impactar, ganan gravedad y quedan inutilizados como objetos en el escenario.

**Enemigos**
- Enemigos terrestres con persecución por suelo.
- Enemigos aéreos que vuelan hacia el jugador.
- Sistema de vida y destrucción al recibir daño suficiente.

**HUD / UI**
- Indicador de vida del jugador.
- Contador de enemigos restantes en tiempo real.
- Mensajes de victoria y derrota.

**Condición de victoria**
- El juego detecta cuando todos los enemigos han sido destruidos y muestra mensaje de victoria.

### 🐞 Bugs conocidos
- El jugador a veces se frena al moverse sobre ciertos pisos y plataformas (problema de colliders en Tilemap).
- El jugador y los enemigos terrestres pueden saltar indefinidamente si atraviesan plataformas con Platform Effector.

---

## [0.2 Beta] - 2025-09-24
### ✨ Nuevas características

**Jugador**
- Sistema de progresión mediante `PlayerProgressionData`.
- Experiencia acumulada al derrotar enemigos.
- Subida de nivel con incremento de daño y vida máxima.
- Daño dinámico según nivel.
- Gestión de eventos de experiencia, nivel y daño para UI.

**Proyectiles**
- Daño configurable desde la progresión del jugador.
- Se asegura que cada proyectil solo impacte una vez por enemigo.

**Enemigos**
- Al morir, otorgan experiencia al jugador.
- Eliminación automática de la lista de enemigos activos en `GameManager` para victoria automática.

**HUD / UI**
- Indicador de nivel del jugador.
- Indicador del daño actual del jugador.
- Barra de experiencia con progreso hacia el siguiente nivel.
- Actualización en tiempo real de experiencia y estadísticas al subir de nivel.
- Contador dinámico de enemigos restantes.
- Mensajes de victoria y derrota.

### 🐞 Bugs conocidos
- El juego inicia acelerado durante un breve momento (o lo hace tras un tiempo en ejecución).
- El jugador puede posicionarse dentro de las plataformas luego de saltar.

---

## [0.3 Beta] - 2025-10-12
### ✨ Nuevas características

**Jugador**
- Se implementó Jetpack:
  - Se activa tras un salto y manteniendo `Space` presionado.
  - Movimiento horizontal y vertical controlable mientras se usa.
  - Consumo de fuel limitado, con regeneración cuando está en el suelo.
  - Fuel máximo y regeneración aumentan al subir de nivel.
- Mejor integración con la UI: barra de fuel visible en tiempo real.
- Experiencia, nivel y daño continúan actualizándose en tiempo real.
- Animaciones implementadas: Idle, Correr, Saltar y Recibir daño.

**Generación Procedural**
- Terreno generado dinámicamente usando Perlin Noise.
- Cuevas y suelo separados en distintos Tilemaps.
- Tecla `R` regenera el terreno con nueva semilla.
- Bloques destructibles que permiten al jugador acceder a nuevas zonas.

**Spawners y enemigos**
- Se generan 6 spawner tiles para aparición de enemigos.
- Máximo 12 enemigos activos en pantalla (6 Ground, 6 Air), configurable.
- Los enemigos reaparecen indefinidamente hasta implementar endgame / boss.
- IA mejorada para seguimiento y persecución del jugador.
- Animaciones implementadas:
  - Enemigo Air: Idle y Attack.
  - Enemigo Ground: Idle, Run, Jump y Attack.

**Object Pool**
- Se implementa Object Pool para:
  - Proyectiles del jugador.
  - Enemigos generados y reutilizados al morir.
- Configuración editable desde el Inspector.

**HUD / UI**
- Barra de fuel para el Jetpack.
- Indicadores de vida, daño, nivel y experiencia actualizados dinámicamente.
- Sonidos y música agregados: efectos de daño, disparo y música de fondo.

### 🐞 Bugs conocidos
- El jugador puede saltar levemente con fuel bajo antes de activar el jetpack, causando pequeños saltos no deseados.
- Mejorar el chasing de enemigos terrestres mediante pathing más inteligente.

---

## [0.4 Beta] - 2025-10-20
### ✨ Nuevas características

**Jugador**
- Se mejoró la posición de spawn, evitando que aparezca fuera del nivel o sobre el vacío.
- Se mantienen correctamente los datos de progresión al avanzar entre niveles.
- Animación de recibir daño añadida, sincronizada con efectos sonoros.

**Enemigos**
- Se agregaron animaciones de recibir daño para enemigos terrestres y aéreos.
- Se añadieron partículas y efectos visuales al destruir spawner tiles.
- Ajustes en la generación de enemigos: ahora se crean exactamente 6 spawner tiles válidos.
- Coordinación mejorada con `GameManager` para actualización de conteo y reinicio de niveles.

**Luces y ambientación**
- Se incorporaron Spot Lights y una Global Light 2D para mejorar la iluminación general.
- Ajustes visuales en sombras y efectos de partículas para una mejor atmósfera.

**Audio**
- Música de menú principal y música del Nivel 1 implementadas.
- Nuevos efectos de sonido para daño, disparo, spawner y victoria.
- Control de volumen y mezcla general ajustados desde el `AudioManager`.

**Interfaz y Menú Principal**
- Nuevo menú principal con:
  - Botón Jugar (inicia el juego desde Nivel 1).
  - Sección Controles / Objetivos (muestra información de gameplay).
  - Botón Bugs conocidos (lista de errores actuales del proyecto).
  - Botón Salir (cierra la aplicación).
- Transiciones y música del menú implementadas correctamente.

**Sistema de niveles**
- Al completar un nivel, se pausa el juego y se muestra un mensaje de victoria.
- Al presionar una tecla, se carga el siguiente nivel conservando la progresión.
- Mensajes dinámicos y reinicio fluido entre niveles.

### 🐞 Bugs conocidos
- Persisten errores visuales temporales en el recuento de spawner tiles durante carga de niveles (muestra “12/6”).
- Algunas animaciones pueden solaparse con la de recibir daño.

### 🔮 Mejoras planificadas
- Ajustar sincronización entre animaciones y efectos de daño.
- Agregar un jefe al final de cada nivel
- Agregar menú de pausa con opciones de reinicio y salida.
- Optimizar carga de niveles y generación procedural para reducir picos de rendimiento.
- Implementar nuevos niveles y ajustes de dificultad progresiva.
- Incorporar elementos de gameplay que vuelvan mas divertido al juego
