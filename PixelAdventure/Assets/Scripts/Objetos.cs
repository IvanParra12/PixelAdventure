using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Objetos;

public class Objetos : MonoBehaviour
{
    public enum ObjetosEquipo
    {
        Salud,
        Velocidad,
        Daño
    };

    [SerializeField] ObjetosEquipo objetosEquipo;

    public void UsarObjeto()
    {
        Personaje personaje = GameObject.FindObjectOfType<Personaje>();
        UIManager uiManager = GameObject.FindObjectOfType<UIManager>();

        switch (objetosEquipo)
        {
            case ObjetosEquipo.Salud:
                if (personaje.vida < 5)
                {
                    personaje.SumaVida();
                    Destroy(this.gameObject);
                }
                break;
            case ObjetosEquipo.Velocidad:
                uiManager.MostrarImagenVelocidad();
                Destroy(this.gameObject);
                break;
            case ObjetosEquipo.Daño:
                uiManager.MostrarImagenFuerza();
                personaje.AumentoFuerza();
                Destroy(this.gameObject);
                break;

        }
        uiManager.totalObjetos--;
    }
}
