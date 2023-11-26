using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using MongoDB.Driver;
using Mug.Services.CosmosDb;

namespace Mug.Extensions
{
    public static class SeviceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDbService(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["AZURE_COSMOS_CONNECTIONSTRING"];
            var databaseName = config["AZURE_COSMOS_DATABASE"];

            if (connectionString == null) throw new ArgumentException("AZURE_COSMOS_CONNECTIONSTRING not configured.");
            if (databaseName == null) throw new ArgumentException("AZURE_COSMOS_DATABASE not configured.");

            services.AddSingleton(new CosmosDbService(connectionString, databaseName));

            return services;
        }

        public static IServiceCollection AddAzureAdB2CAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var instance = config["AZURE_AD_B2C_INSTANCE"];
            var clientId = config["AZURE_AD_B2C_CLIENTID"];
            var domain = config["AZURE_AD_B2C_DOMAIN"];
            var signedOutCallbackPath = config["AZURE_AD_B2C_SIGNEDOUTCALLBACKPATH"];
            var signUpSignInPolicyId = config["AZURE_AD_B2C_SIGNUPSIGNINPOLICY"];
            var resetPasswordPolicyId = config["AZURE_AD_B2C_RESETPASSWORDPOLICYID"];
            var editProfilePolicyId = config["AZURE_AD_B2C_EDITPROFILEPOLICYID"];
            var clientSecret = config["AZURE_AD_B2C_CLIENTSECRET"];
            var callbackPath = config["AZURE_AD_B2C_CALLBACKPATH"];

            if (instance == null || 
                clientId == null || 
                domain == null || 
                signedOutCallbackPath == null ||
                signUpSignInPolicyId == null ||
                resetPasswordPolicyId == null ||
                editProfilePolicyId == null ||
                clientSecret == null ||
                callbackPath == null)
            {
                throw new ArgumentException("AZURE_AD_B2C not configured.");
            }

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(options =>
                    {
                        config.Bind("AzureAdB2C", options);
                        options.Instance = instance;
                        options.ClientId = clientId;
                        options.Domain = domain;
                        options.SignedOutCallbackPath = signedOutCallbackPath;
                        options.SignUpSignInPolicyId = signUpSignInPolicyId; // Default policy
                        options.ResetPasswordPolicyId = resetPasswordPolicyId;
                        options.EditProfilePolicyId = editProfilePolicyId;
                        options.ClientSecret = clientSecret;
                        options.CallbackPath = callbackPath;
                    });

            return services;
        }
    }
}