using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // Salud máxima y actual del enemigo
    public float maxHealth = 100f;
    private float currentHealth;

    // Referencia al relleno de la barra de salud
    public Image lifeBar;

    // Daño causado por el jugador
    public float damageBullet = 20f;

    // Efectos de partículas
    public ParticleSystem smallExplosion;
    public ParticleSystem bigExplosion;

    void Awake()
    {
        // Inicializar la salud del enemigo
        currentHealth = maxHealth;

        // Inicializar la barra de salud
        if (lifeBar != null)
        {
            lifeBar.fillAmount = 1f; // Barra de salud al máximo
        }

        // Detener los efectos de partículas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión es con una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reproducir el efecto de partículas de daño
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del enemigo
            currentHealth -= damageBullet;

            // Actualizar la barra de salud
            if (lifeBar != null)
            {
                lifeBar.fillAmount = currentHealth / maxHealth;
            }

            // Destruir la bala que impactó
            Destroy(other.gameObject);

            // Verificar si el enemigo ha muerto
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

        // Destruir al enemigo después de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de partículas
    }
}