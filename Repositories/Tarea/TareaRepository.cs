using tl2_tp10_2023_TcassasT.Models;
using System.Diagnostics;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class TareaRepository: ITareaRepository {
  public void CrearTareaEnTablero(Tarea tarea) {
    String query = String.Format(
      "INSERT INTO tareas (nombre, descripcion, color, estado, idUsuarioAsignado, idTablero) VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5});",
      tarea.Nombre,
      tarea.Descripcion,
      tarea.Color,
      (int) tarea.Estado,
      tarea.IdUsuarioAsignado,
      tarea.IdTablero
    );
    EjecutaNonQueryTareas(query);
  }

  public void ModificarTarea(int idTarea, Tarea tarea) {
    String query = String.Format(
      "UPDATE tareas SET nombre = '{0}' descripcion = '{1}' color = '{2}' estado = {3} idUsuarioAsignado = {4} WHERE id = {5};",
      tarea.Nombre,
      tarea.Descripcion,
      (int) tarea.Estado,
      tarea.IdUsuarioAsignado,
      idTarea
    );
    EjecutaNonQueryTareas(query);
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
    String query = String.Format(
      "DELETE FROM tareas WHERE id = {0};",
      idTarea
    );
    EjecutaNonQueryTareas(query);
  }

  public void AsignarTareaAUsuario(int idUsuario, int idTarea) {
    String query = String.Format(
      "UPDATE tareas SET idUsuarioAsignado = {0} WHERE id = {1};",
      idUsuario,
      idTarea
    );
    EjecutaNonQueryTareas(query);
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
          tarea.IdUsuarioAsignado = Convert.ToInt32(reader[5]);
          tarea.IdTablero = Convert.ToInt32(reader[6]);

          tareas.Add(tarea);
        }
      }

      connection.Close();
    }

    return tareas;
  }

  private void EjecutaNonQueryTareas(String query) {
    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}