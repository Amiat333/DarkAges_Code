using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoBase : MonoBehaviour
{
    public GameObject objetoDestino;

    public float limiteInferior = 0.0f;
    public float limiteSuperior = 1.0f;

    public float separicionY = 1.0f;


    // Start is called before the first frame update
    //void Start() { }

    // Update is called once per frame
    void Update()
    {
        

        if(objetoDestino.transform.position.y > limiteInferior &&
            objetoDestino.transform.position.y < limiteSuperior)
        {
            Vector3 posicion2D = new Vector3(objetoDestino.transform.position.x,
                                         objetoDestino.transform.position.y + separicionY,
                                         transform.position.z);

            transform.position = posicion2D;
        } else if (objetoDestino.transform.position.y > limiteInferior)
        {
            Vector3 posicion2D = new Vector3(objetoDestino.transform.position.x,
                                             transform.position.y,
                                             transform.position.z);

            transform.position = posicion2D;
        }

        
    }
}
