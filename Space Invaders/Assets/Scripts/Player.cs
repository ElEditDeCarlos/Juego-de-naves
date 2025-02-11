using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con la interfaz gr�fica (Image)

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento
    public int speed = 50;
    // Velocidad de giro
    public int turnSpeed = 125;
    // Prefab de la bala
    public GameObject bulletPrefab;
    // Array de posiciones y rotaciones para los ca�ones
    public Transform[] posRotBullet;
    // Componente AudioSource para reproducir el sonido de disparo
    private AudioSource shootAudio;

    // Salud del jugador
    public float maxHealth = 100f;
    private float currentHealth;

    // Barra de salud (HUD)
    public Image lifeBar; // Referencia al relleno de la barra de salud

    // Efectos de part�culas
    public ParticleSystem smallExplosion; // Para recibir da�o
    public ParticleSystem bigExplosion;  // Para la muerte

    // Da�o causado por una bala enemiga
    public float damageBullet = 20f;

    void Awake()
    {
        // Inicializar el componente AudioSource
        shootAudio = GetComponent<AudioSource>();

        // Inicializar la salud del jugador
        currentHealth = maxHealth;
        if (lifeBar != null)
        {
            lifeBar.fillAmount = 1; // Barra de salud al m�ximo
        }

        // Detener los efectos de part�culas iniciales
        if (smallExplosion != null) smallExplosion.Stop();
        if (bigExplosion != null) bigExplosion.Stop();
    }

    void Update()
    {
        // Llamar a los m�todos de movimiento, giro y ataque en cada fotograma
        Movement();
        Turning();
        Attack(); // Llamar al m�todo de ataque
    }

    // M�todo para el movimiento de traslaci�n
    private void Movement()
    {
        // Obtener la entrada del jugador (teclas W, A, S, D)
        float horizontal = Input.GetAxis("Horizontal"); // A/D para izquierda/derecha (eje Z)
        float vertical = Input.GetAxis("Vertical");     // W/S para adelante/atr�s (eje X)

        // Invertir el signo de la entrada horizontal para corregir A y D
        horizontal = -horizontal;

        // Calcular la direcci�n del movimiento
        Vector3 direction = new Vector3(vertical, 0, horizontal); // Cambiamos el orden de los ejes

        // Mover al jugador usando Translate
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    // M�todo para la rotaci�n basada en el rat�n
    private void Turning()
    {
        // Obtener la entrada del rat�n
        float xMouse = Input.GetAxis("Mouse X"); // Movimiento horizontal del rat�n
        float yMouse = Input.GetAxis("Mouse Y"); // Movimiento vertical del rat�n

        // Calcular la rotaci�n basada en el movimiento del rat�n
        Vector3 rotation = new Vector3(-yMouse, xMouse, 0);

        // Aplicar la rotaci�n al jugador
        transform.Rotate(rotation.normalized * turnSpeed * Time.deltaTime);
    }

    // M�todo para el ataque (disparo de balas)
    private void Attack()
    {
        // Verificar si el jugador ha hecho clic izquierdo
        if (Input.GetMouseButtonDown(0))
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
    }

    // M�todo para detectar colisiones
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si la colisi�n es con una bala enemiga
        if (other.CompareTag("BulletEnemy"))
        {
            // Reproducir el efecto de part�culas de da�o
            if (smallExplosion != null) smallExplosion.Play();

            // Reducir la salud del jugador
            currentHealth -= damageBullet;

            // Actualizar la barra de salud (si existe)
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

    // M�todo para manejar la muerte del jugador
    private void Death()
    {
        // Reproducir el efecto de part�culas de muerte
        if (bigExplosion != null) bigExplosion.Play();

        // Desactivar la jerarqu�a de la c�mara antes de destruir el jugador
        Camera.main.transform.SetParent(null);

        // Destruir al jugador despu�s de un breve retraso
        Destroy(gameObject, 1.0f); // 1 segundo para que se vea el efecto de part�culas
    }
}