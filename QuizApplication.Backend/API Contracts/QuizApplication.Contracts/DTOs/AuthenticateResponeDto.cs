using QuizApplication.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace QuizApplication.Contracts.DTOs
{
    public class AuthenticateResponseDto
    {
        public int Id { get; set; }

        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponseDto(Admin admin, string jwtToken, string refreshToken)
        {
            Id = admin.Id;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
