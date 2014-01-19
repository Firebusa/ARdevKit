﻿using ARdevKit.Controller.ProjectController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ARdevKit.Model.Project
{
    /// <summary>
    ///     Encapsulates everything, that is needed for an AR-Application
    ///     and so this the element, which the user saves, loads or exports
    /// </summary>
    [Serializable]
    public class Project// : ISerializable
    {
        /// <summary>
        /// The name
        /// </summary>
        private string name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>   Full pathname of the project file. </summary>
        private string projectPath;
        /// <summary>   Gets or sets the full pathname of the project file. </summary>
        ///
        /// <value> The full pathname of the project file. </value>
        public string ProjectPath
        {
            get { return projectPath; }
            set { projectPath = value; }
        }

        /// <summary>   The sensor. </summary>
        protected AbstractSensor sensor;
        /// <summary>   Gets or sets the sensor. </summary>
        ///
        /// <value> The sensor. </value>
        public AbstractSensor Sensor
        {
            get { return sensor; }
            set { sensor = value; }
        }

        /// <summary>
        /// The sources
        /// </summary>
        private List<AbstractSource> sources;
        /// <summary>
        /// Gets or sets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        public List<AbstractSource> Sources
        {
            get { return sources; }
            set { sources = value; }
        }

        /// <summary>
        /// The trackables
        /// </summary>
        private List<AbstractTrackable> trackables;
        /// <summary>
        /// Gets or sets the trackables.
        /// </summary>
        /// <value>
        /// The trackables.
        /// </value>
        public List<AbstractTrackable> Trackables
        {
            get { return trackables; }
            set { trackables = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            name = "";
            projectPath = "";
            sensor = null; 
            trackables = new List<AbstractTrackable>();
            sources = new List<AbstractSource>();
        }

        /// <summary>   Initializes a new instance of the <see cref="Project"/> class. </summary>
        /// 
        /// <param name="name"> The name. </param>
        public Project(string name) : this()
        {
            this.name = name;
        }

        /// <summary>   Initializes a new instance of the <see cref="Project"/> class. </summary>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="projectPath">  Full pathname of the project file. </param>
        public Project(string name, string projectPath)
            : this(name)
        {
            this.projectPath = projectPath;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(AbstractProjectVisitor visitor)
        {
            visitor.Visit(this);
            foreach (AbstractTrackable t in Trackables)
            {
                t.Accept(visitor);
            }
            sensor.Accept(visitor);
        }

        /// <summary>
        ///     Is needed for Custom Serialization. Provides the Serializer with the needed information.
        /// </summary>
        /// <param name="info">Serialization Information, which is modified to encapsulate the things to save</param>
        /// <param name="context">describes aim and source of a serialized stream</param>
        [Obsolete("GetObjectData is obsolete, serialization is done without customization.")]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the associated source, if it is associated with the project.
        /// </summary>
        /// <param name="source">The source, which is searched.</param>
        /// <returns>the associated source </returns>
        public AbstractSource findSource(AbstractSource source)
        {
            return this.sources[this.sources.IndexOf(source)];
        }

        /// <summary>
        /// Returns, if the specified source is associated with this project.
        /// </summary>
        /// <param name="source">The specified source.</param>
        /// <returns>true, if the source is associated with this project
        ///          false, else</returns>
        public bool existSource(AbstractSource source)
        {
            foreach (AbstractSource s in sources)
            {
                if (s == source)
                {
                    return true;
                }
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     tests if all trackables in this.trackables are null. if there are one which is not null
        ///     it's true.
        /// </summary>
        ///
        /// <remarks>   Lizzard, 1/19/2014. </remarks>
        ///
        /// <returns>   true if trackable, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool isTrackable()
        {
            foreach (AbstractTrackable temp in this.trackables)
            {
                if (temp != null)
                {
                    return true;
                }
            }
            return false;
        }
    }

    
}
