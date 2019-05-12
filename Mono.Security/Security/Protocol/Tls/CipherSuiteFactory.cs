﻿using System;

namespace Mono.Security.Protocol.Tls
{
	internal class CipherSuiteFactory
	{
		public static CipherSuiteCollection GetSupportedCiphers(SecurityProtocolType protocol)
		{
			if (protocol != SecurityProtocolType.Default)
			{
				if (protocol != SecurityProtocolType.Ssl2)
				{
					if (protocol == SecurityProtocolType.Ssl3)
					{
						return CipherSuiteFactory.GetSsl3SupportedCiphers();
					}
					if (protocol == SecurityProtocolType.Tls)
					{
						goto IL_2D;
					}
				}
				throw new NotSupportedException("Unsupported security protocol type");
			}
			IL_2D:
			return CipherSuiteFactory.GetTls1SupportedCiphers();
		}

		private static CipherSuiteCollection GetTls1SupportedCiphers()
		{
			return new CipherSuiteCollection(SecurityProtocolType.Tls)
			{
				{
					53,
					"TLS_RSA_WITH_AES_256_CBC_SHA",
					CipherAlgorithmType.Rijndael,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					32,
					32,
					256,
					16,
					16
				},
				{
					47,
					"TLS_RSA_WITH_AES_128_CBC_SHA",
					CipherAlgorithmType.Rijndael,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					16,
					16,
					128,
					16,
					16
				},
				{
					10,
					"TLS_RSA_WITH_3DES_EDE_CBC_SHA",
					CipherAlgorithmType.TripleDes,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					24,
					24,
					168,
					8,
					8
				},
				{
					5,
					"TLS_RSA_WITH_RC4_128_SHA",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					false,
					16,
					16,
					128,
					0,
					0
				},
				{
					4,
					"TLS_RSA_WITH_RC4_128_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					false,
					16,
					16,
					128,
					0,
					0
				},
				{
					9,
					"TLS_RSA_WITH_DES_CBC_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					8,
					8,
					56,
					8,
					8
				},
				{
					3,
					"TLS_RSA_EXPORT_WITH_RC4_40_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					5,
					16,
					40,
					0,
					0
				},
				{
					6,
					"TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5",
					CipherAlgorithmType.Rc2,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					5,
					16,
					40,
					8,
					8
				},
				{
					8,
					"TLS_RSA_EXPORT_WITH_DES40_CBC_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					5,
					8,
					40,
					8,
					8
				},
				{
					96,
					"TLS_RSA_EXPORT_WITH_RC4_56_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					7,
					16,
					56,
					0,
					0
				},
				{
					97,
					"TLS_RSA_EXPORT_WITH_RC2_CBC_56_MD5",
					CipherAlgorithmType.Rc2,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					7,
					16,
					56,
					8,
					8
				},
				{
					98,
					"TLS_RSA_EXPORT_WITH_DES_CBC_56_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					8,
					8,
					64,
					8,
					8
				},
				{
					100,
					"TLS_RSA_EXPORT_WITH_RC4_56_SHA",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					7,
					16,
					56,
					0,
					0
				}
			};
		}

		private static CipherSuiteCollection GetSsl3SupportedCiphers()
		{
			return new CipherSuiteCollection(SecurityProtocolType.Ssl3)
			{
				{
					53,
					"SSL_RSA_WITH_AES_256_CBC_SHA",
					CipherAlgorithmType.Rijndael,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					32,
					32,
					256,
					16,
					16
				},
				{
					10,
					"SSL_RSA_WITH_3DES_EDE_CBC_SHA",
					CipherAlgorithmType.TripleDes,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					24,
					24,
					168,
					8,
					8
				},
				{
					5,
					"SSL_RSA_WITH_RC4_128_SHA",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					false,
					16,
					16,
					128,
					0,
					0
				},
				{
					4,
					"SSL_RSA_WITH_RC4_128_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					false,
					16,
					16,
					128,
					0,
					0
				},
				{
					9,
					"SSL_RSA_WITH_DES_CBC_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					false,
					true,
					8,
					8,
					56,
					8,
					8
				},
				{
					3,
					"SSL_RSA_EXPORT_WITH_RC4_40_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					5,
					16,
					40,
					0,
					0
				},
				{
					6,
					"SSL_RSA_EXPORT_WITH_RC2_CBC_40_MD5",
					CipherAlgorithmType.Rc2,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					5,
					16,
					40,
					8,
					8
				},
				{
					8,
					"SSL_RSA_EXPORT_WITH_DES40_CBC_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					5,
					8,
					40,
					8,
					8
				},
				{
					96,
					"SSL_RSA_EXPORT_WITH_RC4_56_MD5",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					7,
					16,
					56,
					0,
					0
				},
				{
					97,
					"SSL_RSA_EXPORT_WITH_RC2_CBC_56_MD5",
					CipherAlgorithmType.Rc2,
					HashAlgorithmType.Md5,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					7,
					16,
					56,
					8,
					8
				},
				{
					98,
					"SSL_RSA_EXPORT_WITH_DES_CBC_56_SHA",
					CipherAlgorithmType.Des,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					true,
					8,
					8,
					64,
					8,
					8
				},
				{
					100,
					"SSL_RSA_EXPORT_WITH_RC4_56_SHA",
					CipherAlgorithmType.Rc4,
					HashAlgorithmType.Sha1,
					ExchangeAlgorithmType.RsaKeyX,
					true,
					false,
					7,
					16,
					56,
					0,
					0
				}
			};
		}
	}
}
