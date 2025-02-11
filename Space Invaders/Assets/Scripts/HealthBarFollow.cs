using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    void Update()
    {
        // Hacer que el Canvas siempre mire hacia la c�mara
        transform.LookAt(Camera.main.transform.position);

        // Corregir la rotaci�n para que la barra de salud no est� invertida
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
    }
}