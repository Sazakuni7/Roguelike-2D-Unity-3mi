using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Jugador jugador;

    private bool juegoPausado = false;

    // Lista para gestionar enemigos activos
    private List<Enemy> enemigosActivos = new List<Enemy>();

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruir instancias duplicadas
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistir entre escenas
    }

    private void Start()
    {
        // Inicializar la lista de enemigos activos
        enemigosActivos.AddRange(FindObjectsOfType<Enemy>());
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnVictory += HandleVictory;
        GameEvents.OnGamePaused += PauseGame;
        GameEvents.OnGameResumed += ResumeGame;

        // Suscribirse al evento de destrucción de enemigos
        Enemy.OnEnemyDestroyed += EliminarEnemigo;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnVictory -= HandleVictory;
        GameEvents.OnGamePaused -= PauseGame;
        GameEvents.OnGameResumed -= ResumeGame;

        // Cancelar la suscripción al evento de destrucción de enemigos
        Enemy.OnEnemyDestroyed -= EliminarEnemigo;
    }

    private void Update()
    {
        // Pausar o reanudar el juego con la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                GameEvents.TriggerGameResumed();
            }
            else
            {
                GameEvents.TriggerGamePaused();
            }
        }

        // Reiniciar el juego con la tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f; // Detener el tiempo al perder
    }

    private void HandleVictory()
    {
        Debug.Log("Victory!");
        // No detenemos el tiempo al ganar
    }

    private void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    private void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté corriendo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    private void EliminarEnemigo(float _)
    {
        // Eliminar enemigos destruidos de la lista
        enemigosActivos.RemoveAll(e => e == null);

        // Verificar si todos los enemigos han sido eliminados
        if (enemigosActivos.Count == 0)
        {
            GameEvents.TriggerVictory();
        }
    }
}