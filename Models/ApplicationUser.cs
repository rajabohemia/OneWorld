using System;
using Microsoft.AspNetCore.Identity;

namespace OneWorld.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateStamp { get; set; }
    }
}