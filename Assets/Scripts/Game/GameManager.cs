using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Jugador jugador;
    private bool juegoPausado = false;
    private bool esperandoContinuarNivel = false;

    private List<Enemy> enemigosActivos = new List<Enemy>();
    public int EnemigosActivos => enemigosActivos.Count;

    public int NivelJuego { get; private set; } = 1; // Nivel jugable actual

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (SpawnerTileManager.Instance == null)
            new GameObject("SpawnerTileManager").AddComponent<SpawnerTileManager>();
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
        GameEvents.OnNextLevel += AvanzarNivel;
        GameEvents.OnGamePaused += PauseGame;
        GameEvents.OnGameResumed += ResumeGame;

        Enemy.OnEnemyDestroyed += EliminarEnemigo;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnVictory -= HandleVictory;
        GameEvents.OnNextLevel -= AvanzarNivel;
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
            RestartLevel(mantenerProgresion: false); // reinicio limpio
        }

        // Avanzar de nivel al pulsar cualquier tecla cuando se muestra victoria
        if (esperandoContinuarNivel && Input.anyKeyDown)
        {
            esperandoContinuarNivel = false;
            RestartLevel(mantenerProgresion: true);
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
        GameUI.Instance.MostrarVictoria();
        esperandoContinuarNivel = true;
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

    private void AvanzarNivel()
    {
        RestartLevel(mantenerProgresion: true);
    }

    public void RestartLevel(bool mantenerProgresion)
    {
        Time.timeScale = 1f;

        PlayerProgressionData progresionActual = null;

        if (mantenerProgresion && jugador != null)
        {
            progresionActual = Instantiate(jugador.DatosProgresion); // Hacemos copia
            NivelJuego++;
            Debug.Log($"Subiendo a nivel jugable {NivelJuego}");
        }

        if (jugador != null)
        {
            Destroy(jugador.gameObject);
            jugador = null;
        }

        if (Spawner.Instance != null)
            Spawner.Instance.playerSpawned = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        StartCoroutine(ReaplicarProgresionJugador(progresionActual));
    }

    private IEnumerator ReaplicarProgresionJugador(PlayerProgressionData progresion)
    {
        yield return null; // Esperamos que la escena cargue

        if (progresion != null && Spawner.Instance != null)
        {
            // Instanciamos jugador en posición de spawn
            GameObject jugadorGO = Instantiate(Spawner.Instance.jugadorPrefab, Spawner.Instance.GetSpawnPosicionJugador(), Quaternion.identity);
            jugador = jugadorGO.GetComponent<Jugador>();
            jugador.SetDatosProgresion(progresion);
            Spawner.Instance.playerSpawned = true;

            GameUI.Instance.ConectarJugador(jugador);
            Debug.Log("Progresión reaplicada al siguiente nivel.");
        }
        else
        {
            // Reinicio limpio
            ResetearProgresionJugador();
            Debug.Log("Reinicio limpio: progresión del jugador reiniciada.");
        }

        AjustarDificultadEnemigos();
    }

    private void AjustarDificultadEnemigos()
    {
        float multiplicadorVida = 1f + (NivelJuego - 1) * 0.5f;

        foreach (var enemigo in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            enemigo.Inicializar(multiplicadorVida);
        }

        Debug.Log($"Enemigos escalados para nuevo nivel. Multiplicador de vida: {multiplicadorVida}");
    }

    private void EliminarEnemigo(float _)
    {
        enemigosActivos.RemoveAll(e => e == null);
    }

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

    public void RegistrarSpawnerTileDestruido(int restantes)
    {
        Debug.Log($"SpawnerTile destruido. Restantes: {restantes}");
    }

    public void TodosLosSpawnerTilesDestruidos()
    {
        Debug.Log("Todos los SpawnerTiles fueron destruidos. Eliminando enemigos activos...");

        foreach (var enemigo in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            if (enemigo != null)
                enemigo.gameObject.SetActive(false);

        enemigosActivos.Clear();
        GameUI.Instance.ActualizarEnemigosRestantesUI();

        if (Spawner.Instance != null)
        {
            Spawner.Instance.puedeSpawnear = false;
            Debug.Log("[GameManager] Spawner detenido: no hay más SpawnerTiles activos.");
        }

        // Mostrar pantalla de victoria y esperar tecla para continuar
        HandleVictory();
    }

    public Jugador GetJugador() => jugador;

    public void SetJugador(Jugador j)
    {
        jugador = j;
        ResetearProgresionJugador();
        GameUI.Instance?.ConectarJugador(jugador);
    }
}
