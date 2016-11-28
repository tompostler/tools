/* Perceptual image hash calculation tool based on algorithm descibed in
 * Block Mean Value Based Image Perceptual Hashing by Bian Yang, Fan Gu and Xiamu Niu
 *
 * Copyright 2014 Commons Machinery http://commonsmachinery.se/
 * Distributed under an MIT license, please see LICENSE in the top dir.
 */
// Source drawn from https://github.com/commonsmachinery/blockhash/blob/master/blockhash.c on 2016-11-27
// Modified under the MIT license by Tom Postler to work with C#

namespace Unlimitedinf.Tools.Hashing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Perceptual image hash calculation tool based on algorithm descibed in 
    /// Block Mean Value Based Image Perceptual Hashing by Bian Yang, Fan Gu and Xiamu Niu.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Blockhash")]
    public sealed class Blockhash : HashAlgorithm
    {
        private static byte[] ImageToRGBA(Image image)
        {
            Bitmap bitmap = image as Bitmap;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] rgba = new byte[bitmap.Width * bitmap.Height * 4];
            int width = data.Width;

            try
            {
                Parallel.For(0, data.Height, (scanline) =>
                {
                    byte[] pixelData = new byte[data.Stride];
                    Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < width; pixeloffset++)
                    {
                        // PixelFormat.Format32bppArgb means the data is stored in memory as BGRA. But we want RGBA.
                        int pixel = (scanline * width + pixeloffset) * 4;
                        rgba[pixel + 0] = pixelData[pixeloffset * 4 + 2];
                        rgba[pixel + 1] = pixelData[pixeloffset * 4 + 1];
                        rgba[pixel + 2] = pixelData[pixeloffset * 4 + 0];
                        rgba[pixel + 3] = pixelData[pixeloffset * 4 + 3];
                    }
                });
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return rgba;
        }

        private static byte[] bits_to_bytes(int[] bits)
        {
            byte[] result = new byte[bits.Length / 4];
            for (int i = 0; i < bits.Length; i += 4)
                result[i/4] = (byte)((bits[i] << 3) + (bits[i + 1] << 2) + (bits[i + 2] << 1) + (bits[i + 3]));
            return result;
        }

        private static int[] blockhash_quick(int bits, byte[] data, int width, int height)
        {
            int x, y, ix, iy;
            int ii, alpha, value;
            int block_width;
            int block_height;
            int[] blocks;

            block_width = width / bits;
            block_height = height / bits;

            blocks = new int[bits * bits];
            for (y = 0; y < bits; y++)
            {
                for (x = 0; x < bits; x++)
                {
                    value = 0;

                    for (iy = 0; iy < block_height; iy++)
                    {
                        for (ix = 0; ix < block_width; ix++)
                        {
                            ii = ((y * block_height + iy) * width + (x * block_width + ix)) * 4;

                            alpha = data[ii + 3];
                            if (alpha == 0)
                            {
                                value += 765;
                            }
                            else
                            {
                                value += data[ii] + data[ii + 1] + data[ii + 2];
                            }
                        }
                    }

                    blocks[y * bits + x] = value;
                }
            }

            return translate_blocks_to_bits(blocks, bits * bits, block_width * block_height);
        }

        private static int[] blockhash(int bits, byte[] data, int width, int height)
        {
            float block_width;
            float block_height;
            float y_frac, y_int;
            float x_frac, x_int;
            float x_mod, y_mod;
            float weight_top, weight_bottom, weight_left, weight_right;
            int block_top, block_bottom, block_left, block_right;
            int x, y, ii, alpha;
            float value;
            float[] blocks;

            if (width % bits == 0 && height % bits == 0)
            {
                return blockhash_quick(bits, data, width, height);
            }

            block_width = (float)width / bits;
            block_height = (float)height / bits;

            blocks = new float[bits * bits];

            for (y = 0; y < height; y++)
            {
                y_mod = ((float)y + 1) % block_height;
                y_int = (int)y_mod;
                y_frac = y_mod % 1;

                weight_top = (1 - y_frac);
                weight_bottom = y_frac;

                // y_int will be 0 on bottom/right borders and on block boundaries
                if (y_int > 0 || (y + 1) == height)
                {
                    block_top = block_bottom = (int)Math.Floor((float)y / block_height);
                }
                else
                {
                    block_top = (int)Math.Floor((float)y / block_height);
                    block_bottom = (int)Math.Ceiling((float)y / block_height);
                }

                for (x = 0; x < width; x++)
                {
                    x_mod = ((float)x + 1) % block_width;
                    x_int = (int)x_mod;
                    x_frac = x_mod % 1;

                    weight_left = (1 - x_frac);
                    weight_right = x_frac;

                    // x_int will be 0 on bottom/right borders and on block boundaries
                    if (x_int > 0 || (x + 1) == width)
                    {
                        block_left = block_right = (int)Math.Floor((float)x / block_width);
                    }
                    else
                    {
                        block_left = (int)Math.Floor((float)x / block_width);
                        block_right = (int)Math.Ceiling((float)x / block_width);
                    }

                    ii = (y * width + x) * 4;

                    alpha = data[ii + 3];
                    if (alpha == 0)
                    {
                        value = 765;
                    }
                    else
                    {
                        value = data[ii] + data[ii + 1] + data[ii + 2];
                    }

                    // add weighted pixel value to relevant blocks
                    blocks[block_top * bits + block_left] += value * weight_top * weight_left;
                    blocks[block_top * bits + block_right] += value * weight_top * weight_right;
                    blocks[block_bottom * bits + block_left] += value * weight_bottom * weight_left;
                    blocks[block_bottom * bits + block_right] += value * weight_bottom * weight_right;
                }
            }

            return translate_blocks_to_bitsf(blocks, bits * bits, (int)(block_width * block_height));
        }

        private static int[] translate_blocks_to_bits(int[] blocks, int nblocks, int pixels_per_block)
        {
            float half_block_value;
            int bandsize, i, j, v;
            float m;

            half_block_value = pixels_per_block * 256 * 3 / 2;
            bandsize = nblocks / 4;

            for (i = 0; i < 4; i++)
            {
                int[] subblocks = new int[bandsize];
                Array.Copy(blocks, i * bandsize, subblocks, 0, bandsize);
                m = (float)subblocks.Median();
                for (j = i * bandsize; j < (i + 1) * bandsize; j++)
                {
                    v = blocks[j];
                    blocks[j] = (v > m || (Math.Abs(v - m) < 1 && m > half_block_value)) ? 1 : 0;
                }
            }
            return blocks;
        }

        private static int[] translate_blocks_to_bitsf(float[] blocks, int nblocks, int pixels_per_block)
        {
            int[] result = new int[nblocks];
            float half_block_value;
            int bandsize, i, j;
            float m, v;

            half_block_value = pixels_per_block * 256 * 3 / 2;
            bandsize = nblocks / 4;

            for (i = 0; i < 4; i++)
            {
                float[] subblocks = new float[bandsize];
                Array.Copy(blocks, i * bandsize, subblocks, 0, bandsize);
                m = (float)subblocks.Median();
                for (j = i * bandsize; j < (i + 1) * bandsize; j++)
                {
                    v = blocks[j];
                    result[j] = (v > m || (Math.Abs(v - m) < 1 && m > half_block_value)) ? 1 : 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Given a file name for an image, return the blockhash.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bits"></param>
        /// <param name="quick"></param>
        /// <returns></returns>
        public static byte[] ProcessImage(string fileName, int bits = 16, bool quick = false)
        {
            return ProcessImage(Image.FromFile(fileName), bits, quick);
        }

        /// <summary>
        /// Given a steam containing an image, return the blockhash.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bits"></param>
        /// <param name="quick"></param>
        /// <returns></returns>
        public static byte[] ProcessImage(Stream stream, int bits = 16, bool quick = false)
        {
            return ProcessImage(Image.FromStream(stream), bits, quick);
        }

        /// <summary>
        /// Given an image, return the blockhash.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bits"></param>
        /// <param name="quick"></param>
        /// <returns></returns>
        public static byte[] ProcessImage(Image image, int bits = 16, bool quick = false)
        {
            if (ReferenceEquals(image, null))
                throw new ArgumentNullException(nameof(image));

            byte[] image_data = ImageToRGBA(image);

            int[] hash;

            if (quick)
            {
                hash = blockhash_quick(bits, image_data, image.Width, image.Height);
            }
            else
            {
                hash = blockhash(bits, image_data, image.Width, image.Height);
            }

            return bits_to_bytes(hash);
        }

        /// <summary>
        /// Compute the hamming distances between two bit arrays.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int HammingDistance(byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, null))
                throw new ArgumentNullException(nameof(left));
            if (ReferenceEquals(right, null))
                throw new ArgumentNullException(nameof(right));
            if (left.Length != right.Length)
                throw new ArgumentException("Should only compare equal-length arrays");

            int count = 0;
            for (int i = 0; i < left.Length; i++)
            {
                int diff = left[i] ^ right[i];
                count += ((diff >> 3) & 1)
                    + ((diff >> 2) & 1)
                    + ((diff >> 1) & 1)
                    + (diff & 1);
            }

            return count;
        }

        #region HashAlgorithm overrides and implmentation

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }

        public static new HashAlgorithm Create()
        {
            return new Blockhash();
        }

        public static new byte[] ComputeHash(Stream inputStream)
        {
            return ProcessImage(inputStream);
        }

        #endregion
    }
}
