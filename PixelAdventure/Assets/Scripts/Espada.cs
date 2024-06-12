using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    private BoxCollider2D colEspada;
    [SerializeField] Personaje personaje;
 

    private void Awake()
    {
        colEspada = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Orco"))
        {
            Orco orco = other.GetComponent<Orco>();
            if (orco != null)
            {
                for (int i = 1; i <= personaje.fuerza; i++)
                {
                    orco.Herida();
                }
            }
        } else if (other.CompareTag("EnemigoBosque"))
        {
            EnemigoBosque enemigoBosque = other.GetComponent<EnemigoBosque>();
            if (enemigoBosque != null)
            {
                for (int i = 1; i <= personaje.fuerza; i++)
                {
                    enemigoBosque.Herida();
                }
            }
        }
        else if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                for (int i = 1; i <= personaje.fuerza; i++)
                {
                    boss.Herida();
                }
            }
        }
    }
}