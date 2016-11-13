using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.IO;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Jbe.NewsReader.ExternalServices
{
    [Export(typeof(ICryptographicService)), Export, Shared]
    public class CryptographicService : ICryptographicService
    {
        private const uint keySize = 256;

        public Task<Stream> EncryptAsync(Stream stream, string key, string salt, uint iterationCount)
        {
            return RunSymmetricAlgorithmAsync(CryptographicEngine.Encrypt, stream, key, salt, iterationCount);
        }

        public Task<Stream> DecryptAsync(Stream stream, string key, string salt, uint iterationCount)
        {
            return RunSymmetricAlgorithmAsync(CryptographicEngine.Decrypt, stream, key, salt, iterationCount);
        }

        private static async Task<Stream> RunSymmetricAlgorithmAsync(Func<CryptographicKey, IBuffer, IBuffer, IBuffer> algorithm, Stream stream, string key, string salt, uint iterationCount)
        {
            var contentBuffer = await CreateBuffer(stream).ConfigureAwait(false);  // Create first the buffer synchronously
            var derivedKeyAndSalt = await Task.Run(() => DeriveKeyAndSalt(key, salt, iterationCount)).ConfigureAwait(false);  // Ensure that the UI thread is not blocked

            var symmetricAlgorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            var symmetricKey = symmetricAlgorithm.CreateSymmetricKey(derivedKeyAndSalt.Item1);
            var resultBuffer = algorithm(symmetricKey, contentBuffer, derivedKeyAndSalt.Item2);
            return CreateStream(resultBuffer);
        }

        private static Tuple<IBuffer, IBuffer> DeriveKeyAndSalt(string key, string salt, uint iterationCount)
        {
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            var saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf8);

            var pbkdf2Sha512 = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha512);
            var derivedKeyBuffer = CryptographicEngine.DeriveKeyMaterial(pbkdf2Sha512.CreateKey(keyBuffer), KeyDerivationParameters.BuildForPbkdf2(saltBuffer, iterationCount), keySize);
            var derivedSaltBuffer = CryptographicEngine.DeriveKeyMaterial(pbkdf2Sha512.CreateKey(derivedKeyBuffer), KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 8), keySize);
            return new Tuple<IBuffer, IBuffer>(derivedKeyBuffer, derivedSaltBuffer);
        }

        private static async Task<IBuffer> CreateBuffer(Stream stream)
        {
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                return CryptographicBuffer.CreateFromByteArray(MemoryStreamToArrayFast(memoryStream));
            }   
        }

        private static MemoryStream CreateStream(IBuffer buffer)
        {
            byte[] content;
            CryptographicBuffer.CopyToByteArray(buffer, out content);
            return new MemoryStream(content, 0, content.Length, writable: false, publiclyVisible: true);
        }

        private static byte[] MemoryStreamToArrayFast(MemoryStream memoryStream)
        {
            // Use the internal buffer when it has the correct length. Otherwise, use .ToArray() which creates a copy of the buffer.

            byte[] buffer = null;
            ArraySegment<byte> bufferSegment;
            if (memoryStream.TryGetBuffer(out bufferSegment))
            {
                buffer = bufferSegment.Array;
            }
            if (buffer == null || buffer.Length != memoryStream.Length)
            {
                buffer = memoryStream.ToArray();
            }
            return buffer;
        }
    }
}
