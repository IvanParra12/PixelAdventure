using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlDatos : MonoBehaviour
{
    public static ControlDatos Instance;

    public int vida;
    public int fuerza;
    public float velocidad;
    public int numOrbes;
    public List<int> corazonesEstado; // para el estado de los corazones
    public List<int> orbesEstado; // para el estado de los orbes

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarEstadoPersonaje(Personaje personaje)
    {
        vida = personaje.vida;
        fuerza = personaje.fuerza;
        velocidad = personaje.velocidad;
        numOrbes = personaje.numOrbes;
    }

    public void CargarEstadoPersonaje(Personaje personaje)
    {
        personaje.vida = vida;
        personaje.fuerza = fuerza;
        personaje.velocidad = velocidad;
        personaje.numOrbes = numOrbes;
    }

    public void GuardarEstadoUI(UIManager uiManager)
    {
        corazonesEstado = new List<int>();
        foreach (var corazon in uiManager.corazones)
        {
            corazonesEstado.Add(corazon.GetComponent<Image>().sprite == uiManager.corazonActivado ? 1 : 0);
        }

        orbesEstado = new List<int>();
        foreach (var orbe in uiManager.orbes)
        {
            orbesEstado.Add(orbe.GetComponent<Image>().sprite == uiManager.orbeVerde ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeAzul ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeAmarillo ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeRojo ? 1 : 0);
        }
    }

    public void CargarEstadoUI(UIManager uiManager)
    {
        for (int i = 0; i < uiManager.corazones.Count; i++)
        {
            uiManager.corazones[i].GetComponent<Image>().sprite = corazonesEstado[i] == 1 ? uiManager.corazonActivado : uiManager.corazonQuitado;
        }

        for (int i = 0; i < uiManager.orbes.Count; i++)
        {
            if (orbesEstado[i] == 1)
            {
                uiManager.ActivarOrbe(i);
            }
            else
            {
                uiManager.DesactivarOrbe(i);
            }
        }
    }

    public void ReiniciarDatos()
    {
        vida = 5;
        fuerza = 1;
        velocidad = 5;
        numOrbes = 0;
        corazonesEstado = new List<int> { 1, 1, 1, 1, 1 };
        orbesEstado = new List<int> { 0, 0, 0, 0 };
    }
}
