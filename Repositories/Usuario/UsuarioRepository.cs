using System.Data.SqlClient;
using System.Security.Cryptography;
using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class UsuarioRepository: IUsuarioRepository {
  private static readonly byte[] salt = System.Text.Encoding.UTF8.GetBytes("J7f9rPm2qL1s5oKv");
  private readonly string _databaseConectionString;
  public UsuarioRepository(string databaseConectionString) {
    _databaseConectionString = databaseConectionString;
  }

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

  public Usuario GetUsuario(string nombreDeUsuario) {
    String query = String.Format(
      "SELECT * FROM usuarios WHERE nombreDeUsuario = '{0}';",
      nombreDeUsuario
    );
    return EjecutaQueryReaderUsuarios(query)[0];
  }

  public List<Usuario> GetMiembrosDeTablero(int idTablero) {
    string query = string.Format(
      "SELECT usuarios.* FROM usuarios, usuarioTablero WHERE usuarioTablero.usuarioId = usuarios.id AND usuarioTablero.tableroId = {0};",
      idTablero
    );
    return EjecutaQueryReaderUsuarios(query);
  }

  public List<Usuario> GetCandidatosAMiembrosDeTablero(int idTablero, string busqueda) {
    string query = string.Format(
      "SELECT * FROM usuarios u WHERE NOT EXISTS (SELECT * FROM usuarioTablero ut	WHERE ut.usuarioid = u.id AND ut.tableroid = {0}) AND u.nombreDeUsuario like '%{1}%' LIMIT 50;",
      idTablero,
      string.IsNullOrEmpty(busqueda) ? "" : busqueda
    );
    return EjecutaQueryReaderUsuarios(query);
  }

  public void CrearUsuario(Usuario usuario) {
    Usuario usuarioExistente = GetUsuario(usuario.NombreDeUsario);
    if (usuarioExistente != null) {
      throw new Exception("Ya existe un usuario con ese nombre de usuario");
    }

    string query = String.Format(
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
      "UPDATE usuarios SET nombreDeUsuario = '{0}', rol = {1} WHERE id = {2};",
      usuario.NombreDeUsario,
      (int) usuario.Rol,
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

    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
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
    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}