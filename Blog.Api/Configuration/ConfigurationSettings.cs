namespace Blog_Api.Configuration;

public class ConfigurationSettings
{
        
    public static IConfigurationRoot GetKeyVaultConfiguration(IConfigurationRoot builtConfig)
    {
        return GetConfigurationWithMsi(builtConfig) ?? GetConfigurationWithCredentials(builtConfig);
    }

    private static IConfigurationRoot GetConfigurationWithCredentials(IConfigurationRoot builtRootConfig)
    {
        try
        {
            return new ConfigurationBuilder()
                .AddAzureKeyVault(builtRootConfig["KeyVault:Url"], builtRootConfig["KeyVault:ClientId"],
                    builtRootConfig["KeyVault:ClientSecret"])
                .Build();
        }
        catch (Exception)
        {
            throw new Exception("Cannot find configuration for Key Vault.");
        }
    }

    private static IConfigurationRoot GetConfigurationWithMsi(IConfigurationRoot builtRootConfig)
    {
#if DEBUG
        return null;
#else
            try
            {
                var keyVaultConfigBuilder = new ConfigurationBuilder();
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient((authority, resource, scope) => azureServiceTokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                keyVaultConfigBuilder.AddAzureKeyVault(builtRootConfig["KeyVault:Url"], keyVaultClient, new DefaultKeyVaultSecretManager());
                return keyVaultConfigBuilder.Build();
            }
            catch (Exception)
            {
                throw new Exception("Cannot find configuration for Key Vault.");
            }
#endif
    }
        
        
}