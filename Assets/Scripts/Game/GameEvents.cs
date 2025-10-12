using System;

public static class GameEvents
{
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public static event Action OnVictory;

    public static event Action<Jugador> OnPlayerSpawned;
    public static void TriggerGamePaused() => OnGamePaused?.Invoke();
    public static void TriggerGameResumed() => OnGameResumed?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerVictory() => OnVictory?.Invoke();

    public static void TriggerPlayerSpawned(Jugador jugador) => OnPlayerSpawned?.Invoke(jugador);
}