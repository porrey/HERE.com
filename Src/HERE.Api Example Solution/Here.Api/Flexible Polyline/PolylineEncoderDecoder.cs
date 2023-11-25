using System;
using System.Collections.Generic;
using System.Text;

namespace HERE.Api
{
	public class PolylineEncoderDecoder
	{
		/// <summary>Header version
		/// A change in the version may affect the logic to encode and decode the rest of the header and data
		/// </summary>
		private static readonly byte FORMAT_VERSION = 1;

		// Base64 URL-safe characters
		private static readonly char[] ENCODING_TABLE =
			"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();

		private static readonly int[] DECODING_TABLE =
		{
			62, -1, -1, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1,
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
			22, 23, 24, 25, -1, -1, -1, -1, 63, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35,
			36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51
		};

		/// <summary>
		/// Encode the list of coordinate triples.
		/// The third dimension value will be eligible for encoding only when ThirdDimension is other than ABSENT.
		/// This is lossy compression based on precision accuracy.
		/// </summary>
		/// <param name="coordinates">coordinates {@link List} of coordinate triples that to be encoded.</param>
		/// <param name="precision">Floating point precision of the coordinate to be encoded.</param>
		/// <param name="thirdDimension">{@link ThirdDimension} which may be a level, altitude, elevation or some other custom value</param>
		/// <param name="thirdDimPrecision">Floating point precision for thirdDimension value</param>
		/// <returns>URL-safe encoded {@link String} for the given coordinates.</returns>
		public static string Encode(List<HerePosition> coordinates, int precision, ThirdDimension thirdDimension, int thirdDimPrecision)
		{
			if (coordinates == null || coordinates.Count == 0)
			{
				throw new ArgumentException("Invalid coordinates!");
			}

			if (!Enum.IsDefined(typeof(ThirdDimension), thirdDimension))
			{
				throw new ArgumentException("Invalid thirdDimension");
			}

			Encoder enc = new(precision, thirdDimension, thirdDimPrecision);
			foreach (HerePosition coordinate in coordinates)
			{
				enc.Add(coordinate);
			}

			return enc.GetEncoded();
		}

		/// <summary>
		/// Decode the encoded input {@link String} to {@link List} of coordinate triples.
		/// @see PolylineDecoder#getThirdDimension(String) getThirdDimension
		/// @see LatLngZ
		/// </summary>
		/// <param name="encoded">encoded URL-safe encoded {@link String}</param>
		/// <returns>{@link List} of coordinate triples that are decoded from input</returns>
		public static List<HerePosition> Decode(string encoded)
		{
			if (string.IsNullOrEmpty(encoded?.Trim()))
			{
				throw new ArgumentException("Invalid argument!", nameof(encoded));
			}

			List<HerePosition> result = new();
			Decoder dec = new(encoded);

			decimal lat = 0;
			decimal lng = 0;
			decimal z = 0;

			while (dec.DecodeOne(ref lat, ref lng, ref z))
			{
				result.Add(new HerePosition(lat, lng, z));
				lat = 0;
				lng = 0;
				z = 0;
			}

			return result;
		}

		/// <summary>
		/// ThirdDimension type from the encoded input {@link String}
		/// </summary>
		/// <param name="encoded">URL-safe encoded coordinate triples {@link String}</param>
		public static ThirdDimension GetThirdDimension(string encoded)
		{
			int index = 0;
			long header = 0;
			Decoder.DecodeHeaderFromString(encoded.ToCharArray(), ref index, ref header);
			return (ThirdDimension)((header >> 4) & 0b_111);
		}

		public byte GetVersion()
		{
			return FORMAT_VERSION;
		}

		/// <summary>
		/// Internal class for configuration, validation and encoding for an input request.
		/// </summary>
		private class Encoder
		{
			private readonly StringBuilder _result;
			private readonly Converter _latConverter;
			private readonly Converter _lngConverter;
			private readonly Converter _zConverter;
			private readonly ThirdDimension _thirdDimension;

