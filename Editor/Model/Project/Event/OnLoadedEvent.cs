﻿using ARdevKit.Model.Project.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARdevKit.Model.Project.Event
{
    public class OnLoadedEvent : AbstractEvent
    {
        public OnLoadedEvent(string augmentationID)
            : base(augmentationID)
        {
            head = augmentationID + ".onLoaded = function()";
            content = new string[] { "/*This function is called when the arel.Oject is loaded.", "TODO Add your code here*/" };
        }
    }
}