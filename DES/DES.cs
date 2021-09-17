using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DES {
    // ReSharper disable once InconsistentNaming
    public static class DES {
        private static readonly byte[] IP = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private static readonly byte[] FP = {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        private static readonly byte[] K1P = {
            57, 49, 41, 33, 25, 17, 9, 1,
            58, 50, 42, 34, 26, 18, 10, 2,
            59, 51, 43, 35, 27, 19, 11, 3,
            60, 52, 44, 36
        };

        private static readonly byte[] K2P = {
            63, 55, 47, 39, 31, 23, 15, 7,
            62, 54, 46, 38, 30, 22, 14, 6,
            61, 53, 45, 37, 29, 21, 13, 5,
            28, 20, 12, 4
        };

        private static readonly byte[] CP = {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        private static readonly byte[] EP = {
            32, 1, 2, 3, 4, 5, 4, 5,
            6, 7, 8, 9, 8, 9, 10, 11,
            12, 13, 12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21, 20, 21,
            22, 23, 24, 25, 24, 25, 26, 27,
            28, 29, 28, 29, 30, 31, 32, 1
        };

        private static readonly byte[] P = {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };

        private static readonly byte[,,] Sbox = {
            {
                { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
            }, {
                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
            }, {
                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
            }, {
                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
            }, {
                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
            }, {
                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
            }, {
                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
            }, {
                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            }
        };

        private static readonly byte[] ShiftBits = {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
        };

        public static byte[] Encrypt(string message, string key) {
            var messageBytes = Encoding.Default.GetBytes(message);
            return Des(messageBytes, GetKeyBytes(key), true);
        }

        public static string Decrypt(ReadOnlySpan<byte> encryptedMessage, string key) {
            var decryptedMessage = Des(encryptedMessage, GetKeyBytes(key), false);
            return Encoding.Default.GetString(decryptedMessage);
        }

        private static byte[] EnlargeMessage(ReadOnlySpan<byte> originalMessage) {
            var length = originalMessage.Length;
            if (length % 8 != 0) {
                length += 8 - length % 8;
            }

            var enlargedMessage = new byte[length];
            originalMessage.CopyTo(enlargedMessage);
            return enlargedMessage;
        }

        private static byte[] Des(ReadOnlySpan<byte> originalMessage, byte[] keyBytes, bool encrypt) {
            var messageBytes = EnlargeMessage(originalMessage);
            var numOfParts   = messageBytes.Length / 8;

            var subKeys = CreateSubKeys(keyBytes);

            for (var i = 0; i < numOfParts; i++) {
                var messagePart = messageBytes.AsSpan().Slice(i * 8, 8);
                FeistelCypher(messagePart, subKeys, encrypt);
            }

            return messageBytes;
        }

        private static BitArray[] CreateSubKeys(byte[] keyBytes) {
            Array.Reverse(keyBytes);
            var keyBits = new BitArray(keyBytes);
            var left    = new bool[28];
            var right   = new bool[28];
            for (var i = 0; i < 28; i++) {
                left[27 - i]  = keyBits[64 - K1P[i]];
                right[27 - i] = keyBits[64 - K2P[i]];
            }

            var subKeys = new BitArray[16];
            var temp    = new BitArray(56);

            for (var i = 0; i < 16; i++) {
                left.Rotate(ShiftBits[i]);
                right.Rotate(ShiftBits[i]);

                var subKey = new BitArray(48);
                for (var j = 0; j < 28; j++) {
                    temp[j]      = right[j];
                    temp[j + 28] = left[j];
                }

                for (var j = 0; j < 48; j++) {
                    subKey[47 - j] = temp[56 - CP[j]];
                }

                subKeys[i] = subKey;
            }

            return subKeys;
        }

        private static void FeistelCypher(Span<byte> messageBytes, IReadOnlyList<BitArray> subKeys, bool encrypt) {
            messageBytes.Reverse();
            var messageBits = new BitArray(64);
            for (var i = 0; i < 8; i++) {
                var messageByte = messageBytes[i];
                for (var j = 0; j < 8; j++) {
                    messageBits[i * 8 + j] = Convert.ToBoolean(messageByte >> j & 1);
                }
            }

            var left  = new BitArray(32);
            var right = new BitArray(32);
            for (var i = 0; i < 32; i++) {
                left[31 - i]  = messageBits[64 - IP[i]];
                right[31 - i] = messageBits[64 - IP[i + 32]];
            }

            var temp = new BitArray(32);
            for (var i = 0; i < 16; i++) {
                for (var j = 0; j < 32; j++) {
                    temp[j] = right[j];
                }

                var subKey = encrypt ? subKeys[i] : subKeys[15 - i];
                F(right, subKey);
                right = right.Xor(left);

                (left, temp) = (temp, left);
            }

            var leftRight = new BitArray(64);
            for (var i = 0; i < 32; i++) {
                leftRight[i]      = left[i];
                leftRight[i + 32] = right[i];
            }

            for (var i = 0; i < 64; i++) {
                messageBits[63 - i] = leftRight[64 - FP[i]];
            }

            var tempByteArray = new byte[8];
            messageBits.CopyTo(tempByteArray, 0);
            tempByteArray.CopyTo(messageBytes);
            messageBytes.Reverse();
        }

        private static void F(BitArray right, BitArray subKey) {
            var extended = new BitArray(48);
            for (var j = 0; j < 48; j++) {
                extended[47 - j] = right[32 - EP[j]];
            }

            var result = extended.Xor(subKey);

            var newRight = new BitArray(32);

            for (var j = 0; j < 8; j++) {
                var pack = j * 6;

                byte row = 0;
                row |= Convert.ToByte(result[pack]);
                row |= (byte)(Convert.ToInt32(result[pack + 5]) << 1);

                byte column = 0;
                for (var k = 0; k < 4; k++) {
                    column |= (byte)(Convert.ToInt32(result[pack + k + 1]) << k);
                }

                var value = Sbox[7 - j, row, column];

                for (var k = 0; k < 4; k++) {
                    newRight[j * 4 + k] = Convert.ToBoolean(value >> k & 1);
                }
            }

            for (var j = 0; j < 32; j++) {
                right[31 - j] = newRight[32 - P[j]];
            }
        }

        private static byte[] GetKeyBytes(string key) {
            const int keyLength = 8;
            var       keyBytes  = new byte[keyLength];
            Encoding.Default.GetBytes(key).AsSpan()[..8].CopyTo(keyBytes);
            return keyBytes;
        }
    }
}
