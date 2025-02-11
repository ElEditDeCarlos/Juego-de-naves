using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Prefab del enemigo que se clonar�
    public GameObject enemyPrefab;

    // Array de posiciones donde pueden aparecer los enemigos
    public Transform[] posRotEnemy;

    // Intervalo de tiempo entre la generaci�n de enemigos
    public float timeBetweenEnemies = 5.0f;

    void Start()
    {
        // Invocar el m�todo CreateEnemies repetidamente con un intervalo de tiempo
        InvokeRepeating("CreateEnemies", 1.0f, timeBetweenEnemies);
    }

    private void CreateEnemies()
    {
        // Elegir una posici�n aleatoria del array
        int n = Random.Range(0, posRotEnemy.Length);

        // Instanciar el enemigo en la posici�n y rotaci�n seleccionadas
        Instantiate(enemyPrefab, posRotEnemy[n].position, posRotEnemy[n].rotation);
    }
}
