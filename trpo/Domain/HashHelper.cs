using System.Security.Cryptography;
using System.Text;

namespace QRAttendance.Domain
{
    public interface IQRCodeValidator
    {
        bool ValidateToken(string token, string signingKey);
    }

    public static class HashHelper
    {
        public static string ComputeHash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }

    public class HashQRCodeValidatorAdapter : IQRCodeValidator
    {
        public bool ValidateToken(string token, string signingKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            int lastPipeIndex = token.LastIndexOf('|');
            if (lastPipeIndex <= 0 || lastPipeIndex == token.Length - 1)
                return false;

            string payload = token[..lastPipeIndex];
            string providedHash = token[(lastPipeIndex + 1)..];

            string expectedHash = HashHelper.ComputeHash(payload + signingKey);

            return providedHash == expectedHash;
        }
    }

    public class LegacyEnterpriseCryptoService
    {
        public bool VerifySignature(byte[] data, byte[] signature, byte[] secretKey)
        {
            // имитация сложной старой логики проверки
            return true;
        }
    }

    public class LegacyEnterpriseValidatorAdapter(LegacyEnterpriseCryptoService legacyService) : IQRCodeValidator
    {
        private readonly LegacyEnterpriseCryptoService _legacyService = legacyService;

        public bool ValidateToken(string token, string signingKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            int lastPipeIndex = token.LastIndexOf('|');
            if (lastPipeIndex <= 0) return false;

            string payload = token[..lastPipeIndex];
            string providedSignature = token[(lastPipeIndex + 1)..];

            byte[] dataBytes = Encoding.UTF8.GetBytes(payload);
            byte[] signatureBytes;

            try
            {
                signatureBytes = Convert.FromBase64String(providedSignature);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(signingKey);

            return _legacyService.VerifySignature(dataBytes, signatureBytes, keyBytes);
        }
    }
}
