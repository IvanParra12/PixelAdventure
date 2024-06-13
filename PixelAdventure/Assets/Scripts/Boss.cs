using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemigo
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

        objetivo = new GameObject("Objetivo").transform;

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

    public override void Herida()
    {
        base.Herida();
        if (vida == 0)
        {
            Muerte();
        }
    }

    protected override void Muerte()
    {
        orbeRojo.SetActive(true);
        base.Muerte();
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
