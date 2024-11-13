using Google.Apis.Auth;

namespace BlogAngular.Api.Services
{
    public class GoogleAuthService
    {
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload;
            }
            catch (Exception ex)
            {
                // Handle validation failure
                throw new Exception("Invalid Google token", ex);
            }
        }
    }
}
