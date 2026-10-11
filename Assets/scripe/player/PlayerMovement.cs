using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D rb2D;
    private float move;
    public float jumpForce = 4;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    // Control para pausar el movimiento durante el golpe
    private bool sePuedeMover = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Si recibió daño, no lee inputs ni permite saltar/correr
        if (!sePuedeMover) return;

        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce); 
            GetComponent<PlayerAudio>()?.PlayJumpSound();        
        }
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        }
    }

    // Función pública para congelar al personaje durante la animación de Hit
    public void BloquearMovimiento(float duracion)
    {
        StartCoroutine(RutinaBloqueo(duracion));
    }

    private IEnumerator RutinaBloqueo(float duracion)
    {
        sePuedeMover = false;
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero; // Frena al personaje de golpe
        }

        yield return new WaitForSeconds(duracion);

        sePuedeMover = true;
    }
}