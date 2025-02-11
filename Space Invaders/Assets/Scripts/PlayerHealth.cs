using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Salud máxima y actual del jugador
    public float maxHealth = 100f;
    private float currentHealth;

    // Referencia a la barra de salud
    public Image lifeBar;

    // Daño causado por una bala enemiga
    public float damageBullet = 20f;

    // Efectos de partículas
    public ParticleSystem smallExplosion;
    public ParticleSystem bigExplosion;

    public GameManager gameManager;

    void Awake()
    {
        // Inicializar la salud del jugador
        currentHealth = maxHealth;

        // Inicializar la barra de salud
        if (lifeBar != null)
        {
            lifeBar.fillAmount = 1f; // Barra de salud al máximo
        }

        gameManager = FindObjectOfType<GameManager>();

        // Detener los efectos de partículas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión es con una bala enemiga
        if (other.CompareTag("BulletEnemy"))
        {
            // Reproducir el efecto de partículas de daño
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del jugador
            currentHealth -= damageBullet;

            // Actualizar la barra de salud
            if (lifeBar != null)
            {
                lifeBar.fillAmount = currentHealth / maxHealth;
            }

            // Destruir la bala que impactó
            Destroy(other.gameObject);

            // Verificar si el jugador ha muerto
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        // Reproducir el efecto de partículas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Desactivar la jerarquía de la cámara antes de destruir el jugador
        Camera.main.transform.SetParent(null);

        // Llamar al método GameOver del GameManager
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        // Destruir al jugador después de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de partículas
    }
}