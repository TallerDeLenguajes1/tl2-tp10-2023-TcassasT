using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace tl2_tp10_2023_TcassasT.Middlewares;

public class VerificarDatosEnSesion {
  private readonly RequestDelegate _next;

  public VerificarDatosEnSesion(RequestDelegate next) {
    _next = next;
  }

  public async Task Invoke(HttpContext context){
    bool requestVaALogin = context.Request.Path.StartsWithSegments("/Usuario/Login");
    bool noHayDatosDeSesion = context.Session.GetString("NombreDeUsuario") == null || context.Session.GetString("Rol") == null;

    if (!requestVaALogin) {
      if (noHayDatosDeSesion) {
        context.Response.Redirect("/Usuario/Login");
        return;
      }
    }

    await _next(context);
  }
}
