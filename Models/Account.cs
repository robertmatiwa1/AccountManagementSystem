using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementSystem.Models;

[Index("account_number", Name = "IX_Account_num", IsUnique = true)]
public partial class Account
{
    [Key]
    public int code { get; set; }

    [Required(ErrorMessage = "Person is required")]
    [Display(Name = "Person")]
    public int person_code { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Required(ErrorMessage = "Account Number is required")]
    [Display(Name = "Account Number")]
    public string account_number { get; set; } = null!;

    [Column(TypeName = "money")]
    [Required(ErrorMessage = "Balance is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
    [Display(Name = "Outstanding Balance")]
    public decimal outstanding_balance { get; set; }

    [Display(Name = "Is Closed")]
    public bool is_closed { get; set; }

    [InverseProperty("account_codeNavigation")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("person_code")]
    [InverseProperty("Accounts")]
    public virtual Person person_codeNavigation { get; set; } = null!;
}