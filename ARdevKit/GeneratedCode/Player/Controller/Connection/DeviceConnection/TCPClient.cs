﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Wenn der Code neu generiert wird, gehen alle Änderungen an dieser Datei verloren
// </auto-generated>
//------------------------------------------------------------------------------
namespace Player.Controller.Connection.DeviceConnection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A TCP client. </summary>
    ///
    /// <remarks>   Geht, 18.12.2013. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

	public class TCPClient
	{
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Connects the given IP. </summary>
        ///
        /// <remarks>   Geht, 18.12.2013. </remarks>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="IP">   The IP to connect. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		public virtual void connect(IPAddress IP)
		{
			throw new System.NotImplementedException();
		}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sends a string. </summary>
        ///
        /// <remarks>   Geht, 18.12.2013. </remarks>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="str">  The. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		public virtual void sendString(string str)
		{
			throw new System.NotImplementedException();
		}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Disconnects this object. </summary>
        ///
        /// <remarks>   Geht, 18.12.2013. </remarks>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		public virtual void disconnect()
		{
			throw new System.NotImplementedException();
		}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Listen and receive. </summary>
        ///
        /// <remarks>   Geht, 18.12.2013. </remarks>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		public virtual void listenAndReceive()
		{
			throw new System.NotImplementedException();
		}

	}
}
