using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public record UserLoginDto<T>
    {
        public string Email { get; init; }
        [Required(ErrorMessage = "Username is needed")] 
        public string Username { get; init; }
        [Required(ErrorMessage = "Password is needed")] 
        public required string Password { get; init; }
    }
    public record UserLoginDto()
    {
#if DEBUG
        //Only used in debug mode to show the connection string
        public string ConnectionString { get; init; }
#endif
    }
}
