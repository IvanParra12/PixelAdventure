using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoBosque : Enemigo
{
    public Transform personaje;
    public GameObject puntosRutaPadre; // Este objeto contendrá los puntos de ruta como hijos.
    private List<Vector3> puntosRutaGlobales = new List<Vector3>();

    private float distancia;
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

        distancia = Vector3.Distance(personaje.position, this.transform.position);

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
        RotarEnemigo();
        ActualizarAnimacion();

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
            Invoke(nameof(Muerte), 0.5f);
        }
    }

    protected override void Muerte()
    {
        Instantiate(prefabMoneda, transform.position, Quaternion.identity);
        base.Muerte();
    }

    void MovimientoEnemigo(bool esDetectado)
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

    void RotarEnemigo()
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

    void ActualizarAnimacion()
    {
        Vector3 direccionMovimiento = agente.velocity.normalized;
        anim.SetFloat("Movimiento X", direccionMovimiento.x);
        anim.SetFloat("Movimiento Y", direccionMovimiento.y);
        anim.SetBool("Moviendo", direccionMovimiento.sqrMagnitude > 0.1f);
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
        Personaje personaje = objetivo.GetComponent<Personaje>();

        if (distancia < 2)
        {
            for (int i = 1; i <= fuerza; i++)
            {
                personaje.CausarHerida();
            }
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeAtacar = true;
    }
}
