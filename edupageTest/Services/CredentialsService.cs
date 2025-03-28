//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Security;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Windows;
//using System.IO;

//namespace edupageTest.Services
//{
//    public static class CredentialsService
//    {
//        private static readonly string AppDataPath = Path.Combine(
//            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
//            "EdupageTest");

//        private static readonly string CredentialsPath = Path.Combine(AppDataPath, "credentials.dat");
//        private static readonly byte[] EncryptionEntropy = Encoding.UTF8.GetBytes("YourAppSpecificSalt123!");

//        private class CredentialData
//        {
//            public byte[] EncryptedUsername { get; set; }
//            public string PasswordHash { get; set; }
//            public byte[] Salt { get; set; }
//        }

//        public static void SaveCredentials(string username, SecureString password, bool rememberMe)
//        {
//            try
//            {
//                Directory.CreateDirectory(AppDataPath);

//                if (!rememberMe)
//                {
//                    ClearCredentials();
//                    return;
//                }

//                // Generate a random salt
//                var salt = new byte[32];
//                using (var rng = RandomNumberGenerator.Create())
//                {
//                    rng.GetBytes(salt);
//                }

//                var data = new CredentialData
//                {
//                    EncryptedUsername = EncryptString(username),
//                    PasswordHash = HashPassword(password, salt),
//                    Salt = salt
//                };

//                File.WriteAllText(CredentialsPath, JsonSerializer.Serialize(data));
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Failed to save credentials: {ex.Message}");
//            }
//            finally
//            {
//                password?.Dispose();
//            }
//        }

//        public static (string username, bool rememberMe) LoadCredentials()
//        {
//            try
//            {
//                if (!File.Exists(CredentialsPath)) return (null, false);

//                var json = File.ReadAllText(CredentialsPath);
//                var data = JsonSerializer.Deserialize<CredentialData>(json);

//                return data == null
//                    ? (null, false)
//                    : (DecryptString(data.EncryptedUsername), true);
//            }
//            catch
//            {
//                return (null, false);
//            }
//        }

//        public static bool VerifyCredentials(SecureString enteredPassword)
//        {
//            try
//            {
//                if (!File.Exists(CredentialsPath)) return false;

//                var json = File.ReadAllText(CredentialsPath);
//                var data = JsonSerializer.Deserialize<CredentialData>(json);

//                if (data == null) return false;

//                var enteredHash = HashPassword(enteredPassword, data.Salt);
//                return CryptographicEquals(
//                    Convert.FromBase64String(data.PasswordHash),
//                    enteredHash);
//            }
//            catch
//            {
//                return false;
//            }
//            finally
//            {
//                enteredPassword?.Dispose();
//            }
//        }

//        public static void ClearCredentials()
//        {
//            try
//            {
//                if (File.Exists(CredentialsPath))
//                {
//                    File.Delete(CredentialsPath);
//                }
//            }
//            catch { /* Ignore deletion errors */ }
//        }

//        private static string HashPassword(SecureString password, byte[] salt)
//        {
//            using var pbkdf2 = new Rfc2898DeriveBytes(
//                SecureStringToString(password),
//                salt,
//                100000,
//                HashAlgorithmName.SHA512);

//            return Convert.ToBase64String(pbkdf2.GetBytes(64));
//        }

//        private static byte[] EncryptString(string input)
//        {
//            var bytes = Encoding.UTF8.GetBytes(input);
//            return ProtectedData.Protect(bytes, EncryptionEntropy, DataProtectionScope.CurrentUser);
//        }

//        private static string DecryptString(byte[] encryptedData)
//        {
//            var bytes = ProtectedData.Unprotect(encryptedData, EncryptionEntropy, DataProtectionScope.CurrentUser);
//            return Encoding.UTF8.GetString(bytes);
//        }

//        private static bool CryptographicEquals(byte[] a, byte[] b)
//        {
//            if (a == null || b == null || a.Length != b.Length)
//                return false;

//            var result = 0;
//            for (int i = 0; i < a.Length; i++)
//                result |= a[i] ^ b[i];

//            return result == 0;
//        }

//        private static string SecureStringToString(SecureString secureString)
//        {
//            IntPtr ptr = IntPtr.Zero;
//            try
//            {
//                ptr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(secureString);
//                return System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
//            }
//            finally
//            {
//                if (ptr != IntPtr.Zero)
//                    System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(ptr);
//            }
//        }
//    }
//}
