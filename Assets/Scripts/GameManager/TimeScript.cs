using UnityEngine;

public class DebugTime : MonoBehaviour
{
    private void Update()
    {
        Debug.Log($"Time.timeScale: {Time.timeScale}, Time.fixedDeltaTime: {Time.fixedDeltaTime}, FPS: {1.0f / Time.deltaTime}");
    }
}