﻿using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Security.Claims;

namespace gather_standalone.Properties
{
    internal class Jwt
    {
        public static string Create(Guid guid, string dataString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim( "guid", guid.ToString() ),
                    new Claim( "post", dataString)
                }),
                Expires = now.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["jwtSecret"])), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