			public Encoder(int precision, ThirdDimension thirdDimension, int thirdDimPrecision)
			{
				this._latConverter = new Converter(precision);
				this._lngConverter = new Converter(precision);
				this._zConverter = new Converter(thirdDimPrecision);
				this._thirdDimension = thirdDimension;
				this._result = new StringBuilder();
				this.EncodeHeader(precision, (int)this._thirdDimension, thirdDimPrecision);
			}

			private void EncodeHeader(int precision, int thirdDimensionValue, int thirdDimPrecision)
			{
				// Encode the `precision`, `third_dim` and `third_dim_precision` into one encoded char
				if (precision < 0 || precision > 15)
				{
					throw new ArgumentException("precision out of range");
				}

				if (thirdDimPrecision < 0 || thirdDimPrecision > 15)
				{
					throw new ArgumentException("thirdDimPrecision out of range");
				}

				if (thirdDimensionValue < 0 || thirdDimensionValue > 7)
				{
					throw new ArgumentException("thirdDimensionValue out of range");
				}

				long res = (thirdDimPrecision << 7) | (thirdDimensionValue << 4) | precision;
				Converter.EncodeUnsignedVarint(PolylineEncoderDecoder.FORMAT_VERSION, this._result);
				Converter.EncodeUnsignedVarint(res, this._result);
			}

			private void Add(decimal lat, decimal lng)
			{
				this._latConverter.EncodeValue(lat, this._result);
				this._lngConverter.EncodeValue(lng, this._result);
			}

			private void Add(decimal lat, decimal lng, decimal z)
			{
				this.Add(lat, lng);
				if (this._thirdDimension != ThirdDimension.Absent)
				{
					this._zConverter.EncodeValue(z, this._result);
				}
			}

			public void Add(HerePosition herePosition)
			{
				if (herePosition == null)
				{
					throw new ArgumentNullException(nameof(herePosition), "Invalid LatLngZ tuple");
				}

				this.Add(herePosition.Latitude, herePosition.Longitude, herePosition.Elevation);
			}

			public string GetEncoded()
			{
				return this._result.ToString();
			}
		}

		/// <summary>
		/// Single instance for decoding an input request.
		/// </summary>
		private class Decoder
		{
			private readonly char[] _encoded;
			private int _index;
			private readonly Converter _latConverter;
			private readonly Converter _lngConverter;
			private readonly Converter _zConverter;

			private int _precision;
			private int _thirdDimPrecision;
			private ThirdDimension _thirdDimension;

			public Decoder(string encoded)
			{
				this._encoded = encoded.ToCharArray();
				this._index = 0;
				this.DecodeHeader();
				this._latConverter = new Converter(this._precision);
				this._lngConverter = new Converter(this._precision);
				this._zConverter = new Converter(this._thirdDimPrecision);
			}

			private bool HasThirdDimension()
			{
				return this._thirdDimension != ThirdDimension.Absent;
			}

			private void DecodeHeader()
			{
				long header = 0;
				DecodeHeaderFromString(this._encoded, ref this._index, ref header);
				this._precision = (int)(header & 0b_1111); // we pick the first 4 bits only
				header >>= 4;
				this._thirdDimension = (ThirdDimension)(header & 0b_111); // we pick the first 3 bits only
				this._thirdDimPrecision = (int)((header >> 3) & 0b_1111);
			}

			public static void DecodeHeaderFromString(char[] encoded, ref int index, ref long header)
			{
				long value = 0;

				// Decode the header version
				if (!Converter.DecodeUnsignedVarint(encoded, ref index, ref value))
				{
					throw new ArgumentException("Invalid encoding");
				}

				if (value != FORMAT_VERSION)
				{
					throw new ArgumentException("Invalid format version");
				}

				// Decode the polyline header
				if (!Converter.DecodeUnsignedVarint(encoded, ref index, ref value))
				{
					throw new ArgumentException("Invalid encoding");
				}

				header = value;
			}

			public bool DecodeOne(ref decimal lat, ref decimal lng, ref decimal z)
			{
				if (this._index == this._encoded.Length)
				{
					return false;
				}

				if (!this._latConverter.DecodeValue(this._encoded, ref this._index, ref lat))
				{
					throw new ArgumentException("Invalid encoding");
				}

				if (!this._lngConverter.DecodeValue(this._encoded, ref this._index, ref lng))
				{
					throw new ArgumentException("Invalid encoding");
				}

				if (this.HasThirdDimension())
				{
					if (!this._zConverter.DecodeValue(this._encoded, ref this._index, ref z))
					{
						throw new ArgumentException("Invalid encoding");
					}
				}

				return true;
			}
		}

		//Decode a single char to the corresponding value
		private static int DecodeChar(char charValue)
		{
			int pos = charValue - 45;
			if (pos < 0 || pos > 77)
			{
				return -1;
			}

			return DECODING_TABLE[pos];
		}

		/// <summary>
		/// Stateful instance for encoding and decoding on a sequence of Coordinates part of an request.
		/// Instance should be specific to type of coordinates (e.g. Lat, Lng)
		/// so that specific type delta is computed for encoding.
		/// Lat0 Lng0 3rd0 (Lat1-Lat0) (Lng1-Lng0) (3rdDim1-3rdDim0)
		/// </summary>
		public class Converter
		{
			private long _multiplier = 0;
			private long _lastValue = 0;

			public Converter(int precision)
			{
				this.SetPrecision(precision);
			}

			private void SetPrecision(int precision)
			{
				this._multiplier = (long)Math.Pow(10, precision);
			}

			public static void EncodeUnsignedVarint(long value, StringBuilder result)
			{
				while (value > 0x1F)
				{
					byte pos = (byte)((value & 0x1F) | 0x20);
					result.Append(ENCODING_TABLE[pos]);
					value >>= 5;
				}

				result.Append(ENCODING_TABLE[(byte)value]);
			}

			public void EncodeValue(decimal value, StringBuilder result)
			{
				/*
				 * Round-half-up
				 * round(-1.4) --> -1
				 * round(-1.5) --> -2
				 * round(-2.5) --> -3
				 */
				long scaledValue = (long)Math.Round(Math.Abs(value * this._multiplier), MidpointRounding.AwayFromZero) * Math.Sign(value);
				long delta = scaledValue - this._lastValue;
				bool negative = delta < 0;

				this._lastValue = scaledValue;

				// make room on lowest bit
				delta <<= 1;

				// invert bits if the value is negative
				if (negative)
				{
					delta = ~delta;
				}

				EncodeUnsignedVarint(delta, result);
			}

			public static bool DecodeUnsignedVarint(char[] encoded,
				ref int index,
				ref long result)
			{
				short shift = 0;
				long delta = 0;

				while (index < encoded.Length)
				{
					long value = DecodeChar(encoded[index]);
					if (value < 0)
					{
						return false;
					}

					index++;
					delta |= (value & 0x1F) << shift;
					if ((value & 0x20) == 0)
					{
						result = delta;
						return true;
					}
					else
					{
						shift += 5;
					}
				}

				if (shift > 0)
				{
					return false;
				}

				return true;
			}

			//Decode single coordinate (say lat|lng|z) starting at index
			public bool DecodeValue(char[] encoded,
				ref int index,
				ref decimal coordinate)
			{
				long delta = 0;
				if (!DecodeUnsignedVarint(encoded, ref index, ref delta))
				{
					return false;
				}

				if ((delta & 1) != 0)
				{
					delta = ~delta;
				}

				delta >>= 1;
				this._lastValue += delta;
				coordinate = (decimal)this._lastValue / this._multiplier;
				return true;
			}
		}
	}
}
