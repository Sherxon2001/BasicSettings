using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.Models
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long AuthDateUnix => ((DateTimeOffset)ExpirationDate).ToUnixTimeSeconds();
        public CustomeIdentityUserDto User { get; set; }
    }
}
