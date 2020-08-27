using Microsoft.EntityFrameworkCore;
using System;

namespace QuizApplication.Contracts.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public Admin Admin { get; set; }
    }
}