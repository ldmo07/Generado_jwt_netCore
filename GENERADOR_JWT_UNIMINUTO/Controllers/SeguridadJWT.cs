using GENERADOR_JWT_UNIMINUTO.Helper;
using GENERADOR_JWT_UNIMINUTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GENERADOR_JWT_UNIMINUTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeguridadJWT : ControllerBase
    {
        #region VARIABLES
        private readonly IDirectorioActivoHelper _helperDa;
        private readonly IJwtGenerador _jwtGenerador;
        #endregion
        #region CONSTRUCTOR
        public SeguridadJWT(IDirectorioActivoHelper helperDa, IJwtGenerador jwtGenerador)
        {
            _helperDa = helperDa;
            _jwtGenerador = jwtGenerador;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        [Route("loginDA")]
        public IActionResult login(UsuarioLogin data)
        {
            bool isLogeadDA = _helperDa.loginDa(data.email, data.password);
            if (isLogeadDA)
            {

                    var token = _jwtGenerador.crearToken(data.email,data.password);
                    return Ok(new { ok = true, logeado = "Si", tokenDiplogrados = token });
            }
            else
            {
                return BadRequest(new { ok = false, logeado = "Error de credenciales" });
            }
        }

        /* TODO :: FUNCIONA SIN MIDDLEWARE Y HACE LA CAPTURA DE LOS CLAIMS EN EL METODO
         
        [HttpGet]
        [Authorize]
        [Route("validarToken")]
        public async Task<IActionResult> protegido()
        {
            /*var accessToken = await HttpContext.GetTokenAsync("access_token");
            var dataToken = new JwtSecurityToken(accessToken).Claims;
            var email = dataToken.Where(x => x.Type.Equals("email")).Select(x => x.Value).FirstOrDefault();
            return Ok(new { ok = true, email});
           
        }
        */

        [HttpGet]
        [Route("validarToken")]
        public IActionResult protegido()
        {
            return Ok(new { ok = true, email = HttpContext.Items["email"] });
        }
    }
}
