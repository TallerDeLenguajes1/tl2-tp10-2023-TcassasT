using tl2_tp10_2023_TcassasT.Models;
using System.Diagnostics;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class TareaRepository: ITareaRepository {
  public void CrearTareaEnTablero(Tarea tarea) {
    EjecutaNonQueryTareas(
      @"INSERT INTO tareas (nombre, descripcion, color, estado, idUsuarioAsignado, idTablero) VALUES (@nombre, @descripcion, @color, @estado, @idUsuarioAsignado, @idTablero)",
      tarea
    );
  }

  public void ModificarTarea(int idTarea, Tarea tarea) {
    tarea.Id = idTarea;
    EjecutaNonQueryTareas(
      @"UPDATE tareas SET nombre = @nombre, descripcion = @descripcion, color = @color, estado = @estado, idUsuarioAsignado = @idUsuarioAsignado WHERE id = @id;",
      tarea
    );
  }

  public void ModificarEstado(int idTarea, EstadoTarea estado) {
    Tarea tarea = new Tarea() { Estado = estado, Id = idTarea };
    EjecutaNonQueryTareas(
      @"UPDATE tareas SET estado = @estado WHERE id = @id;",
      tarea
    );
  }

  public Tarea GetTarea(int idTarea) {
    String query = String.Format(
      "SELECT * FROM tareas WHERE id = {0};",
      idTarea
    );
    return EjecutaQueryReaderTareas(query)[0];
  }

  public List<Tarea> GetTareasByUsuarioId(int idUsuario) {
    String query = String.Format(
      "SELECT * FROM tareas WHERE idUsuarioAsignado = {0};",
      idUsuario
    );
    return EjecutaQueryReaderTareas(query);
  }

  public List<Tarea> GetTareasByTableroId(int idTablero) {
    String query = String.Format(
      "SELECT * FROM tareas WHERE idTablero = {0};",
      idTablero
    );
    return EjecutaQueryReaderTareas(query);
  }

  public List<Tarea> GetTareasByEstado(int estado) {
    String query = String.Format(
      "SELECT * FROM tareas WHERE estado = {0};",
      estado
    );
    return EjecutaQueryReaderTareas(query);
  }

  public void EliminarTarea(int idTarea) {
    Tarea tarea = new Tarea {
      Id = idTarea
    };
    EjecutaNonQueryTareas("DELETE FROM tareas WHERE id = @id;", tarea);
  }

  public void AsignarTareaAUsuario(int idUsuario, int idTarea) {
    Tarea tarea = new Tarea {
      Id = idTarea,
      IdUsuarioAsignado = idUsuario
    };
    EjecutaNonQueryTareas("UPDATE tareas SET idUsuarioAsignado = @idUsuarioAsignado WHERE id = @id;", tarea);
  }

  private List<Tarea> EjecutaQueryReaderTareas(String query) {
    List<Tarea> tareas = new List<Tarea>();

    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();

      SqliteCommand command = new SqliteCommand(query, connection);

      using(var reader = command.ExecuteReader()) {
        while (reader.Read()) {
          Tarea tarea = new Tarea();
          tarea.Id = Convert.ToInt32(reader[0]);
          tarea.Nombre = reader[1].ToString();
          tarea.Descripcion = reader[2].ToString();
          tarea.Color = reader[3].ToString();
          tarea.Estado = (EstadoTarea) Convert.ToInt32(reader[4]);
          tarea.IdUsuarioAsignado = reader.IsDBNull(5) ? null : Convert.ToInt32(reader[5]);
          tarea.IdTablero = Convert.ToInt32(reader[6]);

          tareas.Add(tarea);
        }
      }

      connection.Close();
    }

    return tareas;
  }

  private void EjecutaNonQueryTareas(String query, Tarea nuevaTarea) {
    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      try {
        connection.Open();

        SqliteCommand command = new SqliteCommand(query, connection);

        command.Parameters.Add(new SqliteParameter("@id", nuevaTarea.Id));
        command.Parameters.Add(new SqliteParameter("@nombre", nuevaTarea.Nombre));
        command.Parameters.Add(new SqliteParameter("@descripcion", nuevaTarea.Descripcion));
        command.Parameters.Add(new SqliteParameter("@color", nuevaTarea.Color));
        command.Parameters.Add(new SqliteParameter("@estado", (int) nuevaTarea.Estado));
        command.Parameters.Add(new SqliteParameter("@idUsuarioAsignado", nuevaTarea.IdUsuarioAsignado == null ? (object) DBNull.Value : nuevaTarea.IdUsuarioAsignado));
        command.Parameters.Add(new SqliteParameter("@idTablero", nuevaTarea.IdTablero));

        command.ToString();

        command.ExecuteNonQuery();
        connection.Close();
      } catch (System.Exception e) {
       
        Console.WriteLine(e.Message);
      }
    }
  }
}