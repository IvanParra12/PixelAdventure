using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform personaje;
    public GameObject puntosRutaPadre;
    private List<Vector3> puntosRutaGlobales = new List<Vector3>();

    private float distancia;
    private int indiceRuta;
    private NavMeshAgent agente;
    private bool objetivoDetectado;
    private Transform objetivo;
    private Animator anim;

    private bool puedeAtacar = true;
    [SerializeField] private float tiempoEntreAtaques = 1f;
    [SerializeField] private GameObject orbeRojo;

    public int vida = 1;
    public int fuerza = 1;

    // List of sprite renderers for different body parts
    public List<SpriteRenderer> bodyParts;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agente.updateRotation = false;
        agente.updateUpAxis = false;

        // Inicializar objetivo como un nuevo GameObject temporalmente
        objetivo = new GameObject("Objetivo").transform;

        // Obtener puntos de ruta de los hijos del objeto puntosRutaPadre
        foreach (Transform punto in puntosRutaPadre.transform)
        {
            puntosRutaGlobales.Add(punto.position);
        }
    }

    private void Update()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        float distancia = Vector3.Distance(personaje.position, this.transform.position);

        if (Vector3.Distance(this.transform.position, puntosRutaGlobales[indiceRuta]) < 0.1f)
        {
            if (indiceRuta < puntosRutaGlobales.Count - 1)
            {
                indiceRuta++;
            }
            else if (indiceRuta == puntosRutaGlobales.Count - 1)
            {
                indiceRuta = 0;
            }
        }

        if (distancia < 3)
        {
            objetivoDetectado = true;
        }

        MovimientoEnemigo(objetivoDetectado);
        RotarBoss();

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
            Muerte();
        }
    }

    private void Muerte()
    {
        orbeRojo.SetActive(true);
        Destroy(this.gameObject);
    }

    void MovimientoEnemigo(bool esDetectado)
    {
        if (esDetectado)
        {
            agente.SetDestination(personaje.position);
            objetivo.position = personaje.position;
        }
        else
        {
            agente.SetDestination(puntosRutaGlobales[indiceRuta]);
            objetivo.position = puntosRutaGlobales[indiceRuta];
        }
    }

    void RotarBoss()
    {
        bool mirandoALaIzquierda = this.transform.position.x > objetivo.position.x;

        foreach (var part in bodyParts)
        {
            part.flipX = mirandoALaIzquierda;
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

        for (int i = 1; i <= fuerza; i++)
        {
            personaje.CausarHerida();
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeAtacar = true;
    }
}

