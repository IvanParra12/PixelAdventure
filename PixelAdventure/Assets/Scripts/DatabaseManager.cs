using System;
using System.Data;
using Mono.Data.Sqlite;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private string dbPath;

    void Start()
    {
        // Ruta a la base de datos
        dbPath = "URI=file:" + Application.persistentDataPath + "/GameDatabase.db";

        // Crear una nueva base de datos o abrir la existente
        CreateDatabase();
    }

    void CreateDatabase()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Partida (
                        id_partida INTEGER PRIMARY KEY,
                        NombreNivel TEXT,
                        Monedas INTEGER,
                        Vidas INTEGER,
                        numOrbes INTEGER
                    );
                    CREATE TABLE IF NOT EXISTS Personaje (
                        id_personaje INTEGER PRIMARY KEY,
                        Fuerza INTEGER,
                        Velocidad INTEGER,
                        cantidadObjetos INTEGER
                    );
                    CREATE TABLE IF NOT EXISTS Enemigo (
                        id_enemigo INTEGER PRIMARY KEY,
                        Fuerza INTEGER,
                        Velocidad INTEGER,
                        Vidas INTEGER,
                        Habitat TEXT
                    );
                    CREATE TABLE IF NOT EXISTS PartidaPersonaje (
                        id_partida INTEGER,
                        id_personaje INTEGER,
                        PRIMARY KEY (id_partida, id_personaje),
                        FOREIGN KEY (id_partida) REFERENCES Partida(id_partida),
                        FOREIGN KEY (id_personaje) REFERENCES Personaje(id_personaje)
                    );
                    CREATE TABLE IF NOT EXISTS PartidaEnemigo (
                        id_partida INTEGER,
                        id_enemigo INTEGER,
                        PRIMARY KEY (id_partida, id_enemigo),
                        FOREIGN KEY (id_partida) REFERENCES Partida(id_partida),
                        FOREIGN KEY (id_enemigo) REFERENCES Enemigo(id_enemigo)
                    );
                ";
                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertPartida(int id_partida, string nombreNivel, int monedas, int vidas, int numOrbes)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Partida (id_partida, NombreNivel, Monedas, Vidas, numOrbes) VALUES (@id_partida, @NombreNivel, @Monedas, @Vidas, @numOrbes)";
                command.Parameters.AddWithValue("@id_partida", id_partida);
                command.Parameters.AddWithValue("@NombreNivel", nombreNivel);
                command.Parameters.AddWithValue("@Monedas", monedas);
                command.Parameters.AddWithValue("@Vidas", vidas);
                command.Parameters.AddWithValue("@numOrbes", numOrbes);
                command.ExecuteNonQuery();
            }
        }
    }

    // Métodos similares para insertar y manejar Personaje, Enemigo, PartidaPersonaje y PartidaEnemigo...
}
