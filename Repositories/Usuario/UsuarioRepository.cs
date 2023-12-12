using System.Data.SqlClient;
using System.Security.Cryptography;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class UsuarioRepository: IUsuarioRepository {
  private static readonly byte[] salt = System.Text.Encoding.UTF8.GetBytes("J7f9rPm2qL1s5oKv");

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
  
    using (var pbkdf2 = new Rfc2898DeriveBytes(contrasenia, salt, 10000, HashAlgorithmName.SHA256)) {      
        byte[] hash = pbkdf2.GetBytes(32);
        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);
        return Convert.ToBase64String(hashBytes);
    }
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

  public Usuario Login(Usuario login) {
    String query = String.Format(
      @"SELECT * FROM usuarios WHERE nombreDeUsuario = '{0}'",
      login.NombreDeUsario
    );
    Usuario usuario = EjecutaQueryReaderUsuarios(query)[0];

    if (usuario == null) {
      throw new Exception("Usuario no encontrado");
    }

    if (EncriptaContrasenia(login.Contrasenia).Equals(usuario.Contrasenia)) {
      return usuario;
    } else {
      throw new Exception("Contraseña incorrecta");
    }
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
          usuariosItem.Contrasenia = reader[3].ToString();

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