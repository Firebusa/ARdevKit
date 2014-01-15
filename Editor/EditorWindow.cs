﻿////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	editorwindow.cs
//
// summary:	Implements the editorwindow class
// This is the main window of the entire program, as well as the main entrance point.
// The Editor Window controlls the main tasks and delegates complex task to the other dedicated controllers.
// It also controls the communication between the model and the view, meaning it builds the view and saves/deletes/changes data in the model.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ARdevKit.Model.Project;
using Controller.EditorController;
using ARdevKit.Controller.ProjectController;
using ARdevKit.Controller.EditorController;
using ARdevKit.Controller.Connections.DeviceConnection;
using ARdevKit.Controller.TestController;

namespace ARdevKit
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Form for viewing the editor. This is the main form of the program.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class EditorWindow : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// if true the debug window will be opened when starting the test mode on the device.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool startDebugModeDevice;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// if true the debug window will be opened when starting the test mode locally.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool startDebugModeLocal;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Categories the element belongs to.
        /// List of scene element categories. 
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<SceneElementCategory> elementCategories;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The path of the current project
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private string projectPath;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Linked list containing all IPreviewables.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private LinkedList<IPreviewable> allElements;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The element selection controller.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private ElementSelectionController elementSelectionController;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The preview controller.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private PreviewController previewController;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the project. </summary>
        ///
        /// <value> The project. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Project project { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The property controller.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private PropertyController propertyController;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The device connection controller.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private DeviceConnectionController deviceConnectionController;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The save visitor.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private SaveVisitor saveVisitor;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The export visitor.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private ExportVisitor exportVisitor;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The current element.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private IPreviewable currentElement;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        ///
        /// <value>
        /// The current element.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal IPreviewable CurrentElement
        {
            get { return currentElement; }
            set { currentElement = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the export visitor.
        /// </summary>
        ///
        /// <value>
        /// The export visitor.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal ExportVisitor ExportVisitor
        {
            get { return exportVisitor; }
            set { exportVisitor = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the save visitor.
        /// </summary>
        ///
        /// <value>
        /// The save visitor.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal SaveVisitor SaveVisitor
        {
            get { return saveVisitor; }
            set { saveVisitor = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets allElements.
        /// </summary>
        ///
        /// <value>
        /// Linked list containing elements.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal LinkedList<IPreviewable> AllElements
        {
            get { return allElements; }
            set { allElements = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets a value indicating whether to start debug mode if test mode is started on the
        /// device.
        /// </summary>
        ///
        /// <value>
        /// if true the debug window will be opened when starting the test mode on the device.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool StartDebugModeDevice
        {
            get { return startDebugModeDevice; }
            set { startDebugModeDevice = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets a value indicating whether to start debug mode locally.
        /// </summary>
        ///
        /// <value>
        /// if true the debug window will be opened when starting the test mode locally.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal bool StartDebugModeLocal
        {
            get { return startDebugModeLocal; }
            set { startDebugModeLocal = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the categories the element belongs to.
        /// </summary>
        ///
        /// <value>
        /// The element categories.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        internal List<SceneElementCategory> ElementCategories
        {
            get { return elementCategories; }
            set { elementCategories = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the full pathname of the project file.
        /// </summary>
        ///
        /// <value>
        /// The full pathname of the project file.
        /// </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ProjectPath
        {
            get { return projectPath; }
            set { projectPath = value; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor. initializes components on startup.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public EditorWindow()
        {
            InitializeComponent();
            allElements = new LinkedList<IPreviewable>();
            this.project = new Project();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by Editor for load events. actions to do when the editor is loaded.
        /// </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Editor_Load(object sender, EventArgs e)
        {
            //stub
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_file_new for click events.
        /// </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void tsm_editor_menu_file_new_Click(object sender, EventArgs e)
        {
            //stub
            throw new NotImplementedException();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_file_exit for click events. exits the program.
        /// </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void tsm_editor_menu_file_exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_test_startWithImage for click events. Starts the test
        /// mode for images.
        /// </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void tsm_editor_menu_test_startImage_Click(object sender, EventArgs e)
        {
            if (projectPath == null)
                TestController.StartWithImage();
            else
                TestController.StartWithImage(projectPath);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_test_startWithVideo for click events. Starts the test
        /// mode for images.
        /// </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void tsm_editor_menu_test_startVideo_Click(object sender, EventArgs e)
        {
            if (projectPath == null)
                TestController.StartWithVideo();
            else
                TestController.StartWithVideo(projectPath);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_test_startWithVirtualCamera for click events. Starts the test
        /// mode using a virtual camera.
        /// </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void tsm_editor_menu_test_startWithVirtualCamera_Click(object sender, EventArgs e)
        {
            if (projectPath == null)
                TestController.StartWithVirtualCamera();
            else
                TestController.StartWithVirtualCamera(projectPath);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by tsm_editor_menu_file_open for click events.
        /// </summary>
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void tsm_editor_menu_file_open_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void addDevice()
        {
            //TODO: implement addDevice()
        }

        public void createNewProject(String name)
        {
            //TODO: implement createNewProject(String name)
        }

        public void exportProject()
        {
            //TODO: implement exportProject()
        }

        public void loadProject()
        {
            //TODO: implement loadProject()
        }

        public void openDebugWindow()
        {
            //TODO: implement openDebugWindow()
        }

        public void openTestWindow()
        {
            //TODO: implement openTestWindow()
        }

        public void registerElements()
        {
            //TODO: implement registerElements()
        }

        public void saveProject()
        {
            //TODO: implement saveProject()
        }

        public void sendToDevice()
        {
            //TODO: implement sendToDevice()
        }

        public void updateElementSelectionPanel()
        {
            //TODO: implement updateElementSelectionPanel()
        }

        public void updatePreviewPanel()
        {
            //TODO: implement updatePreviewPanel()
        }

        internal void updatePropertyPanel(IPreviewable selectedElement)
        {
            //TODO: implement updatePropertyPanel(IPreviewable selectedElement)
        }

        public void updateSceneSelectionPanel()
        {
            //TODO: implement updateSceneSelectionPanel()
        }

        public void updateStatusBar()
        {
            //TODO: implement updateStatusBar()
        }

        private void addCategory(SceneElementCategory category)
        {
            //TODO: implement addCategory(SceneElementCategory category)
        }
    }
}
