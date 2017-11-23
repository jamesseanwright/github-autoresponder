using System;
using System.Security.Cryptography;
using System.Text;

namespace GitHubAutoresponder.Webhook {
    public class RequestValidator : IRequestValidator {
        private string ConvertRawBytesToHexString(byte[] bytes) {
            return string.Join(
                string.Empty,
                Array.ConvertAll<byte, string>(bytes, b => b.ToString("x2"))
            );
        }

        public bool IsValidRequest(string expectedSignature, string key, string payload) {
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(key))) {
                byte[] rawPayload = Encoding.ASCII.GetBytes(payload);
                byte[] rawHash = hmac.ComputeHash(rawPayload);
                string hash = this.ConvertRawBytesToHexString(rawHash);
                string signature = $"sha1={hash}";

                return signature == expectedSignature; // TODO: constant-time comparison
            }
        }
    }
}
