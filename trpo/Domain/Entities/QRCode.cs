using System;

namespace QRAttendance.Domain.Entities
{
    public class QRCode
    {
        public string Token { get; }
        public DateTime IssuedAt { get; }
        public DateTime ExpiresAt { get; }

        public QRCode(string token, DateTime issuedAt, DateTime expiresAt)
        {
            Token = token;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
        }

        public bool IsValid(DateTime currentTime)
        {
            return currentTime >= IssuedAt && currentTime <= ExpiresAt;
        }
    }
}
