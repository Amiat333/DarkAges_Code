using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuenteLevadizo01 : MonoBehaviour
{
    public Animator animadorPuente;

    private bool estaArriba = true;

    public void subirPuente() { 
        if (animadorPuente)
        {
            animadorPuente.SetBool("estaElevado", true);
            estaArriba = true;
        }

    }

    public void bajarPuente()
    {
        if (animadorPuente)
        {
            animadorPuente.SetBool("estaElevado", false);
            estaArriba = false;
        }
    }

    public void cambiarEstadoPuente()
    {
        if (estaArriba)
        {
            bajarPuente();
        }
        else
        {
            subirPuente();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        subirPuente();
    }

    // Update is called once per frame
    //void Update() { }
}
