using System.ComponentModel.DataAnnotations;

public record RegisterDataRequestDTO
{
    [Required]
    [MinLength(8)]
    [MaxLength(80)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(80)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; init; } = string.Empty;

    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; init; } = string.Empty;
}