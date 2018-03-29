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
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Dht.Sharp
{
	/// <summary>
	/// An instance of IDht used specifically for the DHT22 sensor.
	/// </summary>
	public class Dht22 : IDht
	{
		/// <summary>
		/// Creates an instance of Dht.Sharp.Dht22 with the given Data Pin
		/// and Trigger Pin.
		/// </summary>
		/// <param name="dataPin">Specifies the GPIO pin used to read data from the sensor. This pin is connected
		/// directly to the data pin on the sensor.</param>
		/// <param name="triggerPin">Specifies the pin used to trigger a reading from the sensor. This pin is connected
		/// to the data pin on the sensor through a N-channel MOSFET.</param>
		public Dht22(GpioPin dataPin, GpioPin triggerPin)
		{
			this.DataPin = dataPin ?? throw new ArgumentNullException(nameof(dataPin));
			this.TriggerPin = triggerPin ?? throw new ArgumentNullException(nameof(triggerPin));
		}

		/// <summary>
		/// Gets/sets the GPIO pin used to read data from the sensor. This pin is connected
		/// directly to the data pin on the sensor.
		/// </summary>
		public GpioPin DataPin { get; set; }

		/// <summary>
		/// Specifies the pin used to trigger a reading from the sensor. This pin is connected
		/// to the data pin on the sensor through a N-channel MOSFET.
		/// </summary>
		public GpioPin TriggerPin { get; set; }

		/// <summary>
		/// Gets/sets a value that indicates how long to wait for the sensor to 
		/// respond to a request for a reading. The default timeout is 100 ms.
		/// </summary>
		public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(100);

		/// <summary>
		/// The Change Reader object used to collect data from the sensor.
		/// </summary>
		protected GpioChangeReader ChangeReader { get; set; }

		/// <summary>
		/// Initializes the sensor.
		/// </summary>
		public async Task Initialize()
		{
			// ***
			// *** Set the trigger pin LOW which sets the data pin HIGH.
			// ***
			this.TriggerPin.Write(GpioPinValue.Low);

			// ***
			// *** Initialize the change reader.
			// ***
			this.ChangeReader = new GpioChangeReader(this.DataPin)
			{
				Polarity = GpioChangePolarity.Falling
			};

			// ***
			// *** The data sheet states that the sensor should be given 1 second to initialize.
			// ***
			await Task.Delay(TimeSpan.FromSeconds(1));
		}

		/// <summary>
		/// Gets a reading from the sensor.
		/// </summary>
		/// <returns>Returns an IDhtReading instance containing 
		/// the data from the sensor.</returns>
		public async Task<IDhtReading> GetReadingAsync()
		{
			IDhtReading returnValue = new DhtReading();

			if (this.ChangeReader != null)
			{
				try
				{
					// ***
					// *** Clear and start the Change Reader.
					// ***
					this.ChangeReader.Clear();
					this.ChangeReader.Start();

					// ***
					// *** Bring the line low for 18 ms (this is needed for the DHT11), the DHT22 does need
					// *** need as long.
					// ***
					this.TriggerPin.Write(GpioPinValue.High);
					await Task.Delay(TimeSpan.FromMilliseconds(18));
					this.TriggerPin.Write(GpioPinValue.Low);

					// ***
					// *** Wait for 43 falling edges, but do not wait for more than the timeout.
					// ***
					CancellationTokenSource source = new CancellationTokenSource((int)this.Timeout.TotalMilliseconds);
					await this.ChangeReader.WaitForItemsAsync(43).AsTask(source.Token);

					// ***
					// *** Get all of the change records.
					// ***
					IList<GpioChangeRecord> changeRecords = this.ChangeReader.GetAllItems();

					// ***
					// *** Convert the change records to the 5 bytes of data that the sensor
					// *** sends out.
					// ***
					byte[] data = changeRecords.ToByteArray();

					// ***
					// *** Convert the 5 bytes of data to an IDhtReading instance.
					// ***
					returnValue = DhtReading.FromData(data);
				}
				catch (TaskCanceledException)
				{
					// ***
					// *** The sensor did not respond to the 
					// *** data request.
					// ***
					returnValue = DhtReading.FromTimeout();
				}
				finally
				{
					this.ChangeReader.Stop();
				}
			}
			else
			{
				throw new DhtNotInitializedException();
			}

			return returnValue;
		}
	}
}
