using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Referencia al panel de Game Over
    public GameObject panelGameOver;

    // Referencia al script que gestiona la generación de enemigos
    public EnemyManager enemyManager;

    void Start()
    {
        // Asegurarse de que el panel de Game Over esté desactivado al inicio
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    // Método público para activar la pantalla de Game Over
    public void GameOver()
    {
        // Activar el panel de Game Over
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // Desactivar la generación de enemigos
        if (enemyManager != null)
        {
            enemyManager.enabled = false;
        }

        // Desbloquear el cursor para interactuar con la interfaz gráfica
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Método público para reiniciar la escena
    public void LoadSceneLevel()
    {
        // Cargar la escena nuevamente
        SceneManager.LoadScene("Level01");
    }
}