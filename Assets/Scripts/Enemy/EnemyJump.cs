using UnityEngine;

public class AutoSalto : MonoBehaviour
{
    [SerializeField] private float fuerzaSalto = 5f;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb != null)
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
    }
}
