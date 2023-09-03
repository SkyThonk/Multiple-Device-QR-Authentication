public record UserDetailsResponseDTO{
    public int UserId {get; init;}
    public string FirstName {get;init;} = string.Empty;

    public string LastName {get;init;} = string.Empty;
}