using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlDatos : MonoBehaviour
{
    public static ControlDatos Instance;

    // Propiedades del personaje
    public int vida;
    public int fuerza;
    public float velocidad;
    public int numOrbes;
    public int totalMonedas;
    public int totalObjetos;

    // Estado de los corazones y orbes
    public List<bool> corazonesActivos = new List<bool>();
    public List<bool> orbesActivos = new List<bool>();

    // Estado del panel de equipo y objetos
    public List<string> objetosPanelEquipo = new List<string>();

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

    // Métodos para actualizar y recuperar la información del juego
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
        totalMonedas = uiManager.totalMonedas;
        totalObjetos = uiManager.totalObjetos;

        corazonesActivos.Clear();
        foreach (var corazon in uiManager.corazones)
        {
            corazonesActivos.Add(corazon.GetComponent<Image>().sprite == uiManager.corazonActivado);
        }

        orbesActivos.Clear();
        foreach (var orbe in uiManager.orbes)
        {
            orbesActivos.Add(orbe.GetComponent<Image>().color.a == 1f);
        }

        objetosPanelEquipo.Clear();
        foreach (Transform child in uiManager.panelEquipo.transform)
        {
            objetosPanelEquipo.Add(child.name.Replace("(Clone)", ""));
        }
    }

    public void CargarEstadoUI(UIManager uiManager)
    {
        uiManager.totalMonedas = totalMonedas;
        uiManager.totalObjetos = totalObjetos;
        uiManager.ActualizarTextoMonedas();

        for (int i = 0; i < uiManager.corazones.Count; i++)
        {
            uiManager.corazones[i].GetComponent<Image>().sprite = corazonesActivos[i] ? uiManager.corazonActivado : uiManager.corazonQuitado;
        }

        for (int i = 0; i < uiManager.orbes.Count; i++)
        {
            if (orbesActivos[i])
            {
                uiManager.ActivarOrbe(i);
            }
        }

        foreach (var objeto in objetosPanelEquipo)
        {
            uiManager.AgregarAlPanelEquipo(objeto);
        }
    }
}
