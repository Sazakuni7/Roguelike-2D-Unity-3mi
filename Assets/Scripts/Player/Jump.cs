using UnityEngine;

public class Saltar : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask capaSuelo; // Capa para identificar el suelo
    [SerializeField] private Transform detectorSuelo; // Punto para verificar si está en el suelo
    [SerializeField] private float radioDeteccion;

    private bool puedoSaltar = true;
    private Rigidbody2D miRigidbody2D;
    private AudioSource miAudioSource;

    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAudioSource = GetComponent<AudioSource>();

        // Reset inicial de velocidad
        if (miRigidbody2D != null)
        {
            miRigidbody2D.linearVelocity = Vector2.zero;
            miRigidbody2D.angularVelocity = 0f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            bool estabaEnSuelo = EstoyEnElSuelo(); // Verificamos si estaba en el suelo antes del salto

            SaltarAccion();

            // Solo reproducir sonido si el salto fue desde el suelo
            if (estabaEnSuelo && miAudioSource != null && jumpSFX != null)
            {
                if (!miAudioSource.isPlaying)
                    miAudioSource.PlayOneShot(jumpSFX);
            }
        }
    }

    private void SaltarAccion()
    {
        if (miRigidbody2D != null && EstoyEnElSuelo())
        {
            miRigidbody2D.linearVelocity = new Vector2(miRigidbody2D.linearVelocity.x, 0f); // Resetear velocidad vertical antes de saltar
            miRigidbody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            puedoSaltar = false;
        }
    }

    private void FixedUpdate()
    {
        if (EstoyEnElSuelo())
        {
            puedoSaltar = true;
        }
    }

    private bool EstoyEnElSuelo()
    {
        // Usar un CircleCast para verificar si el jugador está tocando el suelo
        return Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, capaSuelo);
    }

    private void OnDrawGizmos()
    {
        // Dibujar el área de detección del suelo en la escena
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioDeteccion);
        }
    }

    public bool EstaEnSuelo() => EstoyEnElSuelo();
}