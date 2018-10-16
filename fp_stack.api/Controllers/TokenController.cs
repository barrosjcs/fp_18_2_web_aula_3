﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using fp_stack.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace fp_stack.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody]TokenInfo model)
        {
            if (IsValidUserAndPasswordCombination(model.username, model.password))
            {
                var token = GenerateToken(model.username);
                //Salvar no DB
                return new ObjectResult(token);

            }
            return BadRequest();
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var text = Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256");
            var key = new SymmetricSecurityKey(text);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var payload = new JwtPayload(claims);
            var header = new JwtHeader(signingCredentials);
            var token = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        private bool IsValidUserAndPasswordCombination(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && username == password;
        }
    }
}