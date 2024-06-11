using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTextos : MonoBehaviour
{
    [SerializeField, TextArea(1, 10)] private string[] arrayTextos;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Personaje personaje;

    private int indice;

    private void Awake()
    {
        personaje = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
        uiManager = FindObjectOfType<UIManager>(); // Asegurarse de que UIManager se encuentra
    }

    private void OnMouseDown()
    {
        float distancia = Vector2.Distance(this.gameObject.transform.position, personaje.transform.position);
        if (distancia < 2)
        {
            uiManager.EstadoCajaTexto(true);
            personaje.EstadoConversacion(true);
            ActivaDialogo();
        }
    }

    public void ActivaDialogo()
    {
        if (indice < arrayTextos.Length)
        {
            uiManager.MostrarTextos(arrayTextos[indice]);
            indice++;
        }
        else
        {
            uiManager.EstadoCajaTexto(false);
            personaje.EstadoConversacion(false);
            indice = 0; // Resetear el índice para que el diálogo pueda repetirse si se necesita
        }
    }
}
