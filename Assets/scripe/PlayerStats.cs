using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    private int coins;
    public TMP_Text textoCoins;

    public static int vidas = 3;
    public TMP_Text textoVidas;

    private Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        ActualizarTextoVidas();
    }

    public void SumarMoneda()
    {
        coins++;
        if (textoCoins != null)
            textoCoins.text = coins.ToString();
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarTextoVidas();
        GetComponent<PlayerAudio>()?.PlayHitSound();
        if (vidas > 0)
        {
            if (animator != null)
                animator.SetTrigger("Hit");
            
            Invoke("ReiniciarEscena", 0.4f);
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hit");

            Invoke("GameOver", 0.4f);
        }
    }

    void ActualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = vidas.ToString();
        }
    }

    void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GameOver()
    {
        vidas = 3;
        SceneManager.LoadScene(0); // Vuelve al menú principal
    }
}