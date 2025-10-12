using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraSetup : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera; // Asigna la CinemachineCamera en el inspector

    private void Start()
    {
        // Buscar al jugador en la escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && cinemachineCamera != null)
        {
            // Asignar al jugador como el objetivo de seguimiento
            cinemachineCamera.Follow = player.transform;
            cinemachineCamera.LookAt = player.transform;

        }

    }
}