using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBienvenida : MonoBehaviour
{
    public float temporizadorEsperaBienvenida = 0.0f;
    public float tiempoEsperaBienvenida = 10.0f;

    public GameObject pBienvenida;

    public void CambioEscenaIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void Update()
    {
        if (pBienvenida)
        {
            if (pBienvenida.activeSelf)
            {
                temporizadorEsperaBienvenida += Time.deltaTime;
                if (temporizadorEsperaBienvenida > tiempoEsperaBienvenida)
                {
                    CambioEscenaIndex(1);
                }
            }
        }
    }
}
