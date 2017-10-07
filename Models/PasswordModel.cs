using System;
using System.Security.Cryptography;


/**
	Source: 
	http://csharptest.net/470/another-example-of-how-to-store-a-salted-password-hash/

	Issues: 
	* First it limits you to the use of SHA1 (and thus 20 bytes) hash.
	* If you try to use more than 20 bytes this can potentially fail to generate the same data
	* Your not in control of the HMAC hash algorithm used, and thus rely on .Net to select the SHA1 implementation based on host configuration.
	* I dislike the fact that Rfc2898DeriveBytes did not implement Dispose and call the HMAC’s Dispose method. I guess this was just an oversight
		on Microsoft’s part. It effectively leaves the plain-text password in memory until the GC collects i
 */
namespace dotNetflix.Models
{
	public sealed class PasswordHash
	{
		const int SaltSize = 16, HashSize = 20, HashIter = 10000;
		readonly byte[] _salt, _hash;
		public PasswordHash(string password)
		{
			new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
			_hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
		}
		public PasswordHash(byte[] hashBytes)
		{
			Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
			Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
		}
		public PasswordHash(byte[] salt, byte[] hash)
		{
			Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
			Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
		}
		public byte[] ToArray()
		{
			byte[] hashBytes = new byte[SaltSize + HashSize];
			Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
			Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
			return hashBytes;
		}
		public byte[] Salt { get { return (byte[])_salt.Clone(); } }
		public byte[] Hash { get { return (byte[])_hash.Clone(); } }
		public bool Verify(string password)
		{
			byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
			for (int i = 0; i < HashSize; i++)
				if (test[i] != _hash[i])
					return false;
			return true;
		}
	}
}