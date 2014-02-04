﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ARdevKit.Controller.Connections.DeviceConnection;

namespace ARdevKit
{
    public partial class DeviceSelectionWindow : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The device connection controller.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private DeviceConnectionController deviceConnectionController;
        private int index;

        public DeviceSelectionWindow()
        {
            deviceConnectionController = new DeviceConnectionController(this);
            index = 0;
            InitializeComponent();
            List<string> devices = deviceConnectionController.getPossibleClients();
            foreach (string device in devices)
            {
                deviceList.Items.Add(new ListViewItem(device));                
            }
        }

        private void DeviceSelectionWindow_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void connectTo_Click(object sender, EventArgs e)
        {
            deviceConnectionController.connectToDevice(index);
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            deviceList.Items.Clear();
            List<string> devices = deviceConnectionController.getPossibleClients();
            foreach (string device in devices)
            {
                deviceList.Items.Add(new ListViewItem(device));                
            }
        }

        private void deviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = deviceList.FocusedItem.Index;
        }
    }
}
