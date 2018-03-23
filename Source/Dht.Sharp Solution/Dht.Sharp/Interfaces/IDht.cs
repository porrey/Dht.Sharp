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
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Dht.Sharp
{
	/// <summary>
	/// Defines a type for a DHT sensor that can be used for a DHT22 or DHT11.
	/// </summary>
	public interface IDht
	{
		/// <summary>
		/// Gets/sets the GPIO pin used to read data from the sensor. This pin is connected
		/// directly to the data pin on the sensor.
		/// </summary>
		GpioPin DataPin { get; set; }

		/// <summary>
		/// Specifies the pin used to trigger a reading from the sensor. This pin is connected
		/// to the data pin on the sensor through a N-channel MOSFET.
		/// </summary>
		GpioPin TriggerPin { get; set; }

		/// <summary>
		/// Gets/sets a value that indicates how long to wait for the sensor to 
		/// respond to a request for a reading. The default timeout is 100 ms.
		/// </summary>
		TimeSpan Timeout { get; set; }

		/// <summary>
		/// Initializes the sensor.
		/// </summary>
		Task Initialize();

		/// <summary>
		/// Gets a reading from the sensor.
		/// </summary>
		/// <returns>Returns an IDhtReading instance containing 
		/// the data from the sensor.</returns>
		Task<IDhtReading> GetReadingAsync();
	}
}