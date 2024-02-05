$("#modal-detalle-tarea").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaDescripcion = button.getAttribute("data-bs-tareaDescripcion")

  $(".modal-title").text(tareaNombre);
  $(".modal-body").text(tareaDescripcion);
})
