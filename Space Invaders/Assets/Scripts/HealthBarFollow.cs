using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    void Update()
    {
        // Hacer que el Canvas siempre mire hacia la cámara
        transform.LookAt(Camera.main.transform.position);

        // Corregir la rotación para que la barra de salud no esté invertida
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
    }
}