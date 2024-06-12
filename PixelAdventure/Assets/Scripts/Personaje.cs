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

    public int vida = 5;
    public int fuerza = 1;
    public int numOrbes = 0;
    private const int orbesNecesarios = 4;
    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        uiManager = GameObject.FindObjectOfType<UIManager>();

        // Cargar el estado del personaje desde el singleton al iniciar
        ControlDatos.Instance.CargarEstadoPersonaje(this);
    }

    private void OnDestroy()
    {
        // Guardar el estado del personaje en el singleton antes de destruirlo
        ControlDatos.Instance.GuardarEstadoPersonaje(this);
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
        if (other.CompareTag("Tienda"))
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
            uiManager.SumaCorazones(vida);
            vida++;
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
            if (numOrbes == 4)
            {
                GanarPartida();
            }
        }
    }

    private void GanarPartida()
    {
        SceneManager.LoadScene("Creditos");
    }

    private void Muerte()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene("FinPartida");
    }
}
