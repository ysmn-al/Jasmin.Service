using System.Threading.Tasks;
using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Service.Clients.Jasmin.Host.Client;
using Microsoft.Extensions.Logging;

namespace Jasmin.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJasminClient _jasminClient;
    private readonly ILogger<ApplicationService> _logger;


    public AuthenticationService(IJasminClient jasminClient, ILogger<ApplicationService> logger)
    {
        _jasminClient = jasminClient;
        _logger = logger;
    }

    public async Task<bool> ValidateUser(LoginDto model)
    {

        var user = await _jasminClient.GetTeacherByLogin(model);

        if (user != null /*&& BCrypt.Net.BCrypt.Verify(model.Password, user.Password)*/)
        {
            return true;
        }

        return false;
    }

    public async Task<TeacherDto> CurrentUser(LoginDto model)
    {

        var user = await _jasminClient.GetTeacherByLogin(model);

        if (user != null /*&& BCrypt.Net.BCrypt.Verify(model.Password, user.Password)*/)
        {
            return user;
        }

        return null;
    }

}
