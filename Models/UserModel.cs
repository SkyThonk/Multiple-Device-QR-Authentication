using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiDeviceQrLogin.Models;

[Index(nameof(Email), IsUnique = true)]
public record UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; init; }

    [Required]
    [MinLength(8)]
    [MaxLength(80)]
    public string Email { get; init;} = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; init;} = string.Empty;

    [MaxLength(50)]
    [MinLength(8)]
    public string LastName { get; init;} = string.Empty;

    [Required]
    public string? PasswordHash { get; init;}

    [Required]
    public string? Salt { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public bool IsDeleted { get; init; } = false;

    public DateTime LastLoginDate { get; init;} = DateTime.UtcNow;

}