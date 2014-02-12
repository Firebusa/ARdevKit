﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using ARdevKit.Controller.ProjectController;
using ARdevKit.View;
using System.Drawing;
using System.Windows.Forms;

namespace ARdevKit.Model.Project
{
    /// <summary>
    /// Describes a Marker, which is very flexible, because it is also
    /// a Picture. It is an <see cref="AbstractMarker"/>
    /// </summary>
    [Serializable]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class PictureMarker : Abstract2DTrackable
    {
        /// <summary>
        /// Full pathname of the picture file.
        /// </summary>
        protected string picturePath;
        /// <summary>
        /// Gets or sets the full pathname of the picture file.
        /// </summary>
        /// <value>
        /// The full pathname of the picture file.
        /// </value>
        [CategoryAttribute("General"), Description("Full pathname of the image file"), EditorAttribute(typeof(FileSelectorTypeEditor), 
            typeof(System.Drawing.Design.UITypeEditor))]
        public string PicturePath
        {
            get { return picturePath; }
            set 
            { 
                picturePath = value;
                pictureName = Path.GetFileName(value);
            }
        }

        /// <summary>
        /// Name of the picture.
        /// </summary>
        protected string pictureName;
        /// <summary>
        /// Gets or sets the name of the picture.
        /// </summary>
        /// <value>
        /// The name of the picture.
        /// </value>
        [CategoryAttribute("General"), Description("Name of the image"), ReadOnly(true)]
        public string PictureName
        {
            get { return pictureName; }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PictureMarker()
        {
            type = "PictureMarker";
            similarityThreshold = 0.7;
            vector = new Vector3D(0, 0, 0);
            translationVector = new Vector3D(0, 0, 0);
            rotationVector = new Vector3Di(0, 0, 0, 0);
            Augmentations = new List<AbstractAugmentation>();
            sensorCosID = IDFactory.CreateNewSensorCosID(this);
            fuser = new MarkerFuser();
            picturePath = null;
            pictureName = "";
            size = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imagePath">Full pathname of the image file.</param>
        public PictureMarker(string picturePath) : this()
        {
            size = new Bitmap(picturePath).Height * new Bitmap(picturePath).Width;
            this.picturePath = picturePath;
            pictureName = Path.GetFileName(picturePath);
        }

        /// <summary>
        /// An overwriting method, to accept a <see cref="AbstractProjectVisitor" />
        /// which must be implemented according to the visitor design pattern.
        /// It lets the visitor visit every augmentation associated with it.
        /// </summary>
        /// <param name="visitor">the visitor which encapsulates the action
        /// which is performed on this element</param>
        public override void Accept(AbstractProjectVisitor visitor)
        {
            visitor.Visit(this);
            foreach (AbstractAugmentation augmentation in Augmentations)
            {
                augmentation.Accept(visitor);
            }
            fuser.Accept(visitor);
        }

        /// <summary>
        /// returns a <see cref="Bitmap" /> in order to be displayed
        /// on the PreviewPanel, implements <see cref="IPreviewable" />
        /// </summary>
        /// <returns>
        /// a representative Bitmap
        /// </returns>
        /// <exception cref="FileNotFoundException">If ImagePath is
        ///     not correct.</exception>
        public override Bitmap getPreview()
        {
           return new Bitmap(PicturePath);
        }


        /// <summary>
        /// returns a <see cref="Bitmap" /> in order to be displayed
        /// on the ElementSelectionPanel, implements <see cref="IPreviewable" />
        /// </summary>
        /// <returns>
        /// a representative iconized Bitmap
        /// </returns>
        public override System.Drawing.Bitmap getIcon()
        {
            return Properties.Resources.ARMarker_small_;
        }

        /**
         * <summary>    Makes a deep copy of this object. </summary>
         *
         * <remarks>    Robin, 22.01.2014. </remarks>
         *
         * <returns>    A copy of this object. </returns>
         */

        public override object Clone()
        {
            PictureMarker n = ObjectCopier.Clone<PictureMarker>(this);
            n.sensorCosID = IDFactory.CreateNewSensorCosID(this);
            return n;
        }

        public override bool initElement(EditorWindow ew)
        {
            if (base.initElement(ew))
            {
                bool isInitOk = true;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|PPM Files (*.ppm)|*.ppm|PGM Files (*.pgm)|*.pgm";
                isInitOk = openFileDialog.ShowDialog() == DialogResult.OK;
                if (isInitOk)
                {
                    string path = openFileDialog.FileName;
                    bool isClonedMarker = PicturePath != null;
                    PicturePath = path;


                    if (!ew.project.existTrackable(this))
                    {
                        ew.project.Sensor = new MarkerlessSensor();
                    }
                    else
                    {
                        if (!isClonedMarker)
                        {
                            MessageBox.Show("You can't use the same marker in different Scenes.");
                            PicturePath = null;
                        }
                        return initElement(ew);
                    }

                }
                return isInitOk;
            }
            else
            {
                return false;
            }
        }
    }
}
