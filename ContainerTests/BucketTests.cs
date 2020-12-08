using ContainerLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContainerTests
{
    [TestClass]
    public class BucketUnitTests
    {
        [TestMethod]
        public void DefaultCapacity()
        {
            //Arrange
            Bucket b;
            var expectedCapacity = 12; 
            //Act
            b = new Bucket();
            //Assert
            Assert.IsTrue(expectedCapacity == b.Capacity);
        }

        [TestMethod]
        public void DefaultContent()
        {
            var b = new Bucket();
            Assert.IsTrue(b.Content == 0);
        }

        [TestMethod]
        public void ContentNewBucket()
        {
            var content = 10;
            var b = new Bucket(content);
            Assert.IsTrue(b.Content == content);
        }
        [TestMethod]
        public void ContentCapacityNewBucket()
        {
            var content = 10;
            var capacity = 20;
            var b = new Bucket(content, capacity);
            Assert.IsTrue(b.Content == content && b.Capacity == capacity);
        }

        [TestMethod]
        public void SetContent()
        {
            var b = new Bucket();
            b.Content = 10;
            Assert.AreEqual(b.Content, 10);
        }

        [TestMethod]
        public void AmountFill()
        {
            Bucket b;
            b = new Bucket();
            var amount = 8;

            b.Fill(amount);
            Assert.IsTrue(b.Content == amount);
        }

        [TestMethod]
        public void FullFill()
        {
            Bucket b;
            b = new Bucket();
            var amount = b.Capacity;

            b.Fill(amount);
            Assert.IsTrue(b.Content == amount);
        }

        [TestMethod]
        public void Empty()
        {
            var b = new Bucket(10,12);
            b.Empty();
            Assert.IsTrue(b.Content == 0);
        }

        [TestMethod]
        public void AmountEmpty()
        {
            var content = 10;
            var b = new Bucket(12);
            b.Empty(content);
            Assert.IsTrue(b.Content == 2);
        }

        [TestMethod]
        public void OverflowFullFill()
        {
            Bucket b;
            b = new Bucket();
            var fillAmount = 20;

            b.Fill(fillAmount);
            Assert.IsTrue(b.Content == b.Capacity);
        }

        [TestMethod]
        public void FullEvent()
        {
            var eventFullIsTriggered = false;
            var b = new Bucket();
            b.Full += (o, e) => { eventFullIsTriggered = true; };
            b.Content = b.Capacity;
            Assert.IsTrue(eventFullIsTriggered);
        }

        [TestMethod]
        public void OverflowedEvent()
        {
            var overflowAmount = 10;
            var actualOverflowAmount = 0;
            var b = new Bucket();
            b.Overflowed += (o, e)=> {
                actualOverflowAmount = e.OverflowAmount;
            };
            b.Content = b.Capacity + overflowAmount;
            Assert.AreEqual(actualOverflowAmount, overflowAmount);
        }

        [TestMethod]
        public void OverflowingEvent()
        {
            var overflowAmount = 10;
            var actualOverflowAmount = 0;
            var b = new Bucket();
            b.Overflowing += (o, e)=> {
                e.Cancel = false;
            };
            b.Overflowed += (o, e)=> {
                actualOverflowAmount = e.OverflowAmount;
            };

            b.Content = b.Capacity + overflowAmount;
            Assert.AreEqual(actualOverflowAmount, overflowAmount);
        }
        [TestMethod]
        public void CancelOverflowingEvent()
        {
            var overflowAmount = 10;
            var b = new Bucket();
            b.Overflowing += (o, e)=> {
                e.Cancel = true;
            };
            b.Content = b.Capacity + overflowAmount;
            Assert.IsTrue(b.Content==0);
        }

        [TestMethod]
        public void AllowedFillAmount()
        {
            var fillAmount = 20;
            var allowedFillAmount = 5;

            var b = new Bucket();
            b.Overflowing += (o, e)=> {
                e.Cancel = false;
                e.AllowedFillAmount = allowedFillAmount;
            };
            
            b.Fill(fillAmount);

            Assert.AreEqual(b.Content, allowedFillAmount);
        }


        [TestMethod]
        public void OverflowingAllowedFillAmount()
        {
            var fillAmount = 20;
            var allowedFillAmount = 15;

            var b = new Bucket();
            b.Overflowing += (o, e) => {
                e.Cancel = false;
                e.AllowedFillAmount = allowedFillAmount;
            };

            b.Fill(fillAmount);

            Assert.AreEqual(b.Content, b.Capacity);
        }

        [TestMethod]
        public void OverflowedOverflowingAllowedFillAmount()
        {
            var fillAmount = 20;
            var allowedFillAmount = 15;
            var overflowed = 0;

            var b = new Bucket();
            b.Overflowing += (o, e) => {
                e.Cancel = false;
                e.AllowedFillAmount = allowedFillAmount;
            };

            b.Overflowed += (o, e) => { overflowed = e.OverflowAmount; };
            b.Fill(fillAmount);

            Assert.AreEqual(b.Content + overflowed, allowedFillAmount);
        }

        [TestMethod]
        public void BucketFill()
        {
            var content = 10;
            var expectedB1Content = 10;
            var expectedB2Content = 0;
            var b1 = new Bucket();
            var b2 = new Bucket(content);
            b1.Fill(b2);
            Assert.IsTrue(b1.Content == expectedB1Content && b2.Content == expectedB2Content);
        }

        [TestMethod]
        public void CancelBucketFill()
        {
            var b1 = new Bucket(20, 20);
            var b2 = new Bucket();
            b2.Overflowing += (o, e) => { e.Cancel = true; };
            b2.Fill(b1);
            Assert.IsTrue(b1.Content == 20 && b2.Content == 0); 
        }

        [TestMethod]
        public void AllowedBucketFill()
        {
            var b1 = new Bucket(20,20);
            var b2 = new Bucket();
            var b2Overflowed = false;
            var expectedB1Content = 5;
            var expectedB2Content = 12;
            var expectedB2Overflow = 3;
            var actualB2Overflow = 0;

            b2.Overflowed += (o, e) => {
                b2Overflowed = true;
                actualB2Overflow = e.OverflowAmount;
            };
            b2.Overflowing += (o, e) => { e.Cancel = false; e.AllowedFillAmount = 15; };

            b2.Fill(b1);

            Assert.IsTrue(b2Overflowed == true && 
                b1.Content == expectedB1Content && 
                expectedB2Overflow == actualB2Overflow &&
                b2.Content == expectedB2Content
                );
        }
    }
}
