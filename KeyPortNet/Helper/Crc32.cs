﻿using System;
using System.Text;
using System.Security.Cryptography;


/// <summary>
/// Berechnung einer CRC32 Checksumme
/// </summary>
public class Crc32 : HashAlgorithm
{
	/// <summary>
	/// Checksumme über einen String bilden.
	/// </summary>
    /// <param name="s">string s</param>
	/// <returns>string Checksumme</returns>
    public static string CRC32OfString(string s)
	{
		byte[] original;
		byte[] encoded;

		Crc32 crc32 = new Crc32();
		original = Encoding.Default.GetBytes(s);
		encoded = crc32.ComputeHash(original);

		StringBuilder sbEncoded = new StringBuilder();
		for (int i = 0; i < encoded.Length; i++)
			sbEncoded.Append(encoded[i].ToString("x2"));

		return sbEncoded.ToString().ToUpper();
	}
    /// <summary>
    /// Checksumme über ein byte[] bilden.
    /// </summary>
    /// <param name="byte[] b"></param>
    /// <returns>byte[] Checksumme</returns>
    public static byte[] CRC32OfByteArray(byte[] b)
    {
        byte[] original = b;
        byte[] encoded;

        Crc32 crc32 = new Crc32();
        //original = Encoding.Default.GetBytes(s);
        encoded = crc32.ComputeHash(original);

        StringBuilder sbEncoded = new StringBuilder();
        for (int i = 0; i < encoded.Length; i++)
            sbEncoded.Append(encoded[i].ToString("x2"));

        return Encoding.Default.GetBytes(sbEncoded.ToString().ToUpper());
    }
    public static string CRC32OfByteArray(byte[] b,bool s)
    {
        byte[] original = b;
        byte[] encoded;

        Crc32 crc32 = new Crc32();
        //original = Encoding.Default.GetBytes(s);
        encoded = crc32.ComputeHash(original);

        StringBuilder sbEncoded = new StringBuilder();
        for (int i = 0; i < encoded.Length; i++)
            sbEncoded.Append(encoded[i].ToString("x2"));

        return sbEncoded.ToString().ToUpper();
    }


	#region Konstanten
	public const UInt32 DefaultPolynomial = 0xedb88320;
	public const UInt32 DefaultSeed = 0xffffffff;
	#endregion

	#region Private Variablen
	private UInt32 _hash;
	private UInt32 _seed;
	private UInt32[] _table;
	#endregion

	#region Kontruktoren
	public Crc32()
	{
		_table = InitializeTable(DefaultPolynomial);
		_seed = DefaultSeed;
		Initialize();
	}

	public Crc32(UInt32 polynomial, UInt32 seed)
	{
		_table = InitializeTable(polynomial);
		this._seed = seed;
		Initialize();
	}
	#endregion

	#region Überschreibungen
	public override void Initialize()
	{
		_hash = _seed;
	}

	protected override void HashCore(byte[] buffer, int start, int length)
	{
		_hash = CalculateHash(_table, _hash, buffer, start, length);
	}

	protected override byte[] HashFinal()
	{
		byte[] hashBuffer = UInt32ToBigEndianBytes(_hash);
		this.HashValue = hashBuffer;

		return hashBuffer;
	}

	public override int HashSize
	{
		get { return 32; }
	}
	#endregion

	#region Implementierung
	public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
	{
		return CalculateHash(
			InitializeTable(polynomial),
			seed,
			buffer,
			0,
			buffer.Length);
	}

	private static UInt32[] InitializeTable(UInt32 polynomial)
	{
		UInt32[] createTable = new UInt32[256];

		for (int i = 0; i < 256; i++)
		{
			UInt32 entry = (UInt32)i;

			for (int j = 0; j < 8; j++)
			{
				if ((entry & 1) == 1)
					entry = (entry >> 1) ^ polynomial;
				else
					entry = entry >> 1;
			}

			createTable[i] = entry;
		}

		return createTable;
	}

	private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
	{
		UInt32 crc = seed;

		for (int i = start; i < size; i++)
		{
			unchecked
			{
				crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
			}
		}

		return ~crc;
	}

	private byte[] UInt32ToBigEndianBytes(UInt32 x)
	{
		return new byte[] {
				(byte)((x >> 24) & 0xff),
				(byte)((x >> 16) & 0xff),
				(byte)((x >> 8) & 0xff),
				(byte)(x & 0xff)
			};
	}
	#endregion
}

 
