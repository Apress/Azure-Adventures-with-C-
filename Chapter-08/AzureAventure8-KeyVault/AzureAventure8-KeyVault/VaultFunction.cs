using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace AzureAventure8_KeyVault
{
    public class VaultFunction
    {
        private readonly ILogger<VaultFunction> _logger;
        private readonly IVaultService _vaultService;

        public VaultFunction(ILogger<VaultFunction> logger, IVaultService vaultService)
        {
            _logger = logger;
            _vaultService = vaultService;
        }

        [Function(nameof(ReadSecret))]
        public async Task<IActionResult> ReadSecret([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            //var secretName = "AdventureSecret";
            //var secret = await _vaultService.GetSecretAsync(secretName);
            //_logger.LogInformation($"Secret: {secret}");

            //await _vaultService.UpdateSecretAsync(secretName, "Adventure");
            //var secretUpdated = await _vaultService.GetSecretAsync(secretName);
            //_logger.LogInformation($"Secret updated with {secretUpdated}");

            //await _vaultService.DeleteSecretAsync(secretName);
            //try
            //{
            //    var secretDeleted = await _vaultService.GetSecretAsync(secretName);
            //    _logger.LogInformation($"Secret deleted: {secretDeleted}");
            //} catch
            //{
            //    _logger.LogError("Secret not found");
            //}

            var certificateName = "adventurecertificate";
            var certificate = await _vaultService.GetCertificateAsync(certificateName);
            _logger.LogInformation($"Certifcate name {certificate.Name}, expiry date {certificate.Properties.NotBefore}");

            var certificateDownloaded = await _vaultService.DownloadCertificateAsync(certificateName);
            _logger.LogInformation($"Certificate2: {certificateDownloaded.Thumbprint}");


            return new OkResult();
        }
    }
}
