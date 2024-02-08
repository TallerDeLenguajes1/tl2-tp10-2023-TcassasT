using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

public class UsuarioTableroRepository : IUsuarioTableroRepository {
  public void AgregarUsuarioATablero(int usuarioId, int tableroId) {
    string query = string.Format(
      "INSERT INTO usuarioTablero (usuarioId, tableroId) VALUES ({0}, {1});",
      usuarioId, tableroId
    );
    EjecutaNonQueryUsuarioTablero(query);
  }

  public void RemoverUsuarioDeTablero(int usuarioId, int tableroId) {
    string query = string.Format(
      "DELETE FROM usuarioTablero WHERE usuarioId = {0} AND tableroId = {1};",
      usuarioId, tableroId
    );
    EjecutaNonQueryUsuarioTablero(query);
  }

  public List<UsuarioTablero> GetMembresias(int usuarioId) {
    string query = string.Format(
      "SELECT * FROM usuarioTablero WHERE usuarioId = {0};",
      usuarioId
    );
    return EjecutaQueryReaderUsuarioTablero(query);
  }

  private List<UsuarioTablero> EjecutaQueryReaderUsuarioTablero(String query) {
    List<UsuarioTablero> membrecias = new List<UsuarioTablero>();

    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      
      SqliteCommand command = new SqliteCommand(query, connection);

      using(var reader = command.ExecuteReader()) {
        while (reader.Read()) {
          UsuarioTablero miembro = new UsuarioTablero();
          miembro.Id = Convert.ToInt32(reader[0]);
          miembro.UsuarioId = Convert.ToInt32(reader[1]);
          miembro.TableroId = Convert.ToInt32(reader[2]);
          
          membrecias.Add(miembro);
        }
      }

      connection.Close();
    }

    return membrecias;
  }

  private void EjecutaNonQueryUsuarioTablero(String query) {
    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}