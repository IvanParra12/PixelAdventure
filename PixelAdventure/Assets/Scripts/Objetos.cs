using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                if (!ControlDatos.Instance.mejoraVelocidadActivada) // Solo permitir una vez
                {
                    personaje.AumentoVelocidad();
                    uiManager.MostrarImagenVelocidad();
                    ControlDatos.Instance.velocidad = personaje.velocidad; // Actualizar en ControlDatos
                    ControlDatos.Instance.mejoraVelocidadActivada = true;
                    Destroy(this.gameObject);
                }
                break;
            case ObjetosEquipo.Daño:
                if (!ControlDatos.Instance.mejoraFuerzaActivada) // Solo permitir una vez
                {
                    personaje.AumentoFuerza();
                    uiManager.MostrarImagenFuerza();
                    ControlDatos.Instance.fuerza = personaje.fuerza; // Actualizar en ControlDatos
                    ControlDatos.Instance.mejoraFuerzaActivada = true;
                    Destroy(this.gameObject);
                }
                break;
        }

        uiManager.totalObjetos--;
    }
}
