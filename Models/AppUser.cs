using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{

    public class AppUser:IdentityUser
    {
        [Column(TypeName="nvarchar")]
        [StringLength(400)]
        public string? HomeAddress {get;set;}

        [DataType(DataType.Date)]
        public DateTime? BirthDate{get;set;}  //su dung migration de update CSDL
    }
}