using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Determina el timestep fijo de la física (FixedUpdate)")]
    [SerializeField] private float fixedTimeStep = 0.02f;

    [Tooltip("Activar todos los Rigidbody2D al inicio")]
    [SerializeField] private bool resetRigidbodyVelocities = true;

    private void Awake()
    {
        // Reset de tiempo
        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedTimeStep;

        // Frame rate seguro
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        if (resetRigidbodyVelocities)
        {
            Rigidbody2D[] rigidbodies = Object.FindObjectsByType<Rigidbody2D>(
                FindObjectsSortMode.None);
            foreach (var rb in rigidbodies)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        Debug.Log("[GameInitializer] Inicialización completada: TimeScale=" + Time.timeScale +
                  ", FixedDeltaTime=" + Time.fixedDeltaTime +
                  ", Physics2D.simulationMode=" + Physics2D.simulationMode);

        // 5️⃣ Activar scripts de movimiento después de un frame
        StartCoroutine(ActivarScriptsMovimiento());
    }

    private IEnumerator ActivarScriptsMovimiento()
    {
        yield return null; // espera 1 frame para estabilizar deltaTime

        // Activar todos los scripts que controlan movimiento
        MonoBehaviour[] scriptsMovimiento = Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsSortMode.None);

        foreach (var script in scriptsMovimiento)
        {
            // Activar solo los scripts de jugador/enemigos que tengan "activo" como variable
            var tipo = script.GetType();
            var field = tipo.GetField("activo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(script, true);
            }
        }

        Debug.Log("[GameInitializer] Scripts de movimiento activados.");
    }
}
