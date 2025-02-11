using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Salud m�xima y actual del jugador
    public float maxHealth = 100f;
    private float currentHealth;

    // Referencia a la barra de salud
    public Image lifeBar;

    // Da�o causado por una bala enemiga
    public float damageBullet = 20f;

    // Efectos de part�culas
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
            lifeBar.fillAmount = 1f; // Barra de salud al m�ximo
        }

        gameManager = FindObjectOfType<GameManager>();

        // Detener los efectos de part�culas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisi�n es con una bala enemiga
        if (other.CompareTag("BulletEnemy"))
        {
            // Reproducir el efecto de part�culas de da�o
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del jugador
            currentHealth -= damageBullet;

            // Actualizar la barra de salud
            if (lifeBar != null)
            {
                lifeBar.fillAmount = currentHealth / maxHealth;
            }

            // Destruir la bala que impact�
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
        // Reproducir el efecto de part�culas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Desactivar la jerarqu�a de la c�mara antes de destruir el jugador
        Camera.main.transform.SetParent(null);

        // Llamar al m�todo GameOver del GameManager
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        // Destruir al jugador despu�s de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de part�culas
    }
}