using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PalancaAccion : MonoBehaviour
{
    public UnityEvent eventoE;

    public bool jugadorCerca = false;
    private bool esperandoAccion = false;

    public GameObject panelInfo;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (esperandoAccion && collision.CompareTag("Player"))
        {
            if (eventoE != null)
            {
                eventoE.Invoke();
            }
            esperandoAccion = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (jugadorCerca)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                esperandoAccion = true;
            }

            if (panelInfo)
            {
                panelInfo.SetActive(true);
            }
        }
        else
        {
            if (panelInfo)
            {
                panelInfo.SetActive(false);
            }
        }
    }
}
