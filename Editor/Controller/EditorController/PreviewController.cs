﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARdevKit.Model.Project;
using System.Collections;
using ARdevKit;
using ARdevKit.Controller.EditorController;
using ARdevKit.View;

public class PreviewController
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   The MetaCategory of the current element. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public MetaCategory currentMetaCategory { get; set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   The Trackable which hold the Augmentions and Sources. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public AbstractTrackable trackable { get; set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   The PreviewPanel which we need to add Previewables. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    private Panel panel;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   EditorWindow Instanz </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    private EditorWindow ew;

    /// <summary>   The Index which Trackable out of Project we musst use </summary>
    public int index;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Constructor. </summary>
    ///
    /// <param name="ew">   EditorWindow Instanz. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public PreviewController(EditorWindow ew)
    {
        this.ew = ew;
        this.panel = this.ew.Pnl_editor_preview;
        this.currentMetaCategory = new MetaCategory();
        this.index = 0;
        this.trackable = null;
        this.ew.project.Trackables.Add(trackable);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   (This method is obsolete) adds a preview able. </summary>
    ///
    /// <exception cref="NotImplementedException"> Thrown when the requested operation is
    /// unimplemented. </exception>
    ///
    /// <param name="p">    The Panel to process. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [Obsolete("addPreviewable(IPreviewable p) : eache IPreviewable needs a Vector where the new"
                + "Previewable should sit in the panel you should use addPreviewable(IPreviewable"
                + "currentElement, Vector3d v) for Augmentions & Trackables", true)]
    public void addPreviewAble(IPreviewable p)
        { throw new NotImplementedException(); }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     add Trackable is the method for adding the trackable, each PreviewPanel can holding one
    ///     Trackable.
    /// </summary>
    ///
    /// <summary>   currentMetaCategory musst set to Trackable/Augmention</summary>
    ///
    /// <param name="currentTrackable"> The current Trackable, which should set in the previewPanel. </param>
    /// <param name="v">                The Vector3D to set the Trackable. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public void addPreviewable(IPreviewable currentElement, Vector3D v)
    {
        if (currentMetaCategory == MetaCategory.Trackable && trackable == null)
        {
            

            //ask the user for the picture (if the trackable is a picturemarker)
            if (currentElement.GetType() == typeof(PictureMarker))
            {
                OpenFileDialog openTestImageDialog = new OpenFileDialog();
                openTestImageDialog.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|PPM Files (*.ppm)|*.ppm|PGM Files (*.pgm)|*.pgm";
                if (openTestImageDialog.ShowDialog() == DialogResult.OK)
                {
                    ((PictureMarker)currentElement).ImagePath = openTestImageDialog.FileName;
                    
                    //set the vector to the trackable
                    ((AbstractTrackable)currentElement).vector = v;
                    this.trackable = (AbstractTrackable)currentElement;
                    this.ew.project.Trackables[index] = (AbstractTrackable)currentElement;
                    this.addPictureBox(currentElement, v);
                    if (this.ew.project.Trackables.Count == 1 && this.ew.project.Trackables[0] != null)
                    {
                        this.ew.ElementSelectionController.setElementEnable(typeof(IDMarker), false);
                    }
                    
                }
            }
            else {
                //set the vector to the trackable
                    ((AbstractTrackable)currentElement).vector = v;
                    this.trackable = (AbstractTrackable)currentElement;
                    this.ew.project.Trackables[index] = (AbstractTrackable)currentElement;
                    this.addPictureBox(currentElement, v);
                    if (this.ew.project.Trackables.Count == 1 && this.ew.project.Trackables[0] != null)
                    {
                        this.ew.ElementSelectionController.setElementEnable(typeof(PictureMarker), false);
                    }
                    
            }
            
        }
        else if (currentMetaCategory == MetaCategory.Augmentation && trackable != null && this.ew.project.Trackables[index].Augmentions.Count < 3)
        {
            //set the vector and the trackable in Augmention
            ((AbstractAugmention)currentElement).TranslationVector = v;
            ((AbstractAugmention)currentElement).Trackable = this.trackable;

            //set references 
            trackable.Augmentions.Add((AbstractAugmention)currentElement);

            this.addPictureBox(currentElement, v);

            //set the new box to the front
            this.findBox(currentElement).BringToFront();
        }
        else
        {
            MessageBox.Show("More than one Trackable & three Augmentions are not allowed!");
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     add Source or Augmention, this method can only be used with the element, which is the
    ///     over element by Augmention the overelement is Trackable. by Source the overelement is
    ///     Augmention.
    /// </summary>
    ///
    /// <summary>   currentMetaCategory musst set to Augmention</summary>
    ///
    /// <param name="currentElement">   The current element. </param>
    /// <param name="overElement">      The over element. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void addSource(AbstractSource source, AbstractAugmention currentElement)
    {
        if (currentMetaCategory == MetaCategory.Source)
        {
            if (this.trackable != null && trackable.existAugmention((AbstractAugmention)currentElement))
            {
                //set reference to the augmentions in Source
                source.augmentions.Add((Abstract2DAugmention)currentElement);

                //add references in Augmention, Picturebox + project.sources List.
                ((AbstractDynamic2DAugmention)currentElement).source = source;

                this.currentMetaCategory = MetaCategory.Augmentation;
                this.findBox(currentElement).ContextMenu.MenuItems.Add("show source", new EventHandler(this.show_source_by_click));
                this.findBox(currentElement).ContextMenu.MenuItems.Add("remove source", new EventHandler(this.remove_source_by_click));
                this.ew.project.Sources.Add(((AbstractDynamic2DAugmention)this.findBox((AbstractAugmention)currentElement).Tag).source);
                this.currentMetaCategory = MetaCategory.Source;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     Removes the choosen Source out of the Augmention and also out of the sourcesList in
    ///     Project.
    /// </summary>
    ///
    /// <summary>   currentMetaCategory musst set to Augmention </summary>
    /// 
    /// <remarks>   Lizzard, 1/15/2014. </remarks>
    ///
    /// <param name="source">           Source for the. </param>
    /// <param name="currentElement">   The current element. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void removeSource(AbstractSource source, IPreviewable currentElement)
    {
        if (currentMetaCategory == MetaCategory.Augmentation)
        {
            if (this.ew.project.findSource(source).augmentions.Count > 1)
            {
                ((AbstractDynamic2DAugmention)currentElement).source = null;
                this.ew.project.findSource(source).augmentions.Remove((Abstract2DAugmention)currentElement);
            }
            else if (this.ew.project.findSource(source).augmentions.Count == 1)
            {
                ((AbstractDynamic2DAugmention)currentElement).source = null;
                this.ew.project.Sources.Remove(source);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Removes the Previewable and the Objekt, what is linked to the Previewable. </summary>
    ///
    /// <summary>   currentMetaCategory musst set to Trackable/Augmention</summary>
    /// 
    /// <remarks>   Lizzard, 1/16/2014. </remarks>
    ///
    /// <param name="currentElement">   The current element. </param>
    ///
    /// ### <param name="p">    The p control. </param>
    /// ### <param name="prev"> The previous. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void removePreviewable(IPreviewable currentElement)
    {
        if (currentMetaCategory == MetaCategory.Trackable && trackable != null)
        {
            this.removeAll();
            if (!this.ew.project.isTrackable())
            {
                this.ew.ElementSelectionController.setElementEnable(typeof(PictureMarker), true);
                this.ew.ElementSelectionController.setElementEnable(typeof(IDMarker), true);
            }
        }
        else if (currentMetaCategory == MetaCategory.Augmentation && trackable != null)
        {
            this.trackable.Augmentions.Remove((AbstractAugmention)currentElement);
            this.panel.Controls.Remove(this.findBox((AbstractAugmention)currentElement));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>  Removes all Elements from the PreviewPanel and clear all lists and dictionarys </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void removeAll()
    {
        this.panel.Controls.Clear();
        this.trackable = null;
        this.ew.project.Trackables[index] = null;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     Move the Trackable whith all connected Augmentions &amp; sources to the new vector-
    ///     position.
    /// </summary>
    ///
    /// <summary>   currentMetaCategory musst set to Trackable/Augmention</summary>
    ///
    /// <param name="currentTrackable"> The current Trackable, which should set in the previewPanel. </param>
    /// <param name="v">                The Vector3D to move the Trackable. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void moveElement(IPreviewable currentElement, Vector3D v)
    {
        if (currentMetaCategory == MetaCategory.Trackable && trackable != null)
        {
            ((AbstractTrackable)this.findBox(currentElement).Tag).vector = v;
            this.findBox(currentElement).Location = new Point(v.X, v.Y);
        }
        else if (currentMetaCategory == MetaCategory.Augmentation && trackable != null)
        {
            ((AbstractAugmention)this.findBox(currentElement).Tag).TranslationVector = v;
            this.findBox(currentElement).Location = new Point(v.X, v.Y);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>    (This method is obsolete) updates the preview panel. </summary>
    ///
    /// <exception cref="NotImplementedException"> Thrown when the requested operation is
    /// unimplemented. </exception>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void updatePreviewPanel()
    {
        this.panel.Controls.Clear();
        this.ew.project.Trackables.Add(trackable);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   This Reload funktion is here to load a other Trackable out of the Project. </summary>
    ///
    /// <remarks>   Lizzard, 1/15/2014. </remarks>
    ///
    /// <param name="index">    The Index which Trackable out of Project we musst use. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public void reloadPreviewPanel(int index)
    {
        //if it's the same Scene do nothing
        if (index == this.index)
        { }
        //if it's a scene which exists reload scene
        else if (index < this.ew.project.Trackables.Count)
        {

            this.index = index;
            this.trackable = this.ew.project.Trackables[index];
            this.panel.Controls.Clear();
            if (trackable != null)
            {
                this.addAllToPanel(this.ew.project.Trackables[index]);
            }
        }
        //if the scene is new create a new empty scene
        else if (index >= this.ew.project.Trackables.Count)
        {
            this.index = index;
            this.trackable = null;
            this.panel.Controls.Clear();
            this.ew.project.Trackables.Add(trackable);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     Add all existent Objects of the trackable in the Panel, this funktion is exists for change
    ///     the trackable.
    /// </summary>
    ///
    /// <remarks>   Lizzard, 1/15/2014. </remarks>
    ///
    /// <param name="trackable">    The Trackable which hold the Augmentions and Sources. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void addAllToPanel(AbstractTrackable trackable)
    {
        if (trackable.Augmentions.Count > 0)
        {
            foreach (AbstractAugmention aug in trackable.Augmentions)
            {
                this.addPictureBox(aug, aug.TranslationVector);
            }
        }
        this.addPictureBox(trackable, trackable.vector);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Adds a PictureBox with for the currentElement to the aktuell Scene. </summary>
    ///
    /// <remarks>   Lizzard, 1/17/2014. </remarks>
    ///
    /// <param name="prev">     The previous. </param>
    /// <param name="vector">   The vector. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void addPictureBox(IPreviewable prev, Vector3D vector)
    {
        PictureBox tempBox;
        tempBox = new PictureBox();
        tempBox.Location = new Point(vector.X - prev.getPreview().Height / 8, vector.Y - prev.getPreview().Width / 8);
        tempBox.Image = (Image)prev.getPreview();
        tempBox.Size = new Size(prev.getPreview().Height / 4, prev.getPreview().Width / 4);
        tempBox.SizeMode = PictureBoxSizeMode.StretchImage;
        tempBox.Tag = prev;

        ContextMenu cm = new ContextMenu();
        cm.MenuItems.Add("remove", new EventHandler(this.remove_by_click));
        cm.Tag = prev;

        //adds drag&drop events for augmentations so that sources can be droped on them
        if (currentMetaCategory == MetaCategory.Augmentation)
        {
            ((Control)tempBox).AllowDrop = true;
            DragEventHandler enterHandler = new DragEventHandler(onAugmentationEnter);
            DragEventHandler dropHandler = new DragEventHandler(onAugmentationDrop);
            tempBox.DragEnter += enterHandler;
            tempBox.DragDrop += dropHandler;
        }

        tempBox.ContextMenu = cm;
        tempBox.MouseClick += new MouseEventHandler(selectElement);

        if (tempBox.Tag is AbstractAugmention)
        tempBox.MouseMove += new MouseEventHandler(controlMouseMove);

        this.panel.Controls.Add(tempBox);

    }

    /**
     * <summary>    Raises the drag event when a source enters a augmentation. </summary>
     *
     * <remarks>    Robin, 19.01.2014. </remarks>
     *
     * <param name="sender">    Source of the event. </param>
     * <param name="e">         Event information to send to registered event handlers. </param>
     */

    public void onAugmentationEnter(object sender, DragEventArgs e)
    {
        if (currentMetaCategory == MetaCategory.Source)
        {
            e.Effect = DragDropEffects.Move;
        }
    }

    /**
     * <summary>    Raises the drag event when a source is droped on an augmentation. </summary>
     *
     * <remarks>    Robin, 19.01.2014. </remarks>
     *
     * <param name="sender">    Source of the event. </param>
     * <param name="e">         Event information to send to registered event handlers. </param>
     */

    public void onAugmentationDrop(object sender, DragEventArgs e)
    {
        if (currentMetaCategory == MetaCategory.Source)
        {
            ElementIcon icon = (ElementIcon)e.Data.GetData(typeof(ElementIcon));
            AbstractAugmention augmentation = (AbstractAugmention)((PictureBox)sender).Tag;
            AbstractSource source = ObjectCopier.Clone((AbstractSource)icon.Element.Dummy);
            addSource(source, augmentation);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Searchs in the Panel for the important PictureBox and gives this box back. </summary>
    ///
    /// <param name="prev"> The previous. </param>
    ///
    /// <returns>   The found box. </returns>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private PictureBox findBox(IPreviewable prev)
    {
        if (currentMetaCategory == MetaCategory.Trackable)
        {
            foreach (Control comp in panel.Controls)
            {
                if (comp.Tag == (AbstractTrackable)prev)
                {
                    return (PictureBox)comp;
                }
            }
        }
        else if (currentMetaCategory == MetaCategory.Augmentation)
        {
            foreach (Control comp in panel.Controls)
            {
                if (comp.Tag == (AbstractAugmention)prev)
                {
                    return (PictureBox)comp;
                }
            }
        }
        return null;
    }

    /// <summary>   Select element (Event). </summary>
    ///
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Mouse event information. </param>

    private void selectElement(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ew.PropertyGrid1.SelectedObject = ((Control)sender).Tag;
        }
    }

    /// <summary>   
    /// Event to move a object of type Control. 
    /// Also updates x/y coord in the Tag of the control.            
    /// </summary>
    ///
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Mouse event information. </param>
    private void controlMouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            Control controlToMove = (Control)sender;
            controlToMove.BringToFront();
            controlToMove.Location = new Point(controlToMove.Location.X + e.Location.X - 60,
               controlToMove.Location.Y + e.Location.Y - 60);

            if (((Control)sender).Tag is AbstractAugmention)
            {
                AbstractAugmention aa;
                aa = (AbstractAugmention)((Control)sender).Tag;
                aa.TranslationVector.X = controlToMove.Location.X + e.Location.X;
                aa.TranslationVector.Y = controlToMove.Location.Y + e.Location.Y;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Event handler. removes the current object. </summary>
    ///
    /// <remarks>   Lizzard, 1/19/2014. </remarks>
    ///
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void remove_by_click(object sender, EventArgs e)
    {
        IPreviewable temp = (IPreviewable)((ContextMenu)((MenuItem)sender).Parent).Tag;
        MetaCategory tempMeta = this.currentMetaCategory;
        if (temp.GetType() == this.trackable.GetType())
        {
            this.currentMetaCategory = MetaCategory.Trackable;
        }
        else
        {
            this.currentMetaCategory = MetaCategory.Augmentation;
        }

        this.removePreviewable((IPreviewable)((ContextMenu)((MenuItem)sender).Parent).Tag);
        ew.PropertyGrid1.SelectedObject = null;
        this.currentMetaCategory = tempMeta;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Event handler. Shows Source in PropertyGrid when you want this. </summary>
    ///
    /// <remarks>   Lizzard, 1/19/2014. </remarks>
    ///
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void show_source_by_click(object sender, EventArgs e)
    {
        ew.PropertyGrid1.SelectedObject = ((AbstractDynamic2DAugmention)((ContextMenu)((MenuItem)sender).Parent).Tag).source;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     Event handler. Removes the source of the augmentation and the contextmenuentries of this
    ///     augmentation.
    /// </summary>
    ///
    /// <remarks>   Lizzard, 1/19/2014. </remarks>
    ///
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Event information. </param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    private void remove_source_by_click(object sender, EventArgs e)
    {
        AbstractDynamic2DAugmention temp = (AbstractDynamic2DAugmention)((ContextMenu)((MenuItem)sender).Parent).Tag;
        MetaCategory tempMeta = currentMetaCategory;
        this.currentMetaCategory = MetaCategory.Augmentation;
        this.removeSource(temp.source, temp);
        ew.PropertyGrid1.SelectedObject = null;
        this.currentMetaCategory = tempMeta;

        this.currentMetaCategory = MetaCategory.Augmentation;
        this.findBox(temp).ContextMenu.MenuItems.RemoveAt(1);
        this.findBox(temp).ContextMenu.MenuItems.RemoveAt(1);
        this.currentMetaCategory = MetaCategory.Source;

    }
}

