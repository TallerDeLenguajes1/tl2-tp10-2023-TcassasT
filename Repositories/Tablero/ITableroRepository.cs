using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface ITableroReposiroty {
  List<Tablero> GetTableros();
  Tablero? GetTablero(int id);
  List<Tablero> GetTablerosByUserId(int id);
  List<Tablero> GetTablerosByTableroId(List<int> tablerosId);
  List<TableroExtendido> GetTablerosExtendidosByTableroId(List<int> tablerosId);
  List<TableroMembrecias> GetTablerosMembreciasByTableroId(List<int> tablerosId);
  TableroMembrecias GetTableroMembreciasByTableroId(int id);
  int CrearTablero(Tablero tablero);
  void ModificarTablero(int id, Tablero tablero);
  void EliminarTablero(int id);
}