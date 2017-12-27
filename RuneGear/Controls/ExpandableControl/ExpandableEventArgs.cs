using System;

namespace RuneGear.Controls.ExpandableControl
{
    public class ExpandableEventArgs : EventArgs
    {
        public bool IsExpanded { get; private set; }

        public ExpandableEventArgs(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }
    }
}
