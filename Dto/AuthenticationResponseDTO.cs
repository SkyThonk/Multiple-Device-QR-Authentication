public record AuthenticationResponseDTO {
    public int UserId {get; init;}

    public string UserName {get; init;} = string.Empty;

    public string Token {get; init;} = string.Empty;
}