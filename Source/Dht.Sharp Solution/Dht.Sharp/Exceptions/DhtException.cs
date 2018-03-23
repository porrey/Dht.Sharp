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

namespace Dht.Sharp
{
	/// <summary>
	/// Represents errors that occur while reading the DHT sensor.
	/// </summary>
	public abstract class DhtException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the Dht.Sharp.DhtException class.
		/// </summary>		
		public DhtException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Exception class with a specified error
		/// message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public DhtException(string message) 
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Exception class with a specified error 
		/// message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, 
		/// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public DhtException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
