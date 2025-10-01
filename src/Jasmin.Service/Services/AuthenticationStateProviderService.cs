using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jasmin.Service.Services
{
    public class AuthenticationStateProviderService : AuthenticationStateProvider
    {

        private ClaimsPrincipal _user;


        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(_user ?? new ClaimsPrincipal(new ClaimsIdentity()));
            return Task.FromResult(state);
        }

        public void Logout()
        {
            // Логика для обнуления claims
            NotifyAuthenticationStateChanged(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyAuthenticationStateChanged(ClaimsPrincipal user)
        {
            _user = user;
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        // Используйте ключевое слово new для явного указания на скрытие метода
        public new void NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
        {
             base.NotifyAuthenticationStateChanged(task);
       }
        

    }
}