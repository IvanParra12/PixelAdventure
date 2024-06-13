using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int totalMonedas;
    public int totalObjetos;
    private int precioObjeto;

    [SerializeField] private TMP_Text textoMonedas;
    [SerializeField] public List<GameObject> corazones;
    [SerializeField] public Sprite corazonQuitado, corazonActivado;
    [SerializeField] public List<GameObject> orbes;
    [SerializeField] public Sprite orbeVerde, orbeAzul, orbeAmarillo, orbeRojo;
    [SerializeField] private GameObject cajaTextos;
    [SerializeField] private GameObject panelOrbes;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] public Button BtnEspada;
    [SerializeField] public Button BtnBotas;
    [SerializeField] private Button BtnPocion;
    [SerializeField] private TMP_Text textoBtnEspada;
    [SerializeField] private TMP_Text textoBtnBotas;
    [SerializeField] private TMP_Text textoBtnPocion;
    [SerializeField] public GameObject panelEquipo;
    [SerializeField] private GameObject panelTienda;
    [SerializeField] public GameObject imagenVelocidad;
    [SerializeField] public GameObject imagenDaño;
    private void Start()
    {
        Dinero.sumaMoneda += SumarMonedas;
        CargarEstadoInicial();
        ControlDatos.Instance.CargarEstadoUI(this);
        panelTienda.SetActive(false);  // Asegúrate de que el panel esté desactivado al inicio
    }


    private void OnDestroy()
    {
        ControlDatos.Instance.GuardarEstadoUI(this);
        Dinero.sumaMoneda -= SumarMonedas;
    }

    private void CargarEstadoInicial()
    {
        totalMonedas = ControlDatos.Instance.totalMonedas;  // Asegurarse de cargar el estado de las monedas al iniciar
        ActualizarTextoMonedas(totalMonedas);
    }

    public void CargarEstadoMonedas()
    {
        // Asegúrate de que el ControlDatos ya está inicializado cuando llamas esto
        if (ControlDatos.Instance != null)
        {
            totalMonedas = ControlDatos.Instance.totalMonedas;
            ActualizarTextoMonedas(totalMonedas);
        }
    }


    private void SumarMonedas(int monedas)
    {
        totalMonedas += monedas;
        ActualizarTextoMonedas(totalMonedas);
        ControlDatos.Instance.totalMonedas = totalMonedas;
    }

    public void TogglePanelOrbes()
    {
        if (panelOrbes != null)
        {
            panelOrbes.SetActive(!panelOrbes.activeSelf);
        }
    }

    public void ActivarOrbe(int indice)
    {
        if (indice >= 0 && indice < orbes.Count)
        {
            Image imgOrbe = orbes[indice].GetComponent<Image>();
            switch (indice)
            {
                case 0:
                    imgOrbe.sprite = orbeVerde;
                    break;
                case 1:
                    imgOrbe.sprite = orbeAzul;
                    break;
                case 2:
                    imgOrbe.sprite = orbeAmarillo;
                    break;
                case 3:
                    imgOrbe.sprite = orbeRojo;
                    break;
            }
            imgOrbe.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void DesactivarOrbe(int indice)
    {
        if (indice >= 0 && indice < orbes.Count)
        {
            Image imgOrbe = orbes[indice].GetComponent<Image>();
            imgOrbe.sprite = null;
            imgOrbe.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public void SumaCorazones(int indice)
    {
        if (indice < corazones.Count)
        {
            Image imgCorazon = corazones[indice].GetComponent<Image>();
            imgCorazon.sprite = corazonActivado;
        }
    }

    public void RestaCorazones(int indice)
    {
        if (indice < corazones.Count)
        {
            Image imgCorazon = corazones[indice].GetComponent<Image>();
            imgCorazon.sprite = corazonQuitado;
        }
    }

    public void ActualizarTextoMonedas(int monedas)
    {
        textoMonedas.text = monedas.ToString();
    }

    public void EstadoCajaTexto(bool activado)
    {
        cajaTextos.SetActive(activado);
    }

    public void MostrarTextos(string texto)
    {
        textoDialogo.text = texto;
    }

    public void MostrarImagenFuerza()
    {
        if (imagenDaño != null)
        {
            imagenDaño.SetActive(true);
        }
    }

    public void MostrarImagenVelocidad()
    {
        if (imagenVelocidad != null)
        {
            imagenVelocidad.SetActive(true);
        }
    }

    public void PrecioObjeto(string objeto)
    {
        switch (objeto)
        {
            case "BtnPocion": precioObjeto = 1; break;
            case "BtnBotas": precioObjeto = 10; break;
            case "BtnEspada": precioObjeto = 15; break;
        }
    }

    public void comprarObjeto(string objeto)
    {
        Debug.Log($"Intentando comprar {objeto}, Monedas disponibles: {totalMonedas}, Precio: {precioObjeto}");
        PrecioObjeto(objeto);
        if (totalMonedas >= precioObjeto && totalObjetos < 3)
        {
            if (objeto == "BtnEspada" && BtnEspada.interactable)
            {
                totalMonedas -= precioObjeto;
                ActualizarTextoMonedas(totalMonedas);
                AgregarAlPanelEquipo(objeto);
                BtnEspada.interactable = false;
                textoBtnEspada.text = "Comprado";
            }
            else if (objeto == "BtnBotas" && BtnBotas.interactable)
            {
                totalMonedas -= precioObjeto;
                ActualizarTextoMonedas(totalMonedas);
                AgregarAlPanelEquipo(objeto);
                BtnBotas.interactable = false;
                textoBtnBotas.text = "Comprado";
            }
            else if (objeto == "BtnPocion")
            {
                if (totalObjetos < 3)
                {
                    totalMonedas -= precioObjeto;
                    ActualizarTextoMonedas(totalMonedas);
                    AgregarAlPanelEquipo(objeto);
                }
            }
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o has alcanzado el límite de objetos.");
        }
    }

    public void CerrarPanelTienda()
    {
        if (!panelTienda.activeSelf) return;
        float distancia = Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position);
        if (distancia > 5) // Ejemplo: cerrar si el jugador se aleja más de 5 unidades
        {
            panelTienda.SetActive(false);
        }
    }

    public void AgregarAlPanelEquipo(string objeto)
    {
        GameObject equipo = (GameObject)Resources.Load(objeto);
        if (equipo != null && totalObjetos < 3)
        {
            GameObject nuevoEquipo = Instantiate(equipo, Vector3.zero, Quaternion.identity, panelEquipo.transform);
            nuevoEquipo.name = equipo.name; // Quita el "(Clone)" para manejo uniforme
            ControlDatos.Instance.objetosEnPanel.Add(nuevoEquipo.name);
            totalObjetos++;
        }
    }
}
