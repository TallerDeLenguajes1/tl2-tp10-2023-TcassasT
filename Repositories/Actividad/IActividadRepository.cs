using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface IActividadRepository {
  void AgregarActividad(int usuarioId, int tableroId, int tareaId, string actividadTexto);
  List<ActividadExtendida> GetActividadesByTableroId(int tableroId);
  List<ActividadExtendida> GetActividadesByTareaId(int tareaId);
}
