using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private bool ejecutarCierre = false;

    public void CambioEscenaIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Cerrar()
    {
        ejecutarCierre = true;
    }

    public void Update()
    {
        if (ejecutarCierre == true)
        {
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
