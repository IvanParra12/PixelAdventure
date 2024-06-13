using SQLite;
using static Unity.Collections.AllocatorManager;

public class Partida
{
    [PrimaryKey, AutoIncrement]
    public int id_partida { get; set; }
    public string NombreNivel { get; set; }
    public int Monedas { get; set; }
    public int Vidas { get; set; }
    public int numOrbes { get; set; }
}

public class PersonajeDB
{
    [PrimaryKey, AutoIncrement]
    public int id_personaje { get; set; }
    public int vida { get; set; }
    public int fuerza { get; set; }
    public float velocidad { get; set; }
    public int cantidadObjetos { get; set; }
}


public class EnemigoDB
{
    [PrimaryKey, AutoIncrement]
    public int id_enemigo { get; set; }
    public int id_partida { get; set; }  // Agregado para vincular con Partida
    public string nombre { get; set; }
    public int vida { get; set; }
    public int fuerza { get; set; }
    public float velocidad { get; set; }
    public string habitat { get; set; }
}

public class PersonajePartida
{
    public int id_personaje { get; set; }
    public int id_partida { get; set; }
}

