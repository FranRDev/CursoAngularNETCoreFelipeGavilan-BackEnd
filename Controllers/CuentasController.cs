using AutoMapper;
using back_end.DTOs;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase {

        private readonly SignInManager<IdentityUser> administradorInicioSesion;
        private readonly UserManager<IdentityUser> administradorUsuarios;
        private readonly IConfiguration configuracion;
        private readonly ApplicationDbContext contexto;
        private readonly IMapper mapeador;

        public CuentasController(SignInManager<IdentityUser> administradorInicioSesion, UserManager<IdentityUser> administradorUsuarios, IConfiguration configuracion,
            ApplicationDbContext contexto, IMapper mapeador) {
            this.administradorInicioSesion = administradorInicioSesion;
            this.administradorUsuarios = administradorUsuarios;
            this.configuracion = configuracion;
            this.contexto = contexto;
            this.mapeador = mapeador;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Crear([FromBody] CredencialesUsuarioDTO credenciales) {
            var usuario = new IdentityUser { UserName = credenciales.Correo, Email = credenciales.Correo };
            var resultado = await administradorUsuarios.CreateAsync(usuario, credenciales.Clave);

            if (resultado.Succeeded) {
                return await ConstruirToken(credenciales);

            } else {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credenciales) {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Email, credenciales.Correo)
            };

            var usuario = await administradorUsuarios.FindByEmailAsync(credenciales.Correo);
            var claimsBaseDeDatos = await administradorUsuarios.GetClaimsAsync(usuario);

            claims.AddRange(claimsBaseDeDatos);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["llavejwt"]));
            var sc = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(claims: claims, expires: expiracion, signingCredentials: sc);

            return new RespuestaAutenticacionDTO() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }

        [HttpPost("IniciarSesion")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> IniciarSesion([FromBody] CredencialesUsuarioDTO credenciales) {
            var resultado = await administradorInicioSesion.PasswordSignInAsync(credenciales.Correo, credenciales.Clave, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded) {
                return await ConstruirToken(credenciales);

            } else {
                return BadRequest("Credenciales incorrectas");
            }
        }

        [HttpGet("Usuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerUsuarios([FromQuery] PaginacionDTO paginacionDTO) {
            var consultable = contexto.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(consultable);
            var usuarios = await consultable.OrderBy(u => u.Email).Paginar(paginacionDTO).ToListAsync();
            return mapeador.Map<List<UsuarioDTO>>(usuarios);
        }

        [HttpPost("AnhadirAdministrador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult> AnhadirAdministrador([FromBody] string idUsuario) {
            var usuario = await administradorUsuarios.FindByIdAsync(idUsuario);
            await administradorUsuarios.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, "Admin"));
            return Ok();
        }

        [HttpPost("QuitarAdministrador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult> QuitarAdministrador([FromBody] string idUsuario) {
            var usuario = await administradorUsuarios.FindByIdAsync(idUsuario);
            await administradorUsuarios.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, "Admin"));
            return Ok();
        }

    }

}