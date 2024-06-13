using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int vida = 1;
    public int fuerza = 1;
    public float velocidad = 2.0f;

    public virtual void Herida()
    {
        vida--;
        if (vida <= 0)
        {
            Muerte();
        }
    }

    protected virtual void Muerte()
    {
        Destroy(this.gameObject);
    }
}
