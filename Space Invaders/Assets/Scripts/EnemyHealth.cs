using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // Salud m�xima y actual del enemigo
    public float maxHealth = 100f;
    private float currentHealth;

    // Referencia al relleno de la barra de salud
    public Image lifeBar;

    // Da�o causado por el jugador
    public float damageBullet = 20f;

    // Efectos de part�culas
    public ParticleSystem smallExplosion;
    public ParticleSystem bigExplosion;

    void Awake()
    {
        // Inicializar la salud del enemigo
        currentHealth = maxHealth;

        // Inicializar la barra de salud
        if (lifeBar != null)
        {
            lifeBar.fillAmount = 1f; // Barra de salud al m�ximo
        }

        // Detener los efectos de part�culas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisi�n es con una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reproducir el efecto de part�culas de da�o
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del enemigo
            currentHealth -= damageBullet;

            // Actualizar la barra de salud
            if (lifeBar != null)
            {
                lifeBar.fillAmount = currentHealth / maxHealth;
            }

            // Destruir la bala que impact�
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
        // Reproducir el efecto de part�culas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Destruir al enemigo despu�s de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de part�culas
    }
}