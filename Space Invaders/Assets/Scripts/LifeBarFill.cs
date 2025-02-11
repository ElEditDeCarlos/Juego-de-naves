using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con la interfaz gr�fica (Image)

public class BarraDeVida : MonoBehaviour
{
    // Referencia al relleno de la barra de vida (Image)
    public Image LifeBarFill;

    // Salud m�xima del jugador
    public float maxHealth = 100f;

    // Salud actual del jugador
    private float currentHealth;

    void Start()
    {
        // Inicializar la salud al m�ximo
        currentHealth = maxHealth;

        // Asegurarse de que la barra de vida est� al m�ximo al inicio
        if (LifeBarFill != null)
        {
            LifeBarFill.fillAmount = 1f; // 1 = 100% de la barra llena
        }
    }

    // M�todo para reducir la salud del jugador
    public void ReducirSalud(float cantidad)
    {
        // Reducir la salud actual
        currentHealth -= cantidad;

        // Asegurarse de que la salud no sea menor que 0 ni mayor que maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Actualizar la barra de vida visualmente
        if (LifeBarFill != null)
        {
            LifeBarFill.fillAmount = currentHealth / maxHealth;
        }

        // Verificar si la salud llega a 0
        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    // M�todo para manejar la muerte del jugador
    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        // Aqu� puedes agregar l�gica adicional, como mostrar una pantalla de Game Over.
    }
}