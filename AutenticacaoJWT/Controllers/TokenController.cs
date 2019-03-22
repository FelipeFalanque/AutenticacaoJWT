using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AutenticacaoJwt.Controllers
{
    [Route("/token")]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Create(string username, string password)
        {
               //valida login
               if(CombinaSenhaNomeValido(username,password))
                   return new ObjectResult(GeraToken(username));

              //status code 400
              return BadRequest();     
        }

        private object GeraToken(string username)
        {
            //claims que vão compor o payload
            //definimos: username, notBefore e expires
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            //Token = header + payload(claims)  + signature(chave simétrica+segredo)
            // 
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                              new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minhachavesecreta")),SecurityAlgorithms.HmacSha256)),
                              new JwtPayload(claims));
            
            //gera o token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool CombinaSenhaNomeValido(string username, string password)
        {
            //senha deve ser igual ao nome do usuário para validar o login
            return !string.IsNullOrEmpty(username) && username == password;
        }
    }
}