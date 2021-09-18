using System.Threading.Tasks;
using TestsDelivery.DataTransferObjects;
using TestsDelivery.Infrastructure.User;

namespace TestsDelivery.Services
{
    public interface IUserService
    {
        Task<LoginResult> LoginUser(LoginRequestDto model);

        Task<RegisterResult> RegisterUser(RegisterModelRequestDto model);
    }
}
