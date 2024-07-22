using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureAdventure8_KeyVault
{
    public interface IKeyVaultService
    {
        SecretClient GetSecretClient();
    }

    public class KeyVaultService : IKeyVaultService
    {
        public SecretClient GetSecretClient()
        {
            return new SecretClient(new Uri("https://adventurevault.vault.azure.net/"), new DefaultAzureCredential());
        }
    }
}
