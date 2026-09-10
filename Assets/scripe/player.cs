using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb2D;
    private float move;
    public float jumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private int coins;
    public TMP_Text textoCoins;

    private Animator animator;

    // --- VARIABLES DE AUDIO ---
    private AudioSource audioSource;
    public AudioClip footstepSound;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);

            // Si se está moviendo y está en el suelo, reproduce el sonido
            if (isGrounded && footstepSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = footstepSound;
                audioSource.Play();
            }
        }
        else
        {
            // Si el jugador se detiene, paramos el sonido de inmediato
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); 
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            textoCoins.text = coins.ToString();
        }
        
        if (collision.transform.CompareTag("spikes"))
        {
            if (animator != null)
                animator.SetTrigger("Hit");
            
            Invoke("ReiniciarEscena", 0.4f);
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

    void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}