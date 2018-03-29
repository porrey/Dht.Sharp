// Copyright © 2018 Daniel Porrey
//
// This file is part of the Dht11 Solution.
// 
// Dht.Sharp Solution is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dht.Sharp Solution is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Dht.Sharp Solution. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Gpio;

namespace Dht.Sharp
{
	internal static class DhtExtensions
	{
		/// <summary>
		/// Converts the change record intervals into bits and then into a
		/// 5 byte array.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this IList<GpioChangeRecord> data)
		{
			byte[] returnValue = new byte[15];

			// ***
			// *** The change records contain the time of each input change from 5v (high)
			// *** to 0v (low). This is the falling edge. The time between each falling edge
			// *** is calculated to determine if the data being sent is a 1 or a 0. The first
			// *** two edges are the trigger and the response and must be ignored. The remaining
			// *** 41 represents 40 intervals which is 40 bits or 5 bytes.
			// ***
			for (int i = 3; i < data.Count(); i++)
			{
				// ***
				// *** Calculate the total time interval between falling edges.
				// ***
				int interval = (int)((data[i].RelativeTime - data[i - 1].RelativeTime).TotalMilliseconds * 1000);

				// ***
				// *** Get the value of the next bit.
				// ***
				byte bit = interval > 110 ? (byte)1 : (byte)0;

				// ***
				// *** Get the index of the byte we are filling. The sensor sends
				// *** a total of 5 bytes.
				// ***
				int index = (int)Math.Floor((i - 3) / 8D);

				// ***
				// *** Shift the bits one to the left.
				// ***
				returnValue[index] <<= 1;

				// ***
				// *** Add the next bit to the byte.
				// ***
				returnValue[index] += bit;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the checksum value given by the sensor.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte ExpectedChecksum(this byte[] data)
		{
			return data[4];
		}

		/// <summary>
		/// Calculates the checksum of the first 4 bytes of data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte ActualChecksum(this byte[] data)
		{
			return (byte)(data[0] + data[1] + data[2] + data[3]);
		}

		/// <summary>
		/// Compares the expected and actual checksums.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool HasValidChecksum(this byte[] data)
		{
			return data.ExpectedChecksum() == data.ActualChecksum();
		}

		/// <summary>
		/// Converts the byte data to a temperature value.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static float ToTemperature(this byte[] data)
		{
			float returnValue = 0F;

			// ***
			// *** Get the temperature from bytes 2 and 3. AND byte 2 the 
			// *** high byte with 0x7f to clear the highest bit (this bit is
			// *** used to indicate if the temperature is positive or negative).
			// ***
			returnValue = (((data[2] & 0x7f) << 8) + data[3]) / 10F;

			// ***
			// *** if the highest bit in the temperature is 1, then this
			// *** is a negative temperature value.
			// ***
			bool negativeTemperature = (data[2] & 0x80) == 0x80;
			if (negativeTemperature) returnValue *= -1;

			return returnValue;
		}

		/// <summary>
		/// Converts the byte data to a humidity value.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static float ToHumidty(this byte[] data)
		{
			float returnValue = 0F;

			// ***
			// *** Get the humidity from bytes 0 and 1
			// ***
			returnValue = ((data[0] << 8) + data[1]) / 10F;

			return returnValue;
		}
	}
}
