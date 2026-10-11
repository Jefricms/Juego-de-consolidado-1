using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    private int coin;
    public TMP_Text textoCoins;

    public int vidas = 3;
    public TMP_Text textoVidas;

    private Animator animator;
    private Vector3 puntoDeInicio; // Guarda automáticamente la posición inicial del nivel

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarNuevaEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarNuevaEscena;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        puntoDeInicio = transform.position; // Guarda la posición de arranque
        BuscarReferenciasUI();
        ActualizarTextosUI();
    }

    private void AlCargarNuevaEscena(Scene scene, LoadSceneMode mode)
    {
        animator = GetComponent<Animator>();

        if (vidas <= 0)
        {
            vidas = 3;
        }

        // Si en la nueva escena había un personaje puesto de referencia, toma su posición
        GameObject otroJugador = GameObject.Find("jugador");
        if (otroJugador == null) otroJugador = GameObject.Find("Idle_0");

        if (otroJugador != null && otroJugador != this.gameObject)
        {
            puntoDeInicio = otroJugador.transform.position;
            Destroy(otroJugador); // Elimina el clon de la nueva escena
        }

        ReubicarAlInicio();
        BuscarReferenciasUI();
        ActualizarTextosUI();
    }

    // Regresa al personaje a la posición inicial guardada
    public void ReubicarAlInicio()
    {
        transform.position = puntoDeInicio;

        // Frena la caída o inercia de inmediato
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void BuscarReferenciasUI()
    {
        // Si ya lo arrastraste manualmente en el Inspector, no lo borres
        if (textoCoins != null && textoVidas != null) return;

        TMP_Text[] todosLosTextos = Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include);

        foreach (TMP_Text t in todosLosTextos)
        {
            if (t.GetComponentInParent<UnityEngine.UI.Button>() != null)
                continue;

            Transform padre = t.transform;
            bool descartar = false;
            while (padre != null)
            {
                string nombrePadre = padre.name.ToLower();
                if (nombrePadre.Contains("gameover") || nombrePadre.Contains("fin") || nombrePadre.Contains("pausa"))
                {
                    descartar = true;
                    break;
                }
                padre = padre.parent;
            }
            if (descartar) continue;

            string nombre = t.gameObject.name.ToLower();

            if (textoVidas == null && (nombre == "vidas" || nombre == "textovidas" || nombre.Contains("life")))
            {
                textoVidas = t;
            }

            if (textoCoins == null && (nombre.Contains("coin") || nombre.Contains("moneda") || nombre == "text (tmp)"))
            {
                textoCoins = t;
            }
        }
    }

    public void SumarMoneda()
    {
        coin++;
        ActualizarTextosUI();
    }

    [Header("Efecto de Daño")]
    [SerializeField] private float duracionHit = 0.5f; // Ajusta aquí los segundos que dura el Hit (ej. 0.5s a 0.8s)

    public void PerderVida()
    {
        StartCoroutine(RutinaPerderVida());
    }

    private System.Collections.IEnumerator RutinaPerderVida()
    {
        vidas--;
        ActualizarTextoVidas();
        GetComponent<PlayerAudio>()?.PlayHitSound();

        // 1. Bloquear controles y congelar físicas durante el impacto
        GetComponent<PlayerMovement>()?.BloquearMovimiento(duracionHit);

        // 2. Disparar la animación de daño
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // 3. Esperar a que la animación de Hit se reproduzca en pantalla
        yield return new WaitForSeconds(duracionHit);

        // 4. Evaluar si murió o solo respawnea
        if (vidas <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ActivarGameOver();
            }
        }
        else
        {
            // Tras verse el golpe completo, regresa al punto de inicio
            ReubicarAlInicio();
        }
    }

    public void ActualizarTextosUI()
    {
        ActualizarTextoVidas();

        if (textoCoins != null)
        {
            textoCoins.text = coin.ToString("D2");
        }
    }

    public void ActualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = vidas.ToString();
        }
    }

    public void ReiniciarEstadisticas()
    {
        vidas = 3;
        coin = 0;
        ActualizarTextosUI();
        ReubicarAlInicio();
    }
}