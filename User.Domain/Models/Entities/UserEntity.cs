using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.Entities;

public class UserEntity : IdentityUser<int>
{
    //[Key]
    //public int Id { get; set; }
    //[Required]
    //public string Username { get; set; } = string.Empty;
    //[Required]
    //public string Email { get; set; } = string.Empty;
    //[Required]
    //public string PasswordHash { get; set; } = string.Empty;
    //public List<Role> Roles { get; set; } = new List<Role>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
