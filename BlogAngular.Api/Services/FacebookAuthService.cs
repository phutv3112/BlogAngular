using BlogAngular.Api.Models.Dtos.AuthDtos;
using Newtonsoft.Json;

namespace BlogAngular.Api.Services
{
    public class FacebookAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FacebookAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FacebookUserInfo> VerifyFacebookTokenAsync(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfo>(content);
        }
    }
}
