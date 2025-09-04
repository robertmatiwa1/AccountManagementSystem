using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementSystem.Models;

[Index("id_number", Name = "IX_Person_id", IsUnique = true)]
public partial class Person
{
    [Key]
    public int code { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Display(Name = "First Name")]
    public string? name { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "Surname is required")]
    [Display(Name = "Surname")]
    public string? surname { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "ID Number is required")]
    [Display(Name = "ID Number")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ID Number must be 13 digits")]
    public string id_number { get; set; } = null!;

    [InverseProperty("person_codeNavigation")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}