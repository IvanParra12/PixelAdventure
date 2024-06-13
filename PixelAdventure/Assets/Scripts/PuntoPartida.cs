using UnityEngine;
using UnityEngine.SceneManagement;

public class PuntoPartida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string escenaActual = SceneManager.GetActiveScene().name;
            if (ControlDatos.Instance.OrbeRecogido(escenaActual) || escenaActual == "Inicio")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}