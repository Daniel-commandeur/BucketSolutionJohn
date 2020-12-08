using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace ContainerLibrary
{
    public class OverflowedEventArgs : EventArgs
    {
        public OverflowedEventArgs(int amount)
        {
            this.OverflowAmount = amount;
        }
        public int OverflowAmount { get; }
    }
    public class OverflowingEventArgs : CancelEventArgs
    {
        private int allowed;
        public OverflowingEventArgs(int currentFillAmount)
        {
            this.CurrentFillAmount = currentFillAmount;
        }
        public int CurrentFillAmount { get; }

        public int AllowedFillAmount
        {
            get { return allowed; }
            set {
                allowed = 
                    value <= this.CurrentFillAmount && value >= 0 
                    ? value 
                    : throw new InvalidOperationException($"{value} is not an allowed fill amount, should be between 0 and {this.CurrentFillAmount}");
            }
        }
    }
}
