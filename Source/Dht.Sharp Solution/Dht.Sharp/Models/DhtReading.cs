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
namespace Dht.Sharp
{
	internal class DhtReading : IDhtReading
	{
		internal DhtReading()
		{
			this.Result = DhtReadingResult.None;
		}

		/// <summary>
		/// Gets the result of the sensor reading.
		/// </summary>
		public DhtReadingResult Result { get; set; }

		/// <summary>
		/// Gets the temperature from sensor.
		/// </summary>
		public float Temperature { get; set; }

		/// <summary>
		/// Gets the humidity from the sensor.
		/// </summary>
		public float Humidity { get; set; }

		public static IDhtReading FromData(byte[] data)
		{
			IDhtReading returnValue = new DhtReading();

			// ***
			// *** Verify the checksum.
			// ***
			if (data.HasValidChecksum())
			{
				// ***
				// *** This is a valid reading, convert the temperature and humidity.
				// ***
				((DhtReading)returnValue).Temperature = data.ToTemperature();
				((DhtReading)returnValue).Humidity = data.ToHumidty();
				((DhtReading)returnValue).Result = DhtReadingResult.Valid;
			}
			else
			{
				// ***
				// *** The checksum did not match.
				// ***
				((DhtReading)returnValue).Temperature = 0F;
				((DhtReading)returnValue).Humidity = 0F;
				((DhtReading)returnValue).Result = DhtReadingResult.ChecksumError;
			}

			return returnValue;
		}

		public static IDhtReading FromTimeout()
		{
			IDhtReading returnValue = new DhtReading();

			((DhtReading)returnValue).Temperature = 0F;
			((DhtReading)returnValue).Humidity = 0F;
			((DhtReading)returnValue).Result = DhtReadingResult.Timeout;

			return returnValue;
		}
	}
}
