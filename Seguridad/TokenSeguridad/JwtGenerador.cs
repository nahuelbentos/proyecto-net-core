using System.Collections.Generic;
using System.Security.Claims;
using Aplicacion.Contratos;
using Dominio;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Seguridad
{
  public class JwtGenerador : IJwtGenerador
  {
    public string CrearToken(Usuario usuario, List<string> roles)
    {
      // se puede agregar los claims que se quieran.
      var claims = new List<Claim>{
        new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
      };

      if (roles != null)
      {
        foreach (var role in roles)
        {
          claims.Add(new Claim(ClaimTypes.Role, role));
        }
      }


      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

      var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescripcion = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(30),
        SigningCredentials = credenciales
      };

      var tokenManejador = new JwtSecurityTokenHandler();
      var token = tokenManejador.CreateToken(tokenDescripcion);

      return tokenManejador.WriteToken(token);

    }
  }
}