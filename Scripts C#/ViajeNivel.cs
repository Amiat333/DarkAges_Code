using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViajeNivel : MonoBehaviour
{
    public GameObject interactionUI;

    private Collider2D playerCollider; // Variable para almacenar el colisionador del jugador

    public void CambioEscenaIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mostrar interfaz de interacción
            interactionUI.SetActive(true);
            playerCollider = other; // Almacena el colisionador del jugador
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ocultar interfaz de interacción
            interactionUI.SetActive(false);
            playerCollider = null; // Restablece la variable del colisionador del jugador
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollider != null && Input.GetKeyDown(KeyCode.E))
        {
            CambioEscenaIndex(4);
        }
    }
}
