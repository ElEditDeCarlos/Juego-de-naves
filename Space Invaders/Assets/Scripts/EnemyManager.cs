using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Prefab del enemigo que se clonará
    public GameObject enemyPrefab;

    // Array de posiciones donde pueden aparecer los enemigos
    public Transform[] posRotEnemy;

    // Intervalo de tiempo entre la generación de enemigos
    public float timeBetweenEnemies = 5.0f;

    void Start()
    {
        // Invocar el método CreateEnemies repetidamente con un intervalo de tiempo
        InvokeRepeating("CreateEnemies", 1.0f, timeBetweenEnemies);
    }

    private void CreateEnemies()
    {
        // Elegir una posición aleatoria del array
        int n = Random.Range(0, posRotEnemy.Length);

        // Instanciar el enemigo en la posición y rotación seleccionadas
        Instantiate(enemyPrefab, posRotEnemy[n].position, posRotEnemy[n].rotation);
    }
}
