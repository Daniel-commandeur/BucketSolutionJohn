using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerLibrary
{
    public class Bucket: Container
    {
        private const int minimumBucketCapacity = 10;
        private const int defaultBucketCapacity = 12;
        
        public Bucket()
            :this(0, defaultBucketCapacity) {}

        public Bucket(int content)
            :this(content, defaultBucketCapacity) {}
        
        public Bucket(int content, int capacity)
        {
            base.Capacity = capacity > minimumBucketCapacity ? capacity : minimumBucketCapacity;
            Content = content;
        }
        
        public void Fill(Bucket bucket)
        {
            this.Content += bucket.Content;
            //this.Fill(bucket.Content);
            bucket.Content -= allowedFillAmount;
        }
    }
}
