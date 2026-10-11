using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Vector3 puntoDeInicio;
    private bool esInvulnerable = false;
    [SerializeField] private float tiempoInvulnerable = 1.2f; // Tiempo de protección
    [SerializeField] private float duracionAnimacionHit = 0.35f; // Tiempo que se queda viendo el golpe antes de reaparecer

    void Start()
    {
        ActualizarPuntoInicio();
    }

    public void ActualizarPuntoInicio()
    {
        GameObject spawn = GameObject.Find("SpawnPoint");
        if (spawn == null) spawn = GameObject.Find("Inicio");

        if (spawn != null)
        {
            puntoDeInicio = spawn.transform.position;
        }
        else
        {
            puntoDeInicio = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Monedas
        if (collision.transform.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            PlayerStats stats = GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.SumarMoneda();
            }
            else if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.SumarMoneda();
            }
        }

        // 2. Pinchos / Trampas / Enemigos
        if (collision.transform.CompareTag("spikes"))
        {
            if (!esInvulnerable)
            {
                RecibirDanoSpikes();
            }
        }

        // 4. Barril
        if (collision.transform.CompareTag("Baril"))
        {
            Animator barrelAnimator = collision.GetComponent<Animator>();
            if (barrelAnimator != null)
            {
                barrelAnimator.SetTrigger("Explode");
            }

            Destroy(collision.gameObject, 0.5f);
        }
    }

    private void RecibirDanoSpikes()
    {
        esInvulnerable = true;

        // PlayerStats ya se encarga de restar vida, reproducir el audio y lanzar el trigger "Hit"
        GetComponent<PlayerMovement>()?.BloquearMovimiento(duracionAnimacionHit);
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.PerderVida();

            if (PlayerStats.Instance.vidas > 0)
            {
                // Espera un instante visible para mostrar la animación antes de reubicar
                Invoke(nameof(ReaparecerEnInicio), duracionAnimacionHit);
            }
        }

        Invoke(nameof(QuitarInvulnerabilidad), tiempoInvulnerable);
    }

    private void RecibirDanoCaida()
    {
        esInvulnerable = true;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.PerderVida();

            if (PlayerStats.Instance.vidas > 0)
            {
                ReaparecerEnInicio();
            }
        }

        Invoke(nameof(QuitarInvulnerabilidad), 0.5f);
    }

    private void ReaparecerEnInicio()
    {
        ActualizarPuntoInicio();
        transform.position = puntoDeInicio;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void QuitarInvulnerabilidad()
    {
        esInvulnerable = false;
    }
}