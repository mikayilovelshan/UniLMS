using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Auth
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }
    }
}
