using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;
    public Button menuButton;

    private bool gameOverActivo = false;

    void Awake()
    {
        // Configuración del Singleton
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
        // Asegurarse de que el panel empiece oculto y el tiempo normal
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Asignar funciones a los botones por código
        if (reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(ReiniciarNivel);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(IrAlMenu);
        }
    }

    // Función pública para activar el Game Over
    public void ActivarGameOver()
    {
        if (gameOverActivo) return;

        gameOverActivo = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Pausar el juego
        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // Cambia "Menu" por el nombre exacto de tu escena de menú
    }
}