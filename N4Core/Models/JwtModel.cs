#nullable disable

namespace N4Core.Models
{
    public class JwtModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
