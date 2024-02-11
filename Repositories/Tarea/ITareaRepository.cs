using tl2_tp10_2023_TcassasT.Models;

namespace tl2_tp10_2023_TcassasT.Interfaces;

public interface ITareaRepository {
  void CrearTareaEnTablero(Tarea tarea);
  void ModificarTarea(int idTarea, Tarea tarea);
  void ModificarEstado(int idTarea, EstadoTarea estado);
  void ModificarArchivado(int idTarea, ArchivadoTarea archivado);
  Tarea GetTarea(int idTarea);
  List<Tarea> GetTareasByUsuarioId(int idUsuario);
  List<Tarea> GetTareasByTableroId(int idTablero);
  List<Tarea> GetTareasByEstado(int estado);
  void EliminarTarea(int idTarea);
  void AsignarTareaAUsuario(int idUsuario, int idTarea);
}
