using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace tl2_tp10_2023_TcassasT.Middlewares;

public class VerificarDatosEnSesion {
  private readonly RequestDelegate _next;

  public VerificarDatosEnSesion(RequestDelegate next) {
    _next = next;
  }

  public async Task Invoke(HttpContext context){
    bool requestVaALogin = context.Request.Path.StartsWithSegments("/usuarios/login");
    bool requestVaARegistrarse = context.Request.Path.StartsWithSegments("/usuarios/nuevo");
    bool noHayDatosDeSesion =
      context.Session.GetString("NombreDeUsuario")  == null ||
      context.Session.GetString("Rol")              == null ||
      context.Session.GetInt32("UsuarioId")         == null;

    if (!requestVaALogin && !requestVaARegistrarse) {
      if (noHayDatosDeSesion) {
        context.Response.Redirect("/usuarios/login");
        return;
      }
    }

    await _next(context);
  }
}
