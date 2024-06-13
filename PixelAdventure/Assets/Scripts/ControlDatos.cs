using System.Collections.Generic;
using UnityEngine;
using SQLite;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
using System;

public class ControlDatos : MonoBehaviour
{
    public static ControlDatos Instance;

    public int currentPartidaId;
    public int vida;
    public int fuerza;
    public float velocidad;
    public int cantidadObjetos;
    public int totalMonedas;
    public int numOrbes;
    public List<int> corazonesEstado;
    public List<int> orbesEstado;
    public List<string> objetosEnPanel;
    public bool mejoraFuerzaActivada;
    public bool mejoraVelocidadActivada;
    public bool botonEspadaComprado;
    public bool botonBotasComprado;
    public Dictionary<string, bool> orbesRecogidos = new Dictionary<string, bool>();

    private SQLiteConnection db;
    private const string dbPath = @"D:\GitHub\PixelAdventure\PixelAdventure\BD\GameDatabase.db";
    private static readonly object dbLock = new object(); // Añadir un objeto lock

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            db = new SQLiteConnection(dbPath);

            // Crear las tablas si no existen
            db.CreateTable<Partida>();
            db.CreateTable<PersonajeDB>();
            db.CreateTable<EnemigoDB>();
            db.CreateTable<PersonajePartida>();  // Asegúrate de crear la tabla para la relación

