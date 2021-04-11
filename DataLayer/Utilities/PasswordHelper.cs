using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataLayer.ViewModels
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            SHA256 sha256;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
            sha256 = new SHA256CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = sha256.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }


    }
}
