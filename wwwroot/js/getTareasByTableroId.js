$("#modal-detalle-tarea").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaDescripcion = button.getAttribute("data-bs-tareaDescripcion")

  $("#detalle-tarea-nombre").text(tareaNombre);
  $("#detalle-tarea-descripcion").text(tareaDescripcion);
})

$("#modal-cambiar-estado-tarea").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaId = button.getAttribute("data-bs-tareaId");
  const formAction = $("#modal-cambiar-estado-tarea form").attr("action");

  $("#cambiar-estado-nombre").text(tareaNombre);
  $("#modal-cambiar-estado-tarea form").attr("action", formAction.replace("replaceTareaId", tareaId));
})
