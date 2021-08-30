using System.Threading.Tasks;
using TestsDelivery.Infrastructure.User;
using TestsDelivery.Models.Identity;

namespace TestsDelivery.Services
{
    public interface IUserService
    {
        Task<LoginResult> LoginUser(LoginModel model);

        Task<RegisterResult> RegisterUser(RegisterModel model);
    }
}
