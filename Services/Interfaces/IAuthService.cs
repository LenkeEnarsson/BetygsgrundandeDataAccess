using Models.DTO;

namespace Services;

public interface IAuthService
{
    public Task Login(UserSignUpDto user);
    public Task SignUp(UserSignUpDto user);
}
