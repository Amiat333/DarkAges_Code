using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursor;

    private void Update()
    {
        // Obtiene la posición del mouse en pantalla.
        Vector3 mousePosition = Input.mousePosition;
        // Convierte la posición del mouse en coordenadas del mundo si es necesario.
        // Si tu cursor ya está en la misma posición que el mouse, esto puede no ser necesario.

        // Actualiza la posición del objeto "Cursor" para que siga al mouse.
        transform.position = mousePosition;

        // Cambia el cursor a la textura personalizada.
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}