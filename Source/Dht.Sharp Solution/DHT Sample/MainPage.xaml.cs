// Copyright © 2018-2022 Daniel Porrey
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
using Dht.Sample.Common;
using Dht.Sharp;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Dht.Sample
{
	public partial class MainPage : BindablePage
	{
		private DispatcherTimer _timer = new DispatcherTimer();

		// ***
		// *** Define a reference for the sensor instance.
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
				// *** Increment the counter.
				// ***
				_totalRequests++;

				// ***
				// *** Read the sensor.
				// ***
				IDhtReading reading = await _sensor.GetReadingAsync();

				// ***
				// *** Check the result.
				// ***
				if (reading.Result == DhtReadingResult.Valid)
				{
					float t = reading.Temperature * 1.8F + 32F;
					this.Temperature = t;
					this.Humidity = reading.Humidity;
					_successfulRequests++;
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			finally
			{
				// ***
				// *** Start the timer again.
				// ***
				_timer.Start();
			}

			// ***
			// *** Update the success rate and running time.
			// ***
			this.RaisePropertyChanged(nameof(this.SuccessRate));
			this.RaisePropertyChanged(nameof(this.RunningTime));
		}

		private float _temperature = 0F;
		public float Temperature
		{
			get
			{
				return _temperature;
			}
			set
			{
				this.SetProperty(ref _temperature, value);
			}
		}

		private float _humidity = 0F;
		public float Humidity
		{
			get
			{
				return _humidity;
			}
			set
			{
				this.SetProperty(ref _humidity, value);
			}
		}

		private int _totalRequests = 0;
		private int _successfulRequests = 0;
		public float SuccessRate
		{
			get
			{
				float returnValue = 0F;

				if (_totalRequests != 0F)
				{
					returnValue = 100F * (float)_successfulRequests / (float)_totalRequests;
				}

				return returnValue;
			}
		}

		private DateTime _startedAt = DateTime.Now;
		public string RunningTime
		{
			get
			{
				string returnValue = String.Empty;

				TimeSpan ellapsed = DateTime.Now.Subtract(_startedAt);

				if (ellapsed.Days > 0)
				{
					returnValue = $"{ellapsed.Days} day(s), {ellapsed.Hours} hour(s), {ellapsed.Minutes} minutes(s) and {ellapsed.Seconds} second(s)";
				}
				else if (ellapsed.Hours > 0)
				{
					returnValue = $"{ellapsed.Hours} hour(s), {ellapsed.Minutes} minutes(s) and {ellapsed.Seconds} second(s)";
				}
				else if (ellapsed.Minutes > 0)
				{
					returnValue = $"{ellapsed.Minutes} minutes(s) and {ellapsed.Seconds} second(s)";
				}
				else
				{
					returnValue = $"{ellapsed.Seconds} second(s)";
				}

				return returnValue;
			}
		}
	}
}
