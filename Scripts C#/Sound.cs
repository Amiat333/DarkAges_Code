using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Sound : MonoBehaviour
{
    public AudioMixer mezcladorAudioGeneral;

    public Slider sliderMusica;
    public Slider sliderEfectos;

    static float volumenMusica = 0.0f;
    static float volumenEfecto = 0.0f;

    public void CambiarVolMusica(float volumen)
    {
        if (mezcladorAudioGeneral)
        {
            mezcladorAudioGeneral.SetFloat("volumenMusica", volumen);
            Sound.volumenMusica = volumen;
        }
    }

    public void CambiarVolEfectos(float volumen)
    {
        if (mezcladorAudioGeneral)
        {
            mezcladorAudioGeneral.SetFloat("volumenEfecto", volumen);
            Sound.volumenEfecto = volumen;
        }
    }

    void Start()
    {
        if (mezcladorAudioGeneral)
        {
            mezcladorAudioGeneral.SetFloat("volumenMusica", Sound.volumenMusica);
            mezcladorAudioGeneral.SetFloat("volumenEfecto", Sound.volumenEfecto);
        }

        if (sliderMusica)
        {
            sliderMusica.value = Sound.volumenMusica;
        }

        if (sliderEfectos)
        {
            sliderEfectos.value = Sound.volumenEfecto;
        }
    }

}
