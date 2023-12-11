using System.Data.SqlClient;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class UsuarioRepository: IUsuarioRepository {
  public List<Usuario> GetUsuarios() {
    return EjecutaQueryReaderUsuarios("SELECT * FROM usuarios;");
  }

  public Usuario GetUsuario(int id) {
    String query = String.Format(
      "SELECT * FROM usuarios WHERE id = {0};",
      id
    );
    return EjecutaQueryReaderUsuarios(query)[0];
  }

  public void CrearUsuario(Usuario usuario) {
    String query = String.Format(
      "INSERT INTO usuarios (nombreDeUsuario, rol, contrasenia) VALUES ('{0}', {1}, '{2}');",
      usuario.NombreDeUsario,
      (int) usuario.Rol,
      EncriptaContrasenia(usuario.Contrasenia)
    );
    EjecutaNonQueryUsuarios(query);
  }

  private String EncriptaContrasenia(String? contrasenia) {
    if (string.IsNullOrEmpty(contrasenia)) {
      throw new Exception("Contraseña no puede ser nula");
    }

    byte[] bytesContrasenia = new byte[contrasenia.Length];
    bytesContrasenia = System.Text.Encoding.UTF8.GetBytes(contrasenia);
    return Convert.ToBase64String(bytesContrasenia);
  }

  public void ModificarUsuario(int id, Usuario usuario) {
    String query = String.Format(
      "UPDATE usuarios SET nombreDeUsuario = '{0}' WHERE id = {1};",
      usuario.NombreDeUsario,
      id
    );
    EjecutaNonQueryUsuarios(query);
  }

  public void EliminarUsuario(int id) {
    String query = String.Format(
      "DELETE FROM usuarios WHERE id = {0};",
      id
    );
    EjecutaNonQueryUsuarios(query);
  }

  private List<Usuario> EjecutaQueryReaderUsuarios(String query) {
    List<Usuario> usuarios = new List<Usuario>();

    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      
      SqliteCommand command = new SqliteCommand(query, connection);

      using(var reader = command.ExecuteReader()) {
        while (reader.Read()) {
          Usuario usuariosItem = new Usuario();
          usuariosItem.Id = Convert.ToInt32(reader[0]);
          usuariosItem.NombreDeUsario = reader[1].ToString();
          usuariosItem.Rol = (RolUsuario) Convert.ToInt32(reader[2]);

          usuarios.Add(usuariosItem);
        }
      }

      connection.Close();
    }

    return usuarios;
  }

  private void EjecutaNonQueryUsuarios(String query) {
    string connectionString = "Data Source=DB/kanban.db;";
    using (SqliteConnection connection = new SqliteConnection(connectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}