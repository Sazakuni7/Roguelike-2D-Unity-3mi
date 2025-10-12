using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Jugador jugador;
    private bool juegoPausado = false;

    private List<Enemy> enemigosActivos = new List<Enemy>();

    public int EnemigosActivos => enemigosActivos.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetearProgresionJugador();
        ActualizarListaEnemigos();
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnVictory += HandleVictory;
        GameEvents.OnGamePaused += PauseGame;
        GameEvents.OnGameResumed += ResumeGame;

        Enemy.OnEnemyDestroyed += EliminarEnemigo;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnVictory -= HandleVictory;
        GameEvents.OnGamePaused -= PauseGame;
        GameEvents.OnGameResumed -= ResumeGame;

        Enemy.OnEnemyDestroyed -= EliminarEnemigo;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActualizarListaEnemigos();
    }

    private void ActualizarListaEnemigos()
    {
        enemigosActivos.Clear();
        enemigosActivos.AddRange(Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None));
    }

    private void ResetearProgresionJugador()
    {
        if (jugador != null)
        {
            PlayerProgressionData progresionOriginal = jugador.DatosProgresion;
            if (progresionOriginal != null)
            {
                PlayerProgressionData progresionClonada = Instantiate(progresionOriginal);
                jugador.SetDatosProgresion(progresionClonada);

                progresionClonada.experienciaNecesaria = 100;
                progresionClonada.vidaMaxima = 100;
                progresionClonada.dañoBase = 2;

                Debug.Log("Progresión del jugador reiniciada.");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) GameEvents.TriggerGameResumed();
            else GameEvents.TriggerGamePaused();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

    private void HandleVictory()
    {
        Debug.Log("Victory!");
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
        Time.timeScale = 1f;

        if (jugador != null)
        {
            Destroy(jugador.gameObject);
            jugador = null;
        }

        Spawner.Instance.playerSpawned = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Se actualiza para registrar correctamente la eliminación
    private void EliminarEnemigo(float _)
    {
        enemigosActivos.RemoveAll(e => e == null);
    }

    // Métodos para registrar enemigos desde el Spawner
    public void RegistrarEnemigo(Enemy enemigo)
    {
        if (!enemigosActivos.Contains(enemigo))
            enemigosActivos.Add(enemigo);
    }

    public void DesregistrarEnemigo(Enemy enemigo)
    {
        if (enemigosActivos.Contains(enemigo))
            enemigosActivos.Remove(enemigo);

        GameUI.Instance.ActualizarEnemigosRestantesUI();

        if (enemigosActivos.Count == 0)
            GameEvents.TriggerVictory();
    }

    public Jugador GetJugador() => jugador;

    public void SetJugador(Jugador j)
    {
        jugador = j;
        ResetearProgresionJugador();
        // Notificar a la UI que hay un jugador
        GameUI.Instance?.ConectarJugador(jugador);
    }
}
