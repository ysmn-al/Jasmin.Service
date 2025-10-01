using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using System.Threading.Tasks;

namespace Jasmin.Service.Services
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUser(LoginDto model);

        Task<TeacherDto> CurrentUser(LoginDto model);
    }
}
