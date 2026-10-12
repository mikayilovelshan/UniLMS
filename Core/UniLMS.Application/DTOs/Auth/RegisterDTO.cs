using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniLMS.Application.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Şifrələr bir-biri ilə eyni deyil")]
        public string ConfirmPassword { get; set; }

    }
}
