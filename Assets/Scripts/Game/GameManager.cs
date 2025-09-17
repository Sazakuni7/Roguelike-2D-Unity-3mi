using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Jugador jugador;

    private bool juegoPausado = false;

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

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnVictory += HandleVictory;
        GameEvents.OnGamePaused += PauseGame;
        GameEvents.OnGameResumed += ResumeGame;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnVictory -= HandleVictory;
        GameEvents.OnGamePaused -= PauseGame;
        GameEvents.OnGameResumed -= ResumeGame;
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
}