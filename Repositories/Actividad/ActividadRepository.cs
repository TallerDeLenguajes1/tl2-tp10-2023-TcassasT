using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;
using tl2_tp10_2023_TcassasT.Models;

public class ActividadRepository : IActividadRepository {
  private readonly string _databaseConectionString;
  public ActividadRepository(string databaseConectionString) {
    _databaseConectionString = databaseConectionString;
  }
  public void AgregarActividad(int usuarioId, int tableroId, int tareaId, string actividadTexto) {
    string query = string.Format(
      "INSERT INTO actividad (usuarioId, tableroId, tareaId, actividadTexto) VALUES ({0}, {1}, {2}, '{3}');",
      usuarioId, tableroId, tareaId, actividadTexto
    );
    EjecutaNonQueryActividad(query);
  }

  public List<ActividadExtendida> GetActividadesByTableroId(int tableroId) {
    string query = string.Format(
      "SELECT actividad.*, usuarios.nombreDeUsuario, tareas.nombre FROM actividad LEFT OUTER JOIN usuarios ON usuarios.id = actividad.usuarioId LEFT OUTER JOIN tareas ON tareas.id = actividad.TareaId WHERE tableroId = {0} ORDER BY fecha DESC;",
      tableroId
    );
    return EjecutaQueryReaderActividad(query);
  }

  public List<ActividadExtendida> GetActividadesByTareaId(int tareaId) {
    string query = string.Format(
      "SELECT actividad.*, usuarios.nombreDeUsuario, tareas.nombre FROM actividad LEFT OUTER JOIN usuarios ON usuarios.id = actividad.usuarioId LEFT OUTER JOIN tareas ON tareas.id = actividad.TareaId WHERE tareaId = {0} ORDER BY fecha DESC;",
      tareaId
    );
    return EjecutaQueryReaderActividad(query);
  }

  private List<ActividadExtendida> EjecutaQueryReaderActividad(string query) {
    List<ActividadExtendida> actividades = new List<ActividadExtendida>();

    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
      connection.Open();
      
      SqliteCommand command = new SqliteCommand(query, connection);

      using(var reader = command.ExecuteReader()) {
        while (reader.Read()) {
          ActividadExtendida actividad = new ActividadExtendida();
          actividad.Id = Convert.ToInt32(reader[0]);
          actividad.UsuarioId = Convert.ToInt32(reader[1]);
          actividad.TableroId = Convert.ToInt32(reader[2]);
          actividad.TareaId = Convert.ToInt32(reader[3]);
          actividad.ActividadTexto = reader[4].ToString();
          actividad.Fecha = DateTime.Parse(reader[5].ToString());
          actividad.UsuarioNombre = reader[6].ToString();
          actividad.TareaNombre = reader[7].ToString();
          
          actividades.Add(actividad);
        }
      }

      connection.Close();
    }

    return actividades;
  }

  private void EjecutaNonQueryActividad(string query) {
    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}
