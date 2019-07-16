﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRCBerechnung
{
    public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }

    public class Crc16Ccitt
    {
        const ushort poly = 0x1021;
        ushort[] table = new ushort[256];
        ushort initialValue = 0;

        public ushort ComputeChecksumUshort(byte[] bytes)
        {
            ushort crc = this.initialValue;

            for (int i = 0; i < bytes.Length; i++)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

        public string ComputeChecksumHEX(byte[] bytes)
        {
            return ComputeChecksumUshort(bytes).ToString("X");
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksumUshort(bytes);
            return new byte[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }

        public Crc16Ccitt(InitialCrcValue pinitialValue)
        {
            initialValue = (ushort)pinitialValue;

            ushort temp, a;
            for (int i = 0; i < table.Length; i++)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; j++)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                    {
                        temp = (ushort)((temp << 1) ^ poly);
                    }
                    else
                    {
                        temp <<= 1;
                    }
                    a <<= 1;
                }
                table[i] = temp;
            }
        }
    }


}
