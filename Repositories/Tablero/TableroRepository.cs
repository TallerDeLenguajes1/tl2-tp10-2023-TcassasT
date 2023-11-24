using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class TableroRepository: ITableroReposiroty {
  public List<Tablero> GetTableros() {
    String query = "SELECT * FROM tableros;";
    return EjecutaQueryReaderTableros(query);
  }

  public Tablero GetTablero(int id){
    String query = String.Format(
      "SELECT * FROM tableros WHERE id = {0};",
      id
    );
    return EjecutaQueryReaderTableros(query)[0];
  }

  public List<Tablero> GetTablerosByUserId(int id) {
    String query = String.Format(
      "SELECT * FROM tableros WHERE idUsuarioPropietario = {0};",
      id
    );
    return EjecutaQueryReaderTableros(query);
  }

  public void CrearTablero(Tablero tablero) {
    String query = String.Format(
      "INSERT INTO tableros (idUsuarioPropietario, nombre, descripcion) VALUES ({0}, '{1}', '{2}');",
      tablero.IdUsuarioPropietario,
      tablero.Nombre,
      tablero.Descripcion
    );
    EjecutaNonQueryTableros(query);
  }

  public void ModificarTablero(int id, Tablero tablero) {
    String query = String.Format(
      "UPDATE tableros SET idUsuarioPropietarioi = {0} AND nombre = '{1}' AND descripcion = '{2}' WHERE id = {3};",
      tablero.IdUsuarioPropietario,
      tablero.Nombre,
      tablero.Descripcion,
      id
    );
    EjecutaNonQueryTableros(query);
  }

  public void EliminarTablero(int id) {
    String query = String.Format(
      "DELETE FROM tableros WHERE id = {0};",
      id
    );
    EjecutaNonQueryTableros(query);
  }

  private List<Tablero> EjecutaQueryReaderTableros(String query) {
    List<Tablero> tableros = new List<Tablero>();

    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      
      SqliteCommand command = new SqliteCommand(query, connection);

      using(var reader = command.ExecuteReader()) {
        while (reader.Read()) {
          Tablero tablero = new Tablero();
          tablero.Id = Convert.ToInt32(reader[0]);
          tablero.IdUsuarioPropietario = Convert.ToInt32(reader[1]);
          tablero.Nombre = reader[2].ToString();
          tablero.Descripcion = reader[3].ToString();
          
          tableros.Add(tablero);
        }
      }

      connection.Close();
    }

    return tableros;
  }

  private void EjecutaNonQueryTableros(String query) {
    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}