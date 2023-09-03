using System.ComponentModel.DataAnnotations;

public record LoginDataRequestDTO {

    [Required]
    [MinLength(5, ErrorMessage = "Invalid Email")]
    public string Email {get; init;} = string.Empty;

    [Required]
    [MinLength(8, ErrorMessage = "Invalid Password")]
    public string Password {get; init;} = string.Empty;

}