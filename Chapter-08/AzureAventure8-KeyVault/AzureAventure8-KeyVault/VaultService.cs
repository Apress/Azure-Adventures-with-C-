using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace AzureAventure8_KeyVault
{
    public interface IVaultService
    {
        Task DeleteSecretAsync(string secretName);
        Task<string> GetSecretAsync(string secretName);
        Task UpdateSecretAsync(string secretName, string secretValue);
        Task<X509Certificate2> GetSecretCertificateAsync(string certificateName);
        Task<KeyVaultCertificateWithPolicy> GetCertificateAsync(string certificateName);
        Task<X509Certificate2> DownloadCertificateAsync(string certificateName);
        Task<KeyVaultCertificateWithPolicy> UploadCertificateAsync(string certificateName, X509Certificate2 certificate);
    }

    public class VaultService : IVaultService
    {
        private readonly string _keyVaultUri;

        public VaultService(IConfiguration configuration)
        {
            _keyVaultUri = configuration["KeyVaultUri"];
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var keyVaultClient = new SecretClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            return (await keyVaultClient.GetSecretAsync(secretName)).Value.Value;
        }

        public async Task DeleteSecretAsync(string secretName)
        {
            var keyVaultClient = new SecretClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            await keyVaultClient.StartDeleteSecretAsync(secretName);
        }

        public async Task UpdateSecretAsync(string secretName, string secretValue)
        {
            var keyVaultClient = new SecretClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            await keyVaultClient.SetSecretAsync(secretName, secretValue);
        }

        public async Task<X509Certificate2> GetSecretCertificateAsync(string certificateName)
        {
            var keyVaultClient = new SecretClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            var secret = await keyVaultClient.GetSecretAsync(certificateName);

            return new X509Certificate2(Convert.FromBase64String(secret.Value.Value));
        }

        public async Task<KeyVaultCertificateWithPolicy> GetCertificateAsync(string certificateName)
        {
            var keyVaultClient = new CertificateClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            var certificate = await keyVaultClient.GetCertificateAsync(certificateName);

            return certificate.Value;
        }

        public async Task<X509Certificate2> DownloadCertificateAsync(string certificateName)
        {
            var keyVaultClient = new CertificateClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            var certificate = await keyVaultClient.DownloadCertificateAsync(certificateName);

            return new X509Certificate2(certificate.Value);
        }

        public async Task<KeyVaultCertificateWithPolicy> UploadCertificateAsync(string certificateName, X509Certificate2 certificate)
        {
            var keyVaultClient = new CertificateClient(new Uri(_keyVaultUri), new DefaultAzureCredential());

            var importCertificateOptions = new ImportCertificateOptions(certificateName, certificate.RawData);

            return await keyVaultClient.ImportCertificateAsync(importCertificateOptions);
        }

    }
}
