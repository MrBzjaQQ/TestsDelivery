using System.Threading.Tasks;
using TestsDelivery.BL.Models;
using TestsDelivery.Infrastructure.User;

namespace TestsDelivery.BL.Services.Users
{
    public interface IUserService
    {
        Task<LoginResult> LoginUser(LoginRequestModel model);

        Task<RegisterResult> RegisterUser(RegisterModelRequestModel model);
    }
}
