namespace Models.DTO
{
    public record UserSignUpDto<T>
    {
        public required string Email { get; init; }
        public required string Username { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Password { get; init; }
    }
    public record UserSignUpDto()
    {
#if DEBUG
        //Only used in debug mode to show the connection string
        public string ConnectionString { get; init; }
#endif

    }
}
