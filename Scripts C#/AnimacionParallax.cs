using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionParallax : MonoBehaviour
{
    private float longitud;
    private float posInicial;

    public GameObject camara;
    public float desfaseParallax;


    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position.x;
        longitud = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanciaAlBorde = camara.transform.position.x * (1 - desfaseParallax);
        float distanciaMovimiento = camara.transform.position.x * (desfaseParallax);
        transform.position = new Vector3(posInicial + distanciaMovimiento,
                                         transform.position.y,
                                         transform.position.z);

        if (distanciaAlBorde > posInicial + longitud) {
            posInicial += longitud;
        }
        else if (distanciaAlBorde < posInicial - longitud) 
        {
            posInicial -= longitud;
        }

    }
}
