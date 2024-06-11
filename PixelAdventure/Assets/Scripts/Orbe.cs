using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbe : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Personaje personaje = other.GetComponent<Personaje>();
            if (personaje != null)
            {
                personaje.RecogerOrbe();
                Destroy(this.gameObject);
            }
        }
    }
}

