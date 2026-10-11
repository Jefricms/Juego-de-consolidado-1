using UnityEngine;

public class EnemigoVerde : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float distanciaPatrulla = 3f;

    private Vector3 posicionInicial;
    private bool moviendoDerecha = true;
    private Animator anim;

    void Start()
    {
        posicionInicial = transform.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento de patrulla
        if (moviendoDerecha)
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1);

            if (transform.position.x >= posicionInicial.x + distanciaPatrulla)
            {
                moviendoDerecha = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);

            if (transform.position.x <= posicionInicial.x - distanciaPatrulla)
            {
                moviendoDerecha = true;
            }
        }

        // Animación de correr
        if (anim != null)
        {
            anim.SetFloat("Speed", velocidad);
        }
    }
}