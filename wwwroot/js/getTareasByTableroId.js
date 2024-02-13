$("#modal-detalle-tarea").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaDescripcion = button.getAttribute("data-bs-tareaDescripcion")
  const tareaId = button.getAttribute("data-bs-tareaId");
  const tableroId = button.getAttribute("data-bs-tableroId");

  getActividadesByTareaId(tableroId, tareaId);

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

$("#modal-archivar-tarea").on("show.bs.modal", (event) => {
  debugger;
  const button = event.relatedTarget;
  const tareaNombre = button.getAttribute("data-bs-tareaNombre");
  const tareaId = button.getAttribute("data-bs-tareaId");
  const formAction = $("#modal-archivar-tarea form").attr("action");

  $("#archivar-nombre").text(tareaNombre);
  $("#modal-archivar-tarea form").attr("action", formAction.replace("replaceTareaId", tareaId));
})

function getActividadesByTareaId(tableroId, tareaId) {
  const url = `/tableros/${tableroId}/tareas/${tareaId}/actividad`;

  fetch(url)
    .then(response => {
      if (!response.ok) {
        throw new Error("Error al obtener registros de actividad");
      }
      return response.json();
    })
    .then(actividades => {
      formateaEInsertaActividadesEnDOM(actividades);
    })
    .catch(error => {
      console.error('Error en la solicitud:', error);
    });
}

function formateaEInsertaActividadesEnDOM(actividades) {
  const divActividades = $("#detalle-tarea-actividades");

  if (actividades.length == 0) {
    return divActividades.text("Sin actividades registradas");
  }

  const actividadHtml = [];
  actividades.map((actividad) => {
    actividadHtml.push(`
      <div>
        <p class="m-0">${actividad.actividadTexto}</p>
        <p class="text-black-50 m-0 fw-ligh">Fecha: ${actividad.fecha} - Usuario: ${actividad.usuarioNombre}</p>
        <hr class="bg-danger border-2 border-top border-lightdark mt-0" />
      </div>
    `);
  });

  divActividades.html(actividadHtml.join(''));
}
