using UnityEngine;
using UnityEngine.UI; // Importante para usar la clase Image

public class Enemy : MonoBehaviour
{
    // Velocidad de persecuci�n
    public int speed = 12;
    // Distancia m�nima para detenerse
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

    // Efectos de part�culas
    public ParticleSystem smallExplosion; // Para recibir da�o
    public ParticleSystem bigExplosion;  // Para la muerte

    // Barra de salud
    public Image BarraDeVida; // Renombrado como BarraDeVida

    void Awake()
    {
        // Inicializar el componente AudioSource
        shootAudio = GetComponent<AudioSource>();
        // Buscar al jugador por su etiqueta
        player = GameObject.FindGameObjectWithTag("Player");
        // Llamar al m�todo de ataque de forma repetida
        InvokeRepeating("Attack", 1f, timeBetweenBullets);

        // Inicializar la salud del enemigo
        currentHealth = maxHealth;
        if (BarraDeVida != null)
        {
            BarraDeVida.fillAmount = 1; // Barra de salud al m�ximo
        }

        // Detener los efectos de part�culas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    void Update()
    {
        // Verificar si el jugador existe
        if (player == null)
            return;

        // Mover al enemigo hacia el jugador si est� fuera de la distancia m�nima
        FollowPlayer();
    }

    // M�todo para perseguir al jugador sin rotar
    private void FollowPlayer()
    {
        // Calcular la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // Mover al enemigo si la distancia es mayor que la distancia m�nima
        if (distance > distanceToPlayer)
        {
            // Calcular la direcci�n hacia el jugador
            Vector3 direction = (player.transform.position - transform.position).normalized;
            // Mover al enemigo en la direcci�n del jugador sin rotar
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    // M�todo para el ataque (disparo de balas)
    private void Attack()
    {
        // Recorrer todas las posiciones de disparo
        for (int i = 0; i < posRotBullet.Length; i++)
        {
            // Instanciar una bala en la posici�n y rotaci�n del ca��n correspondiente
            Instantiate(bulletPrefab, posRotBullet[i].position, posRotBullet[i].rotation);
        }
        // Reproducir el efecto de sonido de disparo
        shootAudio.Play();
    }

    // M�todo para detectar colisiones
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisi�n es con una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reproducir el efecto de part�culas de da�o
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del enemigo
            currentHealth -= 20; // Da�o causado por la bala del jugador

            // Actualizar la barra de salud (si existe)
            if (BarraDeVida != null)
            {
                BarraDeVida.fillAmount = currentHealth / maxHealth;
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

    // M�todo para manejar la muerte del enemigo
    private void Death()
    {
        // Reproducir el efecto de part�culas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Destruir al enemigo despu�s de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de part�culas
    }
}