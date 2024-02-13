$("#modal-remover-miembro").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const usuarioId = button.getAttribute("data-bs-usuarioId");
  const usuarioNombre = button.getAttribute("data-bs-usuarioNombre");

  $("#remover-miembro-id").val(usuarioId);
  $("#remover-miembro-nombre").text(usuarioNombre);
})

$("#modal-otorgar-propiedad").on("show.bs.modal", (event) => {
  const button = event.relatedTarget;
  const usuarioId = button.getAttribute("data-bs-usuarioId");
  const usuarioNombre = button.getAttribute("data-bs-usuarioNombre");

  $("#otorgar-propiedad-id").val(usuarioId);
  $("#otorgar-propiedad-nombre").text(usuarioNombre);
})

$(document).ready(function() {
  $("#agregar-miembro-select").selectpicker();
  $(".bs-searchbox").find("input").on("input", (event) => {
    const tableroId = $("#boton-agregar-miembro").attr("data-bs-tableroId");
    const busqueda = event.target.value;
    const url = `/tableros/${tableroId}/miembros/candidatos?busqueda=${busqueda}`;
    fetch(url)
      .then(response => {
        if (!response.ok) {
          throw new Error("Error al candidatos");
        }
        return response.json();
      })
      .then(candidatos => {
        formateaEInsertaActividadesEnDOM(candidatos);
      })
      .catch(error => {
        console.error('Error en la solicitud:', error);
      });
  });
});

function formateaEInsertaActividadesEnDOM(candidatos) {
  const select = $("#agregar-miembro-select");

  if (candidatos.length == 0) {
    select.html("");
    select.selectpicker("refresh");
    return;
  }

  const candidatosHtml = [];
  candidatos.map((candidato) => {
    candidatosHtml.push(`
      <option value=${candidato.id}>${candidato.nombreDeUsario}</option>
    `);
  });

  select.html(candidatosHtml.join(''));
  select.selectpicker("refresh");
}
