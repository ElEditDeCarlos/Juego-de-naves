using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    // Velocidad de la bala
    private int speed = 100;

    void Start()
    {
        // Destruir la bala después de 5 segundos
        Destroy(gameObject, 5);
    }

    void Update()
    {
        // Mover la bala hacia adelante en su eje local (eje Z)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}