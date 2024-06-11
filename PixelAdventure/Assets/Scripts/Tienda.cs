using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    [SerializeField] private GameObject tienda;
    [SerializeField] UIManager uiManager;
    [SerializeField] Personaje personaje;

    private ControlTextos controlTextos;

    private void Awake()
    {
        personaje = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
        controlTextos = FindObjectOfType<ControlTextos>();
    }

    private void OnMouseDown()
    {
        float distancia = Vector2.Distance(this.gameObject.transform.position, personaje.transform.position);
        if (distancia < 2)
        {
            if (uiManager.totalMonedas > 0)
            {
                tienda.SetActive(true);
                personaje.EstadoConversacion(true);
            }
            else
            {
                uiManager.EstadoCajaTexto(true);
                personaje.EstadoConversacion(true);
                controlTextos.ActivaDialogo();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (uiManager != null && uiManager.totalMonedas > 0)
            {
                tienda.SetActive(true);
            }
        }
    }
}
