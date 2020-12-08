using System;

namespace ContainerLibrary
{
    public abstract class Container
    {
        #region Properties

        private int content;
        protected int overflow;
        private const int minimumContent = 0;
        protected int allowedFillAmount = 0;

        public int Content
        {
            get { return content; }
            set {
                if (value < minimumContent)
                {
                    content = minimumContent;
                }
                else
                {
                    if (value > Capacity)
                    {
                        //Overflowing can be cancelled
                        //Amount to overflow could be set
                        var overflowingEventArgs = new OverflowingEventArgs(value - Content);
                        overflowingEventArgs.AllowedFillAmount = value - content;
                        Overflowing?.Invoke(this, overflowingEventArgs);
                        //TODO: check if handler has recreated and passed a new instance of OverflowingEventArgs with an invalid FillAmount

                        if (!overflowingEventArgs.Cancel)
                        {
                            overflow = 0;
                            if (content + overflowingEventArgs.AllowedFillAmount > Capacity)
                            {
                                overflow = content + overflowingEventArgs.AllowedFillAmount - Capacity;
                                content = Capacity;
                            }
                            else
                            {
                                content += overflowingEventArgs.AllowedFillAmount;
                            }
                            allowedFillAmount = overflowingEventArgs.AllowedFillAmount;
                        }
                        else
                        {
                            allowedFillAmount = 0;
                        }
                    }
                    else
                    {
                        allowedFillAmount = value - content;
                        content = value;
                    }


                    if (content >= Capacity)
                    {
                        Full?.Invoke(this, null);
                    }

                    if (overflow > 0)
                    {
                        Overflowed?.Invoke(this, new OverflowedEventArgs(overflow));
                    }
                }

            }
        }

        public int Capacity { get; protected set; }

        #endregion
        
        #region Methods

        public void Fill(int amount)
        {
            this.Content += amount;
        }

        public void Empty()
        {
            this.Content = minimumContent;
        }
        public void Empty(int amount)
        {
            this.Content -= amount;
        }


        #endregion

        #region Events
        public event EventHandler<OverflowingEventArgs> Overflowing;
        public event EventHandler<OverflowedEventArgs> Overflowed;
        public event EventHandler<EventArgs> Full;

        #endregion
    }
}
