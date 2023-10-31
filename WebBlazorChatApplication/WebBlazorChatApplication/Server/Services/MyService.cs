using Microsoft.AspNetCore.Components.Authorization;

namespace WebBlazorChatApplication.Server.Services
{
    public class MyService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public MyService(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<AuthenticationState> GetAuthenticationState()
        {
            return await authenticationStateProvider.GetAuthenticationStateAsync();
        }
    }
}
