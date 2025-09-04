using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementSystem.Models;

public partial class Transaction
{
    [Key]
    public int code { get; set; }

    [Required(ErrorMessage = "Account is required")]
    [Display(Name = "Account")]
    public int account_code { get; set; }

    [Required(ErrorMessage = "Transaction Date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Transaction Date")]
    public DateTime transaction_date { get; set; }

    [Required(ErrorMessage = "Capture Date is required")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Capture Date")]
    public DateTime capture_date { get; set; }

    [Column(TypeName = "money")]
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Display(Name = "Amount")]
    public decimal amount { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description")]
    public string description { get; set; } = null!;

    [ForeignKey("account_code")]
    [InverseProperty("Transactions")]
    public virtual Account account_codeNavigation { get; set; } = null!;
}