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
using Dht.Sharp;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dht.Sample
{
	public partial class MainPage : Page
	{
		private DispatcherTimer _timer = new DispatcherTimer();

		// ***
		// ***
		// ***
		IDht _sensor = null;

		public MainPage()
		{
			this.InitializeComponent();

			// ***
			// *** Initialize the timer
			// ***
			_timer.Interval = TimeSpan.FromSeconds(2);
			_timer.Tick += this.Timer_Tick;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// ***
			// *** Get a reference to the GPIO Controller.
			// ***
			GpioController controller = GpioController.GetDefault();

			// ***
			// *** Make sure the reference is valid (that e are connected to a device with
			// *** a GPIO Controller.
			// ***
			if (controller != null)
			{
				// ***
				// *** Set up the data pin.
				// ***
				GpioPin dataPin = GpioController.GetDefault().OpenPin(5, GpioSharingMode.Exclusive);
				dataPin.SetDriveMode(GpioPinDriveMode.Input);

				// ***
				// *** Set up the trigger pin.
				// ***
				GpioPin triggerPin = GpioController.GetDefault().OpenPin(4, GpioSharingMode.Exclusive);
				triggerPin.SetDriveMode(GpioPinDriveMode.Output);

				// ***
				// *** Create the sensor.
				// ***
				_sensor = new Dht22(dataPin, triggerPin);
				await _sensor.Initialize();
			}

			// ***
			// *** Start the timer.
			// ***
			_timer.Start();
		}

		private async void Timer_Tick(object sender, object e)
		{
			try
			{
				// ***
				// *** Stop the timer so we do not have more than reading
				// *** at a time.
				// ***
				_timer.Stop();

				// ***
				// *** read the sensor.
				// ***
				IDhtReading reading = await _sensor.GetReadingAsync();

				// ***
				// *** Check the result.
				// ***
				if (reading.Result == DhtReadingResult.Valid)
				{
					System.Diagnostics.Debug.WriteLine($"Temperature = {reading.Temperature:0.0} C, Humidity = {reading.Humidity:0.0}%");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"Error = {reading.Result}");
				}
			}
			finally
			{
				// ***
				// *** Start the timer again.
				// ***
				_timer.Start();
			}
		}
	}
}
