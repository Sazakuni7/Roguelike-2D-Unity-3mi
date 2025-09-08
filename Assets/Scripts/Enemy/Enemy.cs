using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float vida = 6f;

    public void RecibirDa�o(float da�o)
    {
        vida -= da�o;
        if (vida <= 0) Morir();
    }

    private void Morir()
    {
        Debug.Log(gameObject.name + " ha sido destruido.");
        Destroy(gameObject);
    }
}
