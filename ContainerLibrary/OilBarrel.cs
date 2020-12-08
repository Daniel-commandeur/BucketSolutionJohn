using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerLibrary
{
    public class OilBarrel: Container
    {
        private const int oilBarrelCapacity = 159;
        public OilBarrel()
        {
            base.Capacity = oilBarrelCapacity;
        }
        public OilBarrel(int content):this()
        {
            this.Content = content;
        }
    }
}
