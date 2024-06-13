using UnityEngine;

public class Tienda : MonoBehaviour
{
    [SerializeField] private GameObject tienda;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float interactDistance = 2.0f; // distancia máxima de interacción
    private Personaje personaje;

    private void Awake()
    {
        personaje = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
    }

    private void Update()
    {
        // Chequear distancia cada frame y mostrar tienda si es adecuado
        CheckDistanceAndShowStore();
    }

    private void CheckDistanceAndShowStore()
    {
        if (Vector2.Distance(transform.position, personaje.transform.position) <= interactDistance)
        {
            if (uiManager.totalMonedas > 0 && !tienda.activeInHierarchy)
            {
                tienda.SetActive(true);
                personaje.EstadoConversacion(true);
            }
        }
        else
        {
            if (tienda.activeInHierarchy)
            {
                tienda.SetActive(false);
                personaje.EstadoConversacion(false);
            }
        }
    }
}
