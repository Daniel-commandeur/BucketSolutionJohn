using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerLibrary
{
    public enum RainBarrelCapacity: int
    {
        Big = 120,
        Middle = 80,
        Small = 60
    }
    public class RainBarrel: Container
    {
        public RainBarrel(RainBarrelCapacity capacity)
        {
            base.Capacity = capacity.ToInt();
        }
        public RainBarrel(RainBarrelCapacity capacity, int content)
            :this(capacity)
        {
            this.Content = content;
        }
    }

    //Extension Methods:
    public static class EnumExtensions
    {
        public static int ToInt(this RainBarrelCapacity capacity)
        {
            return (int)capacity;
        }
        public static RainBarrelCapacity ToRainBarrelCapacity(this int i)
        {
            RainBarrelCapacity result;
            switch (i)
            {
                case (int)RainBarrelCapacity.Big:
                    result = RainBarrelCapacity.Big;
                    break;
                case (int)RainBarrelCapacity.Middle:
                    result = RainBarrelCapacity.Middle;
                    break;
                case (int)RainBarrelCapacity.Small:
                    result = RainBarrelCapacity.Small;
                    break;
                default:
                    throw new InvalidCastException();
            }
            return result;
        }
    }
}
