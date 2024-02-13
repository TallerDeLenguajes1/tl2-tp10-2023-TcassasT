using tl2_tp10_2023_TcassasT.Models;
using tl2_tp10_2023_TcassasT.Interfaces;
using Microsoft.Data.Sqlite;

namespace tl2_tp10_2023_TcassasT.Models;

public class TableroRepository: ITableroReposiroty {
  private readonly string _databaseConectionString;
  private readonly IActividadRepository _actividadRepository;
  private readonly IUsuarioRepository _usuarioRepository;
  public TableroRepository(string databaseConectionString, IActividadRepository actividadRepository, IUsuarioRepository usuarioRepository) {
    _databaseConectionString = databaseConectionString;
    _actividadRepository = actividadRepository;
    _usuarioRepository = usuarioRepository;
  }

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
      "SELECT tableros.* FROM tableros, usuarioTablero WHERE usuarioTablero.tableroId = tableros.id AND usuarioTablero.usuarioId = {0};",
      id
    );
    return EjecutaQueryReaderTableros(query);
  }

  public List<Tablero> GetTablerosByTableroId(List<int> tablerosId) {
    string tablerosQueryInStatement = "";
    tablerosId.ForEach((int tableroId) => {
      tablerosQueryInStatement += tableroId + ",";
    });

    if (tablerosQueryInStatement == "") {
      return new List<Tablero>();
    }

    tablerosQueryInStatement = tablerosQueryInStatement.Substring(0, tablerosQueryInStatement.Length - 1);

    string query = string.Format(
      "SELECT * FROM tableros WHERE id IN ({0});",
      tablerosQueryInStatement
    );

    return EjecutaQueryReaderTableros(query);
  }

  public List<TableroExtendido> GetTablerosExtendidosByTableroId(List<int> tablerosId) {
    List<TableroExtendido> tablerosExtendidos = new List<TableroExtendido>();
    List<Tablero> tableros = GetTablerosByTableroId(tablerosId);

    tableros.ForEach((Tablero tablero) => {
      List<ActividadExtendida> actividades = _actividadRepository.GetActividadesByTableroId(tablero.Id);
      tablerosExtendidos.Add(new TableroExtendido() {
        Id = tablero.Id,
        Nombre = tablero.Nombre,
        Descripcion = tablero.Descripcion,
        IdUsuarioPropietario = tablero.IdUsuarioPropietario,
        Actividades = actividades,
      });
    });

    return tablerosExtendidos;
  }

  public TableroMembrecias GetTableroMembreciasByTableroId(int id) {
    Tablero tablero = GetTablero(id);
    List<Usuario> miembros = _usuarioRepository.GetMiembrosDeTablero(id);

    TableroMembrecias tableroMembrecias = new TableroMembrecias() {
      Id = tablero.Id,
      Nombre = tablero.Nombre,
      Descripcion = tablero.Descripcion,
      IdUsuarioPropietario = tablero.IdUsuarioPropietario,
      Miembros = miembros
    };

    return tableroMembrecias;
  }

  public List<TableroMembrecias> GetTablerosMembreciasByTableroId(List<int> tablerosId) {
    List<TableroMembrecias> tablerosMembrecias = new List<TableroMembrecias>();
    List<Tablero> tableros = GetTablerosByTableroId(tablerosId);

    tableros.ForEach((Tablero tablero) => {
      List<Usuario> miembros = _usuarioRepository.GetMiembrosDeTablero(tablero.Id);
      tablerosMembrecias.Add(new TableroMembrecias() {
        Id = tablero.Id,
        Nombre = tablero.Nombre,
        Descripcion = tablero.Descripcion,
        IdUsuarioPropietario = tablero.IdUsuarioPropietario,
        Miembros = miembros
      });
    });

    return tablerosMembrecias;
  }

  public int CrearTablero(Tablero tablero) {
    string query = string.Format(
      "INSERT INTO tableros (idUsuarioPropietario, nombre, descripcion) VALUES ({0}, '{1}', '{2}');",
      tablero.IdUsuarioPropietario,
      tablero.Nombre,
      tablero.Descripcion
    );
    EjecutaNonQueryTableros(query);

    string query2 = string.Format(
      "SELECT * FROM tableros WHERE idUsuarioPropietario = {0} AND nombre = '{1}' AND descripcion = '{2}' ORDER BY id DESC",
      tablero.IdUsuarioPropietario, tablero.Nombre, tablero.Descripcion
    );
    Tablero tableroRecienCreado = EjecutaQueryReaderTableros(query2)[0];
    return tableroRecienCreado.Id;
  }

  public void ModificarTablero(int id, Tablero tablero) {
    String query = String.Format(
      "UPDATE tableros SET idUsuarioPropietario = {0}, nombre = '{1}', descripcion = '{2}' WHERE id = {3};",
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

    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
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
    using (SqliteConnection connection = new SqliteConnection(_databaseConectionString)) {
      connection.Open();
      SqliteCommand command = new SqliteCommand(query, connection);
      command.ExecuteNonQuery();
      connection.Close();
    }
  }
}
