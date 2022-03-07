using System.Threading.Tasks;
using TestsDelivery.UserModels.Login;
using TestsDelivery.UserModels.Register;
using TestsDelivery.Infrastructure.User;

namespace TestsDelivery.BL.Services.Users
{
    public interface IUserService
    {
        Task<LoginResult> LoginUser(LoginRequestModel model);

        Task<RegisterResult> RegisterUser(RegisterModelRequestModel model);
    }
}
