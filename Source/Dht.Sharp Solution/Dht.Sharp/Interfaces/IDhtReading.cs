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
	/// <summary>
	/// Represents the result of a DHT sensor reading.
	/// </summary>
	public enum DhtReadingResult
	{
		/// <summary>
		/// Indicates that no reading has been taken.
		/// </summary>
		None,
		/// <summary>
		/// Indicates the reading is valid.
		/// </summary>
		Valid,
		/// <summary>
		/// Indicates that the sensor did not respond to the request for a reading.
		/// </summary>
		Timeout,
		/// <summary>
		/// Indicates that the reading from the sensor was invalid.
		/// </summary>
		ChecksumError
	}

	/// <summary>
	/// Defines an interface for a reading from a DHT sensor.
	/// </summary>
	public interface IDhtReading
	{
		/// <summary>
		/// Gets the result of the sensor reading.
		/// </summary>
		DhtReadingResult Result { get; }

		/// <summary>
		/// Gets the humidity from the sensor.
		/// </summary>
		/// <summary>
		/// Gets the temperature from sensor.
		/// </summary>
		float Temperature { get; }

		/// <summary>
		/// Gets the humidity from the sensor.
		/// </summary>
		float Humidity { get; }
	}
}