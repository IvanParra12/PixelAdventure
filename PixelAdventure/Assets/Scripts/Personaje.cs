using UnityEngine;

public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private BoxCollider2D colEspada;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float offsetX = 1;
    private float offsetY = 0;
    private bool hablando;

    public int vida = 5;
    public int fuerza = 5;
    public int numOrbes = 0;
    private const int orbesNecesarios = 4;
    [SerializeField] UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        // Maneja la entrada del jugador para atacar y causar da�o
        if (Input.GetMouseButtonDown(0) && hablando == false)
        {
            animator.SetTrigger("Atacar");
        }

        // Verificar si se ha ganado la partida
        if (numOrbes >= orbesNecesarios)
        {
            GanarPartida();
        }

        // Mostrar/ocultar el panel de orbes al presionar la tecla I
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiManager.TogglePanelOrbes();
        }
    }

    private void FixedUpdate()
    {
        // Llama al m�todo de movimiento en cada frame de f�sica
        Movimiento();
    }

    public void EstadoConversacion(bool habla)
    {
        // Establece el estado de conversaci�n del personaje
        hablando = habla;
    }

    private void Movimiento()
    {
        // Maneja el movimiento del personaje y la animaci�n correspondiente
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontal, vertical) * velocidad;
        animator.SetFloat("Camina", Mathf.Abs(rb.velocity.magnitude));

        // Ajusta el collider del arma y la direcci�n del sprite seg�n el movimiento horizontal
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
        // Maneja la l�gica de recibir da�o y actualizar la vida del personaje
        if (vida > 0)
        {
            vida--;
            uiManager.RestaCorazones(vida);
            animator.SetTrigger("Herido");

            // Si la vida llega a 0, activa la animaci�n de muerte y destruye el objeto
            if (vida == 0)
            {
                animator.SetTrigger("Muerto");
                Invoke(nameof(Muerte), 1.3f);
            }
        }
    }

    #region OBJETOS
    public void SumaVida()
    {
        // Incrementa la vida del personaje y actualiza la interfaz
        if (vida < 5)
        {
            uiManager.SumaCorazones(vida);
            vida++;
        }
    }

    public void AumentoFuerza()
    {
        fuerza = 10; // o cualquier otro valor de aumento de fuerza
    }

    public void AumentoVelocidad()
    {
        velocidad += 2; // o cualquier otro valor de aumento de velocidad
    }

    #endregion

    public void RecogerOrbe()
    {
        if (numOrbes < 4)
        {
            uiManager.activaOrbe(numOrbes);
            numOrbes++;
            if (numOrbes == 4)
            {
                GanarPartida();
            }
        }
    }

    private void GanarPartida()
    {
        Debug.Log("�Has ganado la partida!");
        // Aqu� puedes a�adir m�s l�gica para lo que sucede al ganar la partida,
        // como mostrar un mensaje, cambiar de escena, etc.
    }

    private void Muerte()
    {
        Destroy(this.gameObject);
    }
}
