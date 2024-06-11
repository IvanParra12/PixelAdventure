using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinero : MonoBehaviour
{
    public delegate void SumaMoneda(int dinero);
    public static event SumaMoneda sumaMoneda;

    [SerializeField] private int cantidadMonedas;
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            if (sumaMoneda != null)
            {
                SumarMoneda();
                Destroy(this.gameObject);
            }
        }
    }

    private void SumarMoneda()
    {
        sumaMoneda(cantidadMonedas);
    }
}
