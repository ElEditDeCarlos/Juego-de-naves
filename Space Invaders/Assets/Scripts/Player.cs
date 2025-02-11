using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con la interfaz gráfica (Image)

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento
    public int speed = 50;
    // Velocidad de giro
    public int turnSpeed = 125;
    // Prefab de la bala
    public GameObject bulletPrefab;
    // Array de posiciones y rotaciones para los cañones
    public Transform[] posRotBullet;
    // Componente AudioSource para reproducir el sonido de disparo
    private AudioSource shootAudio;

    // Salud del jugador
    public float maxHealth = 100f;
    private float currentHealth;

    // Barra de salud (HUD)
    public Image lifeBar; // Referencia al relleno de la barra de salud

    // Efectos de partículas
    public ParticleSystem smallExplosion; // Para recibir daño
    public ParticleSystem bigExplosion;  // Para la muerte

    // Daño causado por una bala enemiga
    public float damageBullet = 20f;

    void Awake()
    {
        // Inicializar el componente AudioSource
        shootAudio = GetComponent<AudioSource>();

        // Inicializar la salud del jugador
        currentHealth = maxHealth;
        if (lifeBar != null)
        {
            lifeBar.fillAmount = 1; // Barra de salud al máximo
        }

        // Detener los efectos de partículas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    void Update()
    {
        // Llamar a los métodos de movimiento, giro y ataque en cada fotograma
        Movement();
        Turning();
        Attack(); // Llamar al método de ataque
    }

    // Método para el movimiento de traslación
    private void Movement()
    {
        // Obtener la entrada del jugador (teclas W, A, S, D)
        float horizontal = Input.GetAxis("Horizontal"); // A/D para izquierda/derecha (eje Z)
        float vertical = Input.GetAxis("Vertical");     // W/S para adelante/atrás (eje X)

        // Invertir el signo de la entrada horizontal para corregir A y D
        horizontal = -horizontal;

        // Calcular la dirección del movimiento
        Vector3 direction = new Vector3(vertical, 0, horizontal); // Cambiamos el orden de los ejes

        // Mover al jugador usando Translate
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    // Método para la rotación basada en el ratón
    private void Turning()
    {
        // Obtener la entrada del ratón
        float xMouse = Input.GetAxis("Mouse X"); // Movimiento horizontal del ratón
        float yMouse = Input.GetAxis("Mouse Y"); // Movimiento vertical del ratón

        // Calcular la rotación basada en el movimiento del ratón
        Vector3 rotation = new Vector3(-yMouse, xMouse, 0);

        // Aplicar la rotación al jugador
        transform.Rotate(rotation.normalized * turnSpeed * Time.deltaTime);
    }

    // Método para el ataque (disparo de balas)
    private void Attack()
    {
        // Verificar si el jugador ha hecho clic izquierdo
        if (Input.GetMouseButtonDown(0))
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
    }

    // Método para detectar colisiones
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisión es con una bala enemiga
        if (other.CompareTag("BulletEnemy"))
        {
            // Reproducir el efecto de partículas de daño
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del jugador
            currentHealth -= damageBullet;

            // Actualizar la barra de salud (si existe)
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

    // Método para manejar la muerte del jugador
    private void Death()
    {
        // Reproducir el efecto de partículas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Desactivar la jerarquía de la cámara antes de destruir el jugador
        Camera.main.transform.SetParent(null);

        // Destruir al jugador después de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de partículas
    }
}