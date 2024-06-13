using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orco : Enemigo
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

        Transform puntosRutaTransform = transform.Find("PuntosRuta");
        AjustarPuntosRuta(puntosRutaTransform);

        objetivo = new GameObject("Objetivo").transform;
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

        if (distancia < 2)
        {
            objetivoDetectado = true;
        }

        MovimientoOrco(objetivoDetectado);
        RotarOrco();

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
            anim.SetTrigger("Muerte");
            Invoke(nameof(Muerte), 0.6f);
        }
    }

    protected override void Muerte()
    {
        Instantiate(prefabMoneda, transform.position, Quaternion.identity);
        base.Muerte();
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

    private void AjustarPuntosRuta(Transform puntosRutaTransform)
    {
        foreach (Transform punto in puntosRutaTransform)
        {
            if (punto != puntosRutaTransform)
            {
                puntosRutaGlobales.Add(punto.position);
            }
        }
    }
}
