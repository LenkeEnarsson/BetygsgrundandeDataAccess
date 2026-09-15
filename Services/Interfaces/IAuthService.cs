using Models.DTO;

namespace Services;

public interface IAuthService
{
    public Task Login(UserLoginDto user);
    public Task SignUp(UserSignUpDto user);
}
