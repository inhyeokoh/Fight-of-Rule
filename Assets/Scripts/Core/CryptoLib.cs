using System;
using System.Text;

using System.Security.Cryptography;

class CryptoLib
{
    public static Encoding GetEncoding(string encoding = "utf8")
    {
        Encoding encoder;
        switch (encoding)
        {
            case "utf8":
                encoder = Encoding.UTF8;
                break;
            case "utf32":
                encoder = Encoding.UTF32;
                break;
            case "unicode":
            case "utf16":
                encoder = Encoding.Unicode;
                break;
            default:
                encoder = Encoding.UTF8;
                break;
        }

        return encoder;
    }

    public static byte[] StringToBytes(string str, string encoding = "utf8")
    {
        return GetEncoding(encoding).GetBytes(str);
    }

    public static string BytesToString(byte[] bytes, string encoding = "utf8")
    {

        return GetEncoding(encoding).GetString(bytes);
    }

    public static string EncryptBase64(string plain, string encoding = "utf8")
    {
        return Convert.ToBase64String(StringToBytes(plain, encoding));
    }

    public static string DecryptBase64(string cipher, string encoding = "utf8")
    {
        return BytesToString(Convert.FromBase64String(cipher), encoding);
    }

    public static byte[] EncryptSHA256(string plain, string encoding = "utf8")
    {
        using (SHA256Managed sha256 = new SHA256Managed())
        {
            return sha256.ComputeHash(StringToBytes(plain, encoding));
        }
    }

    public static string DecryptSHA256(byte[] cipher, string encoding = "utf8")
    {
        using (SHA256Managed sha256 = new SHA256Managed())
        {
            return BytesToString(sha256.ComputeHash(cipher), encoding);
        }
    }
}
