using System.Collections;
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
    [SerializeField] private List<GameObject> corazones;
    [SerializeField] private Sprite corazonQuitado, corazonActivado;
    [SerializeField] private List<GameObject> orbes;
    [SerializeField] private Sprite orbeVerde, orbeAzul, orbeAmarillo, orbeRojo;
    [SerializeField] private GameObject cajaTextos;
    [SerializeField] private GameObject panelOrbes;
    [SerializeField] private TMP_Text textoDialogo;
    [SerializeField] private Button BtnEspada;
    [SerializeField] private Button BtnBotas;
    [SerializeField] private TMP_Text textoBtnEspada;
    [SerializeField] private TMP_Text textoBtnBotas;

    [SerializeField] private GameObject panelEquipo;

    [SerializeField] public GameObject imagenVelocidad;
    [SerializeField] public GameObject imagenDa�o;
    private void Start()
    {
        Dinero.sumaMoneda += SumarMonedas;
        ActualizarTextoMonedas();
    }

    // M�todo que se llama cuando se recogen monedas, actualiza el total y el texto en pantalla
    private void SumarMonedas(int monedas)
    {
        totalMonedas += monedas;
        textoMonedas.text = totalMonedas.ToString();

        if (panelOrbes != null)
        {
            panelOrbes.SetActive(false);
        }
    }

    // M�todo para mostrar/ocultar el panel de orbes
    public void TogglePanelOrbes()
    {
        if (panelOrbes != null)
        {
            panelOrbes.SetActive(!panelOrbes.activeSelf);
        }
    }

    public void activaOrbe(int indice)
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
            imgOrbe.color = new Color(1f, 1f, 1f, 1f); // Quitar la opacidad
        }
    }


    // Activa un coraz�n en la interfaz seg�n el �ndice proporcionado
    public void SumaCorazones(int indice)
    {
        Image imgCorazon = corazones[indice].GetComponent<Image>();
        imgCorazon.sprite = corazonActivado;
    }

    private void ActualizarTextoMonedas()
    {
        textoMonedas.text = totalMonedas.ToString();
    }

    // Desactiva un coraz�n en la interfaz seg�n el �ndice proporcionado
    public void RestaCorazones(int indice)
    {
        Image imgCorazon = corazones[indice].GetComponent<Image>();
        imgCorazon.sprite = corazonQuitado;
    }

    // Activa o desactiva la caja de texto de di�logo
    public void EstadoCajaTexto(bool activado)
    {
        cajaTextos.SetActive(activado);
    }

    // Muestra un texto en la caja de di�logo
    public void MostrarTextos(string texto)
    {
        textoDialogo.text = texto.ToString();
    }

    public void MostrarImagenFuerza()
    {
        if (imagenDa�o != null) imagenDa�o.SetActive(true);
    }

    public void MostrarImagenVelocidad()
    {
        if (imagenVelocidad != null) imagenVelocidad.SetActive(true);
    }

    #region TIENDA

    // Asigna el precio del objeto seg�n su nombre
    public void PrecioObjeto(string objeto)
    {
        switch (objeto)
        {
            case "BtnPocion":
                precioObjeto = 1;
                break;
            case "BtnBotas":
                precioObjeto = 1;
                break;
            case "BtnEspada":
                precioObjeto = 1;
                break;
        }
    }

    // M�todo para comprar un objeto si se tienen suficientes monedas y espacio en el inventario
    public void comprarObjeto(string objeto)
    {
        PrecioObjeto(objeto);
        bool espadaComprada = false;
        bool botasCompradas = false;

        if (precioObjeto <= totalMonedas && totalObjetos < 3)
        {
            if (objeto == "BtnEspada" && !espadaComprada)
            {
                espadaComprada = true;
                BtnEspada.interactable = false;
                textoBtnEspada.text = "Comprado";
                AgregarAlPanelEquipo(objeto);
            }
            else if (objeto == "BtnBotas" && !botasCompradas)
            {
                botasCompradas = true;
                BtnBotas.interactable = false;
                textoBtnBotas.text = "Comprado";
                AgregarAlPanelEquipo(objeto);
            }
            else if (objeto != "BtnEspada" && objeto != "BtnBotas")
            {
                AgregarAlPanelEquipo(objeto);
            }
        }
    }

    private void AgregarAlPanelEquipo(string objeto)
    {
        totalObjetos++;
        totalMonedas -= precioObjeto;
        textoMonedas.text = totalMonedas.ToString();

        // Carga el objeto desde los recursos y lo instancia en el panel de equipo
        GameObject equipo = (GameObject)Resources.Load(objeto);
        Instantiate(equipo, Vector3.zero, Quaternion.identity, panelEquipo.transform);
    }

    #endregion
}