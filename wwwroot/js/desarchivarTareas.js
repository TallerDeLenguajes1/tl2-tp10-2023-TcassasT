$("#modal-desarchivar-tarea").on("show.bs.modal", (event) => {
  debugger;
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaId = button.getAttribute("data-bs-tareaId");
  const formAction = $("#modal-desarchivar-tarea form").attr("action");

  $("#desarchivar-nombre").text(tareaNombre);
  $("#modal-desarchivar-tarea form").attr("action", formAction.replace("replaceTareaId", tareaId));
})
