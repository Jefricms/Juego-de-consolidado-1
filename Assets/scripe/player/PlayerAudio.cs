using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private AudioSource audioSource;
    
    [Header("Clips de Audio")]
    public AudioClip footstepSound;
    public AudioClip jumpSound;
    public AudioClip hitSound;

    // Referencias para saber si está en el suelo
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. ESCUDO: Si está sonando el salto o el golpe, los pasos NO pueden interrumpir
        if (audioSource.isPlaying && (audioSource.clip == jumpSound || audioSource.clip == hitSound))
        {
            return; // Sale del Update y deja que el salto/golpe suene en paz
        }

        // Detecta si se está moviendo horizontalmente y está en el suelo
        bool isMoving = Mathf.Abs(rb2D.linearVelocity.x) > 0.1f;

        if (isMoving && isGrounded)
        {
            if (footstepSound != null && audioSource.clip != footstepSound)
            {
                audioSource.clip = footstepSound;
            }

            if (footstepSound != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Si se detiene o está en el aire y sonaba el paso, lo detenemos
            if (audioSource.isPlaying && audioSource.clip == footstepSound)
            {
                audioSource.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }
    }

    // --- FUNCIONES PÚBLICAS ---

    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.Stop();
            audioSource.clip = jumpSound;
            audioSource.Play();
        }
    }

    public void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }
}