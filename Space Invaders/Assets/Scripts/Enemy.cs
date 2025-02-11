using UnityEngine;
using UnityEngine.UI; // Importante para usar la clase Image

public class Enemy : MonoBehaviour
{
    // Velocidad de persecución
    public int speed = 12;
    // Distancia mínima para detenerse
    public float distanceToPlayer = 6f;
    // Prefab de la bala del enemigo
    public GameObject bulletPrefab;
    // Array de posiciones de disparo
    public Transform[] posRotBullet;
    // Tiempo entre disparos
    public float timeBetweenBullets = 2f;
    // Componente AudioSource para el sonido de disparo
    private AudioSource shootAudio;
    // Referencia al jugador
    private GameObject player;

    // Salud del enemigo
    public float maxHealth = 100f;
    private float currentHealth;

    // Efectos de partículas
    public ParticleSystem smallExplosion; // Para recibir daño
    public ParticleSystem bigExplosion;  // Para la muerte

    // Barra de salud
    public Image BarraDeVida; // Renombrado como BarraDeVida

    void Awake()
    {
        // Inicializar el componente AudioSource
        shootAudio = GetComponent<AudioSource>();
        // Buscar al jugador por su etiqueta
        player = GameObject.FindGameObjectWithTag("Player");
        // Llamar al método de ataque de forma repetida
        InvokeRepeating("Attack", 1f, timeBetweenBullets);

        // Inicializar la salud del enemigo
        currentHealth = maxHealth;
        if (BarraDeVida != null)
        {
            BarraDeVida.fillAmount = 1; // Barra de salud al máximo
        }

        // Detener los efectos de partículas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    void Update()
    {
        // Verificar si el jugador existe
        if (player == null)
            return;

        // Mover al enemigo hacia el jugador si está fuera de la distancia mínima
        FollowPlayer();
    }

    // Método para perseguir al jugador sin rotar
    private void FollowPlayer()
    {
        // Calcular la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // Mover al enemigo si la distancia es mayor que la distancia mínima
        if (distance > distanceToPlayer)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.transform.position - transform.position).normalized;
            // Mover al enemigo en la dirección del jugador sin rotar
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    // Método para el ataque (disparo de balas)
    private void Attack()
    {
        // Recorrer todas las posiciones de disparo
        for (int i = 0; i < posRotBullet.Length; i++)
        {
            // Instanciar una bala en la posición y rotación del cañón correspondiente
            Instantiate(bulletPrefab, posRotBullet[i].position, posRotBullet[i].rotation);
        }
        // Reproducir el efecto de sonido de disparo
        shootAudio.Play();
    }

    // Método para detectar colisiones
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión es con una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reproducir el efecto de partículas de daño
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del enemigo
            currentHealth -= 20; // Daño causado por la bala del jugador

            // Actualizar la barra de salud (si existe)
            if (BarraDeVida != null)
            {
                BarraDeVida.fillAmount = currentHealth / maxHealth;
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

    // Método para manejar la muerte del enemigo
    private void Death()
    {
        // Reproducir el efecto de partículas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Destruir al enemigo después de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de partículas
    }
}