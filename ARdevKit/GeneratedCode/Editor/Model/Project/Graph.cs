﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Wenn der Code neu generiert wird, gehen alle Änderungen an dieser Datei verloren
// </auto-generated>
//------------------------------------------------------------------------------
namespace Editor.Model.Project
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A graph. </summary>
    ///
    /// <remarks>   Geht, 18.12.2013. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

	public abstract class Graph : AbstractDynamic2DAugmentation
	{
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the minimum value. </summary>
        ///
        /// <value> The minimum value. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		private int minValue
		{
			get;
			set;
		}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the maximum value. </summary>
        ///
        /// <value> The maximum value. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		private int maxValue
		{
			get;
			set;
		}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the scaling. </summary>
        ///
        /// <value> The scaling. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

		private int scaling
		{
			get;
			set;
		}

	}
}
