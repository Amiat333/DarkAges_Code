using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoBaseNivel02 : MonoBehaviour
{
    public GameObject objetoDestino;

    public float separicionY = 1.0f;


    // Start is called before the first frame update
    //void Start() { }

    // Update is called once per frame
    void Update()
    {
        
            Vector3 posicion2D = new Vector3(objetoDestino.transform.position.x,
                                         objetoDestino.transform.position.y + separicionY,
                                         transform.position.z);

            transform.position = posicion2D; 
    }
}
