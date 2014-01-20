﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace ARdevKit.Model.Project
{

    /// <summary>
    /// Inherits from <see cref="Abstract2DAugmentation"/> and adds
    /// <see cref="AbstractSource"/>, in order to show dynamic content.
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public abstract class AbstractDynamic2DAugmentation : Abstract2DAugmentation
    {
        /* //because it is not fully implemented for this release
        /// <summary>
        /// variable which links an <see cref="AbstractSource"/> to 
        /// this <see cref="Abstract2DAugmentation"/>.
        /// </summary>
        [CategoryAttribute("General")]
        public AbstractSource source { get; set; }
        */

        /// <summary>
        /// Initializes no new instance of the <see cref="AbstractDynamic2DAugmentation"/> class,
        /// but can be used by inheriting classes. It is
        /// using the standard constructor from <see cref="Abstract2DAugmentation"/>.
        /// </summary>
        protected AbstractDynamic2DAugmentation() : base()
        {
            //this.source = null; //because it is not fully implemented for this release
        }

        /// <summary>
        /// Initializes no new instance of the <see cref="AbstractDynamic2DAugmentation"/> class,
        /// but can be used by inheriting classes. It is
        /// using the constructor from <see cref="Abstract2DAugmentation"/>.
        /// </summary>
        /// <param name="isVisible">if set to <c>true</c> [is visible] using AREL.</param>
        /// <param name="translationVector">The translation vector.</param>
        /// <param name="scaling">The scaling.</param>
        /// <param name="trackable">The trackable.</param>
        /// <param name="source">The source.</param>
        protected AbstractDynamic2DAugmentation(bool isVisible,
            Vector3D translationVector, Vector3D scaling, AbstractTrackable trackable, int width, int height,
            AbstractSource source)
            : base(isVisible, translationVector, scaling, trackable, width, height)
        {
            //this.source = source; //because it is not fully implemented for this release
        }
    }
}