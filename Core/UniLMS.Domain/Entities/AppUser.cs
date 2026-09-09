
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
