using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour
{
    [SerializeField] public float velocidad;
    [SerializeField] private BoxCollider2D colEspada;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float offsetX = 1;
    private float offsetY = 0;
    public bool hablando;

    public int vida;
    public int fuerza;
    public int numOrbes; // Cantidad de orbes
    public int cantidadObjetos; // Cantidad de objetos
    private const int orbesNecesarios = 4;
    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        uiManager = FindObjectOfType<UIManager>();

        // Cargar el estado del personaje desde el singleton al iniciar
        vida = ControlDatos.Instance.vida;
        fuerza = ControlDatos.Instance.fuerza;
        velocidad = ControlDatos.Instance.velocidad;
        cantidadObjetos = ControlDatos.Instance.cantidadObjetos;
        numOrbes = ControlDatos.Instance.numOrbes;
        uiManager.ActualizarTextoMonedas(ControlDatos.Instance.totalMonedas);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hablando)
        {
            animator.SetTrigger("Atacar");
        }

        if (numOrbes >= orbesNecesarios)
        {
            GanarPartida();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            uiManager.TogglePanelOrbes();
        }
    }

    private void FixedUpdate()
    {
        Movimiento();
    }


    public void EstadoConversacion(bool habla)
    {
        hablando = habla;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tienda") || other.CompareTag("Orco")
            || other.CompareTag("Mole") || other.CompareTag("Treant")
            || other.CompareTag("Volador"))
        {
            hablando = false;
        }
    }

    private void Movimiento()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontal, vertical) * velocidad;
        animator.SetFloat("Camina", Mathf.Abs(rb.velocity.magnitude));

        if (horizontal > 0)
        {
            colEspada.offset = new Vector2(offsetX, offsetY);
            spriteRenderer.flipX = false;
        }
        else if (horizontal < 0)
        {
            colEspada.offset = new Vector2(-offsetX, offsetY);
            spriteRenderer.flipX = true;
        }
    }

    public void CausarHerida()
    {
        if (vida > 0)
        {
            vida--;
            ControlDatos.Instance.vida = vida;  // Actualizar la vida en ControlDatos
            uiManager.RestaCorazones(vida);
            animator.SetTrigger("Herido");

            if (vida == 0)
            {
                animator.SetTrigger("Muerto");
                Invoke(nameof(Muerte), 1.3f);
            }
        }
    }

    public void SumaVida()
    {
        if (vida < 5)
        {
            vida++;
            ControlDatos.Instance.vida = vida;  // Actualizar la vida en ControlDatos
            uiManager.SumaCorazones(vida - 1);  // Asegurarse de pasar el índice correcto
        }
    }

    public void AumentoFuerza()
    {
        fuerza = 10;
    }

    public void AumentoVelocidad()
    {
        velocidad += 2;
    }

    public void RecogerOrbe()
    {
        if (numOrbes < 4)
        {
            uiManager.ActivarOrbe(numOrbes);
            numOrbes++;
            ControlDatos.Instance.numOrbes = numOrbes; // Actualizar en el singleton
            string escenaActual = SceneManager.GetActiveScene().name;
            ControlDatos.Instance.RecogerOrbe(escenaActual);
            if (numOrbes == 4)
            {
                GanarPartida();
            }
        }
    }

    public void RecogerObjeto()
    {
        cantidadObjetos++;
        ControlDatos.Instance.cantidadObjetos = cantidadObjetos;
    }
    public void Muerte()
    {
        ControlDatos.Instance.GuardarPartida();
        ControlDatos.Instance.GuardarEstadoCompleto();
        SceneManager.LoadScene("FinPartida");
    }

    public void GanarPartida()
    {
        ControlDatos.Instance.GuardarPartida();
        ControlDatos.Instance.GuardarEstadoCompleto();
        SceneManager.LoadScene("Creditos");
    }

}
