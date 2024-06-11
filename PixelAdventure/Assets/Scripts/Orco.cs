using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orco : MonoBehaviour
{
    public Transform personaje;
    private List<Vector3> puntosRutaGlobales = new List<Vector3>();

    private int indiceRuta;
    private NavMeshAgent agente;
    private bool objetivoDetectado;
    private SpriteRenderer sprite;
    private Transform objetivo;
    private Animator anim;

    private bool puedeAtacar = true;
    [SerializeField] private float tiempoEntreAtaques = 1f;

    public int vida = 1;

    [SerializeField] private GameObject prefabMoneda;  // Prefab de la moneda

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        agente.updateRotation = false;
        agente.updateUpAxis = false;

        // Obtener los puntos de ruta desde los hijos del objeto "PuntosRuta"
        Transform puntosRutaTransform = transform.Find("PuntosRuta");
        AjustarPuntosRuta(puntosRutaTransform);

        // Inicializar objetivo como un nuevo GameObject temporalmente
        objetivo = new GameObject("Objetivo").transform;
    }

    private void Update()
    {
        // Comprobamos que la posicion del orco siempre sea 0 en la z porque sino da fallo la navegacion
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        float distancia = Vector3.Distance(personaje.position, this.transform.position);

        // Si la posicion de mi orco es la posicion de uno de los puntos de ruta
        if (Vector3.Distance(this.transform.position, puntosRutaGlobales[indiceRuta]) < 0.1f)
        {
            // Se comprueba que el indice es menor que la cantidad de puntos de ruta - 1
            if (indiceRuta < puntosRutaGlobales.Count - 1)
            {
                // Si es menor sumamos 1 al indice y se dirige al siguiente punto
                indiceRuta++;
            }
            // Si es igual, ponemos el indice a 0 y vuelve al principio
            else if (indiceRuta == puntosRutaGlobales.Count - 1)
            {
                indiceRuta = 0;
            }
        }

        if (distancia < 2)
        {
            objetivoDetectado = true;
        }

        MovimientoOrco(objetivoDetectado);
        RotarOrco();

        // Si el objetivo está detectado y puede atacar, realiza el ataque
        if (objetivoDetectado && puedeAtacar)
        {
            StartCoroutine(Atacar(personaje.gameObject));
        }
    }

    public void Herida()
    {
        vida--;
        if (vida == 0)
        {
            anim.SetTrigger("Muerte");
            Invoke(nameof(Muerte), 0.6f);
        }
    }

    private void Muerte()
    {
        // Instanciar la moneda en la posición actual del orco
        Instantiate(prefabMoneda, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void MovimientoOrco(bool esDetectado)
    {
        if (esDetectado)
        {
            agente.SetDestination(personaje.position);
            objetivo.position = personaje.position; // Actualizamos la posición del objetivo
        }
        else
        {
            agente.SetDestination(puntosRutaGlobales[indiceRuta]);
            objetivo.position = puntosRutaGlobales[indiceRuta];
        }
    }

    void RotarOrco()
    {
        if (this.transform.position.x > objetivo.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && puedeAtacar)
        {
            StartCoroutine(Atacar(collision.gameObject));
        }
    }

    private IEnumerator Atacar(GameObject objetivo)
    {
        puedeAtacar = false;
        anim.SetTrigger("Ataca");
        Personaje personaje = objetivo.GetComponent<Personaje>();
        personaje.CausarHerida();

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeAtacar = true;
    }

    // Método para ajustar los puntos de ruta cuando se instancia el prefab
    private void AjustarPuntosRuta(Transform puntosRutaTransform)
    {
        // Obtener los puntos de ruta y convertir las coordenadas globales
        foreach (Transform punto in puntosRutaTransform)
        {
            if (punto != puntosRutaTransform) // Ignorar el propio objeto contenedor
            {
                puntosRutaGlobales.Add(punto.position);
            }
        }
    }
}
