using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using Sitecore.Plugin.IdentityProvider.Facebook.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sitecore.Plugin.IdentityProvider.Facebook
{
    public class ConfigureSitecore
    {
        private readonly ILogger<ConfigureSitecore> _logger;
        private readonly AppSettings _appSettings;

        public ConfigureSitecore(ISitecoreConfiguration scConfig, ILogger<ConfigureSitecore> logger)
        {
            _logger = logger;
            _appSettings = new AppSettings();
            scConfig.GetSection(AppSettings.SectionName).Bind(_appSettings.FacebookIdentityProvider);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityProvider = _appSettings.FacebookIdentityProvider;
            if (!identityProvider.Enabled)
                return;
            _logger.LogDebug("Configure '" + identityProvider.DisplayName + "'. AuthenticationScheme = " + identityProvider.AuthenticationScheme + ", MetadataAddress = " + identityProvider.AppId + ", Wtrealm = " + identityProvider.AppSecret, Array.Empty<object>());
            new AuthenticationBuilder(services).AddFacebook(identityProvider.AuthenticationScheme, identityProvider.DisplayName, (Action<FacebookOptions>)(options =>
            {
                options.SignInScheme = "idsrv.external";
                options.AppId = identityProvider.AppId;
                options.AppSecret = identityProvider.AppSecret;
            }));
        }
    }
}