            Dinero.sumaMoneda -= ActualizarMonedas;  // Desuscribir primero para asegurar que no se duplica
            Dinero.sumaMoneda += ActualizarMonedas;  // Suscribir al evento
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        db.Close();
        Dinero.sumaMoneda -= ActualizarMonedas;  // Desuscribir del evento
    }

    public void RecogerOrbe(string escena)
    {
        if (!orbesRecogidos.ContainsKey(escena))
        {
            orbesRecogidos[escena] = true;
        }
    }

    public bool OrbeRecogido(string escena)
    {
        return orbesRecogidos.ContainsKey(escena) && orbesRecogidos[escena];
    }

    private void ActualizarMonedas(int cantidad)
    {
        totalMonedas += cantidad;
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ActualizarTextoMonedas(totalMonedas);
        }
    }

    public void GuardarPersonajePartida(int idPersonaje, int idPartida)
    {
        var relacion = new PersonajePartida { id_personaje = idPersonaje, id_partida = idPartida };
        lock (dbLock)
        {
            db.Insert(relacion);
        }
    }

    public void GuardarEnemigo(EnemigoDB enemigo)
    {
        lock (dbLock)
        {
            db.Insert(enemigo);
        }
    }

    public int GuardarEstadoPersonaje()
    {
        var personajeDB = new PersonajeDB
        {
            vida = vida,
            fuerza = fuerza,
            velocidad = velocidad,
            cantidadObjetos = cantidadObjetos
        };

        lock (dbLock)
        {
            db.Insert(personajeDB);
            return db.ExecuteScalar<int>("select last_insert_rowid()");
        }
    }


    public void CargarEstadoPersonaje(int id_personaje)
    {
        lock (dbLock)
        {
            var personaje = db.Find<PersonajeDB>(id_personaje);
            if (personaje != null)
            {
                vida = personaje.vida;
                fuerza = personaje.fuerza;
                velocidad = personaje.velocidad;
                cantidadObjetos = personaje.cantidadObjetos;
            }
        }
    }

    public void GuardarEstadoUI(UIManager uiManager)
    {
        corazonesEstado = new List<int>();
        orbesEstado = new List<int>();
        objetosEnPanel = new List<string>();

        foreach (var corazon in uiManager.corazones)
        {
            corazonesEstado.Add(corazon.GetComponent<Image>().sprite == uiManager.corazonActivado ? 1 : 0);
        }

        foreach (var orbe in uiManager.orbes)
        {
            orbesEstado.Add(orbe.GetComponent<Image>().sprite == uiManager.orbeVerde ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeAzul ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeAmarillo ||
                            orbe.GetComponent<Image>().sprite == uiManager.orbeRojo ? 1 : 0);
        }

        foreach (Transform item in uiManager.panelEquipo.transform)
        {
            objetosEnPanel.Add(item.gameObject.name.Replace("(Clone)", "").Trim());
        }

        mejoraFuerzaActivada = uiManager.imagenDaño.activeSelf;
        mejoraVelocidadActivada = uiManager.imagenVelocidad.activeSelf;
        botonEspadaComprado = !uiManager.BtnEspada.interactable;
        botonBotasComprado = !uiManager.BtnBotas.interactable;
    }
    public void CargarEstadoUI(UIManager uiManager)
    {
        for (int i = 0; i < corazonesEstado.Count; i++)
        {
            uiManager.corazones[i].GetComponent<Image>().sprite = corazonesEstado[i] == 1 ? uiManager.corazonActivado : uiManager.corazonQuitado;
        }

        for (int i = 0; i < orbesEstado.Count; i++)
        {
            if (orbesEstado[i] == 1)
                uiManager.ActivarOrbe(i);
            else
                uiManager.DesactivarOrbe(i);
        }

        foreach (string objeto in objetosEnPanel)
        {
            GameObject prefab = Resources.Load<GameObject>(objeto);
            if (prefab)
            {
                GameObject inst = Instantiate(prefab, Vector3.zero, Quaternion.identity, uiManager.panelEquipo.transform);
                inst.name = prefab.name;
            }
        }

        uiManager.imagenDaño.SetActive(mejoraFuerzaActivada);
        uiManager.imagenVelocidad.SetActive(mejoraVelocidadActivada);
        uiManager.BtnEspada.interactable = !botonEspadaComprado;
        uiManager.BtnBotas.interactable = !botonBotasComprado;
    }

    public void ReiniciarDatos()
    {
        vida = 5;
        fuerza = 1;
        velocidad = 5;
        cantidadObjetos = 0;
        totalMonedas = 0;
        numOrbes = 0;
        corazonesEstado = new List<int> { 1, 1, 1, 1, 1 };
        orbesEstado = new List<int> { 0, 0, 0, 0 };
        objetosEnPanel = new List<string>();
    }

    public void CrearNuevaPartida()
    {
        var nuevaPartida = new Partida
        {
            NombreNivel = SceneManager.GetActiveScene().name,
            Monedas = 0,  // Inicializa con los valores por defecto para el inicio del juego
            Vidas = vida,
            numOrbes = 0
        };

        lock (dbLock)
        {
            db.Insert(nuevaPartida);
            currentPartidaId = db.ExecuteScalar<int>("select last_insert_rowid()");
        }
    }

    public void GuardarPartida()
    {
        // Obtener el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;

        Partida partidaExistente = db.Find<Partida>(currentPartidaId);
        if (partidaExistente != null)
        {
            partidaExistente.NombreNivel = currentSceneName;  // Actualizar con el nombre de la escena actual
            partidaExistente.Monedas = totalMonedas;
            partidaExistente.Vidas = vida;
            partidaExistente.numOrbes = numOrbes;

            lock (dbLock)
            {
                db.Update(partidaExistente);
            }
        }
    }

    public void CargarPartida(int id_partida)
    {
        lock (dbLock)
        {
            var partida = db.Find<Partida>(id_partida);
            if (partida != null)
            {
                vida = partida.Vidas;
                totalMonedas = partida.Monedas;
                numOrbes = partida.numOrbes;
            }
            else
            {
                Debug.LogWarning("Partida no encontrada en la base de datos.");
            }
        }
    }

    public void GuardarEnemigosEnEscena(int idPartida)
    {
        var escena = SceneManager.GetActiveScene().name;
        var enemigos = FindObjectsOfType<Enemigo>();
        var nombresGuardados = new HashSet<string>();

        lock (dbLock)
        {
            foreach (var enemigo in enemigos)
            {
                if (!nombresGuardados.Contains(enemigo.tag))
                {
                    nombresGuardados.Add(enemigo.tag);

                    var enemigoDB = new EnemigoDB
                    {
                        id_partida = idPartida,
                        nombre = enemigo.tag,
                        vida = enemigo.vida,
                        fuerza = enemigo.fuerza,
                        velocidad = enemigo.velocidad,
                        habitat = escena
                    };

                    db.Insert(enemigoDB);
                    Debug.Log($"Enemigo guardado con ID: {enemigoDB.id_enemigo}, nombre: {enemigo.name}");
                }
            }
        }
    }

    public void GuardarEstadoCompleto()
    {
        var uiManager = FindObjectOfType<UIManager>();

        int idPersonaje = GuardarEstadoPersonaje();
        int idPartida = currentPartidaId;  // Asegúrate de que este ID es correcto y está actualizado

        GuardarPersonajePartida(idPersonaje, idPartida);
        GuardarEstadoUI(uiManager);
        GuardarEnemigosEnEscena(idPartida);
    }


    public void CargarEstadoCompleto(int id_personaje, int id_partida, UIManager uiManager)
    {
        CargarEstadoPersonaje(id_personaje);
        CargarEstadoUI(uiManager);
        CargarPartida(id_partida);
        ActualizarMonedas(totalMonedas);  // Actualiza el UI con las monedas cargadas
    }
}
