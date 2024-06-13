using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBotones : MonoBehaviour
{
    public void OnBotonJugar()
    {
        ControlDatos.Instance.ReiniciarDatos();
        SceneManager.LoadScene("Inicio");
        ControlDatos.Instance.CrearNuevaPartida();  // Método nuevo para crear una nueva partida
    }

    public void OnBotonCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void OnBotonMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnBotonSalir()
    {
        Application.Quit();
    }

    public void OnGuardarPartida()
    {
        var uiManager = GameObject.FindObjectOfType<UIManager>();
        ControlDatos.Instance.GuardarEstadoCompleto();
    }

    public void OnCargarPartida()
    {
        var uiManager = GameObject.FindObjectOfType<UIManager>();
        ControlDatos.Instance.CargarEstadoCompleto(1, 1, uiManager);
    }
}
