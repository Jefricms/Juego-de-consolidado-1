using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Animator animator;
    private PlayerAudio playerAudio; // Referencia al script de audio

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<PlayerAudio>(); // Obtiene el script de audio del mismo objeto
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.SumarMoneda();
            }
        }
        
        if (collision.transform.CompareTag("spikes"))
        {
            // 1. Activa la animación
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }

            // 2. Reproduce el sonido de daño directamente desde la referencia
            if (playerAudio != null)
            {
                playerAudio.PlayHitSound();
            }

            // 3. Pierde la vida
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.PerderVida();
            }
        }

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
}