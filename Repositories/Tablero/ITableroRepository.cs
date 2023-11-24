using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface ITableroReposiroty {
  List<Tablero> GetTableros();
  Tablero GetTablero(int id);
  List<Tablero> GetTablerosByUserId(int id);
  void CrearTablero(Tablero tablero);
  void ModificarTablero(int id, Tablero tablero);
  void EliminarTablero(int id);
}