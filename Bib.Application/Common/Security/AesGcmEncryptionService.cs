using System;
using System.Security.Cryptography;
using System.Text;

namespace SeuProjeto.Security
{
    /// <summary>
    /// Serviço de criptografia usando AES-GCM (recomendado para .NET 6+)
    /// </summary>
    public class AesGcmEncryptionService : IDisposable
    {
        private readonly byte[] _key;
        private bool _disposed = false;

        /// <summary>
        /// Tamanhos padrão recomendados
        /// </summary>
        public static class Sizes
        {
            public const int KeySize = 32; // AES-256 (32 bytes = 256 bits)
            public const int NonceSize = 12; // 12 bytes para GCM (recomendado)
            public const int TagSize = 16; // 16 bytes para autenticação
        }

        /// <summary>
        /// Cria uma nova instância com uma chave existente
        /// </summary>
        /// <param name="key">Chave de 32 bytes (256 bits)</param>
        public AesGcmEncryptionService(byte[] key)
        {
            if (key == null || key.Length != Sizes.KeySize)
                throw new ArgumentException($"A chave deve ter {Sizes.KeySize} bytes (256 bits)");

            _key = (byte[])key.Clone();
        }

        /// <summary>
        /// Gera uma nova chave aleatória
        /// </summary>
        public static byte[] GenerateKey()
        {
            var key = new byte[Sizes.KeySize];
            RandomNumberGenerator.Fill(key);
            return key;
        }

        /// <summary>
        /// Criptografa texto plano
        /// </summary>
        public EncryptedResult Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException(nameof(plaintext));

            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Encrypt(plaintextBytes);
        }

        /// <summary>
        /// Criptografa bytes
        /// </summary>
        public EncryptedResult Encrypt(byte[] plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                throw new ArgumentNullException(nameof(plaintext));

            // Gera nonce (IV) aleatório
            var nonce = new byte[Sizes.NonceSize];
            RandomNumberGenerator.Fill(nonce);

            // Prepara arrays para resultado
            var ciphertext = new byte[plaintext.Length];
            var tag = new byte[Sizes.TagSize];

            // Executa criptografia
            using (var aesGcm = new AesGcm(_key))
            {
                aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);
            }

            return new EncryptedResult
            {
                Ciphertext = ciphertext,
                Nonce = nonce,
                Tag = tag
            };
        }

        /// <summary>
        /// Descriptografa para texto
        /// </summary>
        public string DecryptToString(EncryptedResult encrypted)
        {
            var bytes = Decrypt(encrypted);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Descriptografa bytes
        /// </summary>
        public byte[] Decrypt(EncryptedResult encrypted)
        {
            if (encrypted == null)
                throw new ArgumentNullException(nameof(encrypted));

            ValidateEncryptedData(encrypted);

            var plaintext = new byte[encrypted.Ciphertext.Length];

            using (var aesGcm = new AesGcm(_key))
            {
                aesGcm.Decrypt(
                    encrypted.Nonce,
                    encrypted.Ciphertext,
                    encrypted.Tag,
                    plaintext
                );
            }

            return plaintext;
        }

        /// <summary>
        /// Valida dados criptografados
        /// </summary>
        private void ValidateEncryptedData(EncryptedResult encrypted)
        {
            if (encrypted.Ciphertext == null || encrypted.Ciphertext.Length == 0)
                throw new ArgumentException("Ciphertext inválido");

            if (encrypted.Nonce == null || encrypted.Nonce.Length != Sizes.NonceSize)
                throw new ArgumentException($"Nonce deve ter {Sizes.NonceSize} bytes");

            if (encrypted.Tag == null || encrypted.Tag.Length != Sizes.TagSize)
                throw new ArgumentException($"Tag deve ter {Sizes.TagSize} bytes");
        }

        /// <summary>
        /// Versão com strings Base64 para armazenamento fácil
        /// </summary>
        public StringEncryptedResult EncryptToBase64(string plaintext)
        {
            var result = Encrypt(plaintext);
            return new StringEncryptedResult
            {
                Ciphertext = Convert.ToBase64String(result.Ciphertext),
                Nonce = Convert.ToBase64String(result.Nonce),
                Tag = Convert.ToBase64String(result.Tag)
            };
        }

        /// <summary>
        /// Descriptografa a partir de Base64
        /// </summary>
        public string DecryptFromBase64(StringEncryptedResult encrypted)
        {
            var binaryResult = new EncryptedResult
            {
                Ciphertext = Convert.FromBase64String(encrypted.Ciphertext),
                Nonce = Convert.FromBase64String(encrypted.Nonce),
                Tag = Convert.FromBase64String(encrypted.Tag)
            };

            return DecryptToString(binaryResult);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Limpa a chave da memória
                    Array.Clear(_key, 0, _key.Length);
                }
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Resultado em formato binário
    /// </summary>
    public class EncryptedResult
    {
        public byte[] Ciphertext { get; set; } = Array.Empty<byte>();
        public byte[] Nonce { get; set; } = Array.Empty<byte>();
        public byte[] Tag { get; set; } = Array.Empty<byte>();
    }

    /// <summary>
    /// Resultado em formato string (Base64)
    /// </summary>
    public class StringEncryptedResult
    {
        public string Ciphertext { get; set; } = string.Empty;
        public string Nonce { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
    }
}