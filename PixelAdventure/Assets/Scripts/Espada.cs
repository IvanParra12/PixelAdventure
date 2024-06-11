using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    private BoxCollider2D colEspada;

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
                orco.Herida();
            }
        } else if (other.CompareTag("EnemigoBosque"))
        {
            EnemigoBosque enemigoBosque = other.GetComponent<EnemigoBosque>();
            if (enemigoBosque != null)
            {
                enemigoBosque.Herida();
            }
        }
    }
}