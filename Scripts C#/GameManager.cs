using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor.Animations;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject soundAbrirVentana;
    public GameObject soundCerrarVentana;
    public GameObject ScriptHero;

    private AudioSource abrirVentanaAudioSource;
    private AudioSource cerrarVentanaAudioSource;

    public Button resumeButton;

    public static GameManager Instance;

    private Vector3 respawnPosition;

    public bool usarTeclaEscape = true;

    private bool isPaused = false;

    public void CambioEscenaIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void ejecutarAbrirMenu()
    {
        if (pauseMenu)
        {
            HeroKnight heroScript = ScriptHero.GetComponent<HeroKnight>(); // Obtén la referencia al script
            heroScript.enabled = false; // Deshabilita el script
            pauseMenu.SetActive(true);
            isPaused = true;
            abrirVentanaAudioSource.PlayOneShot(abrirVentanaAudioSource.clip);
        }
    }


    private void ejecutarCerrarMenu()
    {
        if (pauseMenu)
        {
            HeroKnight heroScript = ScriptHero.GetComponent<HeroKnight>(); // Obtén la referencia al script
            heroScript.enabled = true; // Habilita el script
            pauseMenu.SetActive(false);
            isPaused = false;
            cerrarVentanaAudioSource.PlayOneShot(cerrarVentanaAudioSource.clip);
        }
    }

    private void ejecutarCambioMenu()
    {
        if (!isPaused)
        {
            ejecutarAbrirMenu();
        } else
        {
            ejecutarCerrarMenu();
        }
    }

    // Start is called before the first frame update
    void Start() 
    {
        abrirVentanaAudioSource = soundAbrirVentana.GetComponent<AudioSource>();
        cerrarVentanaAudioSource = soundCerrarVentana.GetComponent<AudioSource>();
        resumeButton.onClick.AddListener(ejecutarCerrarMenu);
    }

    // Update is called once per frame
    void Update() 
    {
        if (usarTeclaEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            ejecutarCambioMenu();
        }

        if (isPaused)
        {
            Time.timeScale = 0; // Detener la actualización del juego
        }
        else
        {
            Time.timeScale = 1; // Reanudar la actualización del juego
        }
    }

    //private void Awake()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else
    //        Destroy(gameObject);

    //    DontDestroyOnLoad(gameObject);
    //}

    //public void SetRespawnPosition(Vector3 position)
    //{
    //    respawnPosition = position;
    //}

    //public Vector3 GetRespawnPosition()
    //{
    //    return respawnPosition;
    //}
}
