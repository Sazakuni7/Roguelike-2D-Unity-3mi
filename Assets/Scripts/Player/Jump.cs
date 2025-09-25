using UnityEngine;

public class Saltar : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask capaSuelo; // Capa para identificar el suelo
    [SerializeField] private Transform detectorSuelo; // Punto para verificar si está en el suelo
    [SerializeField] private float radioDeteccion; // Radio del detector

    private bool puedoSaltar = true;
    private Rigidbody2D miRigidbody2D;

    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();

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
            SaltarAccion();
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
        // Verificar si el jugador está en el suelo
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
}