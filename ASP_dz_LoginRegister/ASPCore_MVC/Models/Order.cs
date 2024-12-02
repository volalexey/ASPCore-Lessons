using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPCore_MVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } = 0;

        [DefaultValue(1)]
        public int Count { get; set; } = 1;

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; } = "";
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        
        [ForeignKey("Products")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
