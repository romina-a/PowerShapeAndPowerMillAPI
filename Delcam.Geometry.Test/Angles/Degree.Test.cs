// **********************************************************************
// *         � COPYRIGHT 2018 Autodesk, Inc.All Rights Reserved         *
// *                                                                    *
// *  Use of this software is subject to the terms of the Autodesk      *
// *  license agreement provided at the time of installation            *
// *  or download, or which otherwise accompanies this software         *
// *  in either electronic or hard copy form.                           *
// **********************************************************************

using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Autodesk.Geometry.Test.Angles
{
    [TestFixture]
    public class DegreeTest
    {
        #region "Test Context"

        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #endregion

        #region "Type conversion tests"

        [Test]
        public void DegreeToDouble()
        {
            Degree angle = new Degree(1.0);
            double angleDouble = angle;
            ClassicAssert.AreEqual(angleDouble, 1.0);
        }

        [Test]
        public void DegreeToSingle()
        {
            Degree angle = new Degree(1.0);
            float angleSingle = (float) angle;
            ClassicAssert.AreEqual(angleSingle, 1.0);
        }

        [Test]
        public void RadianToDegree()
        {
            Radian angleRadian = new Radian(1.0);
            Degree angleDegree = angleRadian;
            ClassicAssert.AreEqual(angleDegree.Value, 1.0 * 180.0 / Math.PI);
        }

        [Test]
        public void DoubleToDegree()
        {
            Degree angle = 1.0;
            ClassicAssert.AreEqual(angle.Value, 1.0);
        }

        #endregion

        #region "Operator tests"

        [Test]
        public void NegateTest()
        {
            Degree angle = 1;
            angle = -angle;
            ClassicAssert.AreEqual(angle.Value, -1);
        }

        [Test]
        public void SubtractionTest()
        {
            Degree angle1 = new Degree(3.0);
            Degree angle2 = new Degree(2.0);
            Degree difference = angle1 - angle2;
            ClassicAssert.AreEqual(difference.Value, 1.0);
        }

        [Test]
        public void AdditionTest()
        {
            Degree angle1 = new Degree(3.0);
            Degree angle2 = new Degree(2.0);
            Degree sum = angle1 + angle2;
            ClassicAssert.AreEqual(sum.Value, 5.0);
        }

        [Test]
        public void DivisionTest()
        {
            Degree angle1 = new Degree(6.0);
            Degree result = angle1 / 3.0;

            double delta = result - 2.0;
            if (delta < 0.0)
            {
                delta *= -1;
            }
            ClassicAssert.IsTrue(delta <= double.Epsilon);
        }

        [Test]
        public void MultiplicationTest()
        {
            Degree angle1 = new Degree(3.0);
            Degree angle2 = new Degree(2.0);
            ClassicAssert.AreEqual((angle1 * angle2).Value, 6.0);
            ClassicAssert.AreEqual((angle1 * 4.0).Value, 12.0);
            ClassicAssert.AreEqual((4.0 * angle1).Value, 12.0);
        }

        [Test]
        public void LessThanTest()
        {
            Degree angle = new Degree(3.0);
            ClassicAssert.IsTrue(angle < 4.0);
            ClassicAssert.IsFalse(angle < 2.0);

            ClassicAssert.IsTrue(angle < new Degree(4.0));
            ClassicAssert.IsFalse(angle < new Degree(2.0));
        }

        [Test]
        public void GreaterThanTest()
        {
            Degree angle = new Degree(3.0);
            ClassicAssert.IsFalse(angle > 4.0);
            ClassicAssert.IsTrue(angle > 2.0);

            ClassicAssert.IsFalse(angle > new Degree(4.0));
            ClassicAssert.IsTrue(angle > new Degree(2.0));
        }

        [Test]
        public void LessThanOrEqualTest()
        {
            Degree angle = new Degree(3.0);
            ClassicAssert.IsTrue(angle <= 3.0);
            ClassicAssert.IsFalse(angle <= 2.0);

            ClassicAssert.IsTrue(angle <= new Degree(3.0));
            ClassicAssert.IsFalse(angle <= new Degree(2.0));
        }

        [Test]
        public void GreaterThanOrEqualTest()
        {
            Degree angle = new Degree(3.0);
            ClassicAssert.IsTrue(angle >= 3.0);
            ClassicAssert.IsFalse(angle >= 4.0);

            ClassicAssert.IsTrue(angle >= new Degree(3.0));
            ClassicAssert.IsFalse(angle >= new Degree(4.0));
        }

        [Test]
        public void EqualityTest()
        {
            ClassicAssert.IsTrue(new Degree(4.0) == new Degree(4.0));
            ClassicAssert.IsFalse(new Degree(4.0) == new Degree(4.1));
        }

        [Test]
        public void InequalityTest()
        {
            ClassicAssert.IsFalse(new Degree(4.0) != new Degree(4.0));
            ClassicAssert.IsTrue(new Degree(4.0) != new Degree(4.1));
        }

        #endregion

        #region "Method tests"

        [Test]
        public void ToStringTest()
        {
            Degree angle = new Degree(1.23);

            ClassicAssert.AreEqual(angle.ToString(), 1.23.ToString());

            ClassicAssert.AreEqual(angle.ToString("N3"), 1.23.ToString("N3"));

            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("de-DE");
            ClassicAssert.AreEqual(angle.ToString(culture), 1.23.ToString(culture));

            ClassicAssert.AreEqual(angle.ToString("N3", culture), 1.23.ToString("N3", culture));
        }

        [Test]
        public void EqualsTest()
        {
            Radian angleRadian = new Radian(1.0);
            Degree angleDegree = angleRadian;
            ClassicAssert.IsTrue(angleDegree.Equals(angleRadian));
            ClassicAssert.IsTrue(angleDegree.Equals(angleDegree));
            ClassicAssert.IsTrue(angleDegree.Equals(angleRadian, 10));
        }

        [Test]
        public void CompareToTest()
        {
            Degree angle1 = new Degree(2.0);
            Degree angle2 = new Degree(2.0);
            Degree angle3 = new Degree(3.0);

            ClassicAssert.AreEqual(angle1.CompareTo(angle2), angle1.Value.CompareTo(angle2.Value));
            ClassicAssert.AreEqual(angle1.CompareTo(angle3), angle1.Value.CompareTo(angle3.Value));
            ClassicAssert.AreEqual(angle3.CompareTo(angle1), angle3.Value.CompareTo(angle1.Value));
        }

        #endregion
    }
}