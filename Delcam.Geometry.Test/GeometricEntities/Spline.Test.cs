// **********************************************************************
// *         � COPYRIGHT 2018 Autodesk, Inc.All Rights Reserved         *
// *                                                                    *
// *  Use of this software is subject to the terms of the Autodesk      *
// *  license agreement provided at the time of installation            *
// *  or download, or which otherwise accompanies this software         *
// *  in either electronic or hard copy form.                           *
// **********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Autodesk.Geometry.Test.GeometricEntities
{
    [TestFixture]
    public class SplineTest
    {
        [Test]
        public void ReadFromDUCTPictureFileTest()
        {
            Spline splineCurve =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("PicFromCompCurve.pic")))[0];
            ClassicAssert.AreEqual(26, splineCurve.Count);
        }

        [Test]
        public void WriteToDUCTPictureFileTest()
        {
            FileSystem.File tempFile = FileSystem.File.CreateTemporaryFile("pic");
            List<SplinePoint> points = new List<SplinePoint>();
            points.Add(new SplinePoint(new Point(0, 0, 0)));
            points.Add(new SplinePoint(new Point(10, 5, 2)));
            points.Add(new SplinePoint(new Point(20, 9, 5)));
            Spline splineCurve = new Spline(points);
            splineCurve.FreeCurveDirections();
            splineCurve.FreeCurveMagnitudes();

            splineCurve.WriteToDUCTPictureFile(tempFile);
            Spline resultSpline = Spline.ReadFromDUCTPictureFile(tempFile)[0];
            tempFile.Delete();
            ClassicAssert.AreEqual(splineCurve.Count, resultSpline.Count);
            ClassicAssert.AreEqual(splineCurve[0].X.Value, resultSpline[0].X.Value);
            ClassicAssert.AreEqual(splineCurve[0].Y.Value, resultSpline[0].Y.Value);
            ClassicAssert.AreEqual(splineCurve[0].Z.Value, resultSpline[0].Z.Value);
            ClassicAssert.AreEqual(splineCurve[1].X.Value, resultSpline[1].X.Value);
            ClassicAssert.AreEqual(splineCurve[1].Y.Value, resultSpline[1].Y.Value);
            ClassicAssert.AreEqual(splineCurve[1].Z.Value, resultSpline[1].Z.Value);
            ClassicAssert.AreEqual(splineCurve[2].X.Value, resultSpline[2].X.Value);
            ClassicAssert.AreEqual(splineCurve[2].Y.Value, resultSpline[2].Y.Value);
            ClassicAssert.AreEqual(splineCurve[2].Z.Value, resultSpline[2].Z.Value);
            ClassicAssert.IsTrue(splineCurve[0].DirectionAfter.Equals(resultSpline[0].DirectionAfter, 5));
            ClassicAssert.IsTrue(splineCurve[0].MagnitudeAfter.Equals(resultSpline[0].MagnitudeAfter, 5));
            ClassicAssert.IsTrue(splineCurve[1].DirectionBefore.Equals(resultSpline[1].DirectionBefore, 5));
            ClassicAssert.IsTrue(splineCurve[1].DirectionAfter.Equals(resultSpline[1].DirectionAfter, 5));
            ClassicAssert.IsTrue(splineCurve[1].MagnitudeBefore.Equals(resultSpline[1].MagnitudeBefore, 5));
            ClassicAssert.IsTrue(splineCurve[1].MagnitudeAfter.Equals(resultSpline[1].MagnitudeAfter, 4));
            ClassicAssert.IsTrue(splineCurve[2].DirectionBefore.Equals(resultSpline[2].DirectionBefore, 5));
            ClassicAssert.IsTrue(splineCurve[2].MagnitudeBefore.Equals(resultSpline[2].MagnitudeBefore, 5));
        }

        [Test]
        public void SpanControlPointGetTest()
        {
            List<SplinePoint> points = new List<SplinePoint>();
            points.Add(new SplinePoint(new Point(0, 0, 0)));
            points.Add(new SplinePoint(new Point(10, 5, 2)));
            points.Add(new SplinePoint(new Point(20, 9, 5)));
            Spline splineCurve = new Spline(points);
            splineCurve.FreeCurveDirections();
            splineCurve.FreeCurveMagnitudes();

            ClassicAssert.AreEqual(new Point(0, 0, 0), splineCurve.GetSpanControlPoint(0, 0));
            ClassicAssert.AreEqual(new Point(10, 5, 2), splineCurve.GetSpanControlPoint(0, 3));

            ClassicAssert.AreEqual(splineCurve.GetSpanControlPoint(0, 1),
                            splineCurve[0] + splineCurve[0].DirectionAfter * splineCurve[0].MagnitudeAfter);
            ClassicAssert.AreEqual(splineCurve.GetSpanControlPoint(0, 2),
                            splineCurve[1] - splineCurve[1].DirectionBefore * splineCurve[1].MagnitudeBefore);
        }

        [Test]
        public void SpanControlPointSetTest()
        {
            Spline spline =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            spline.FreeCurveDirections();
            spline.FreeCurveMagnitudes();

            Spline splineCopy =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            splineCopy.FreeCurveDirections();
            splineCopy.FreeCurveMagnitudes();

            splineCopy.SetSpanControlPoint(new Point(9, 9, 0), 0, 0);
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartControlPoint.Equals(splineCopy.GetBezierCurve(0).StartControlPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndControlPoint.Equals(splineCopy.GetBezierCurve(0).EndControlPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndPoint.Equals(splineCopy.GetBezierCurve(0).EndPoint, 5));

            splineCopy =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            splineCopy.FreeCurveDirections();
            splineCopy.FreeCurveMagnitudes();
            splineCopy.SetSpanControlPoint(new Point(9, 9, 0), 0, 1);
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartPoint.Equals(splineCopy.GetBezierCurve(0).StartPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndControlPoint.Equals(splineCopy.GetBezierCurve(0).EndControlPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndPoint.Equals(splineCopy.GetBezierCurve(0).EndPoint, 5));

            splineCopy =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            splineCopy.FreeCurveDirections();
            splineCopy.FreeCurveMagnitudes();
            splineCopy.SetSpanControlPoint(new Point(9, 9, 0), 0, 2);
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartPoint.Equals(splineCopy.GetBezierCurve(0).StartPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartControlPoint.Equals(splineCopy.GetBezierCurve(0).StartControlPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndPoint.Equals(splineCopy.GetBezierCurve(0).EndPoint, 5));

            splineCopy =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            splineCopy.FreeCurveDirections();
            splineCopy.FreeCurveMagnitudes();
            splineCopy.SetSpanControlPoint(new Point(9, 9, 0), 0, 3);
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartPoint.Equals(splineCopy.GetBezierCurve(0).StartPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).StartControlPoint.Equals(splineCopy.GetBezierCurve(0).StartControlPoint, 5));
            ClassicAssert.IsTrue(spline.GetBezierCurve(0).EndControlPoint.Equals(splineCopy.GetBezierCurve(0).EndControlPoint, 5));
        }

        [Test]
        public void BezierCurveTest()
        {
            Point startPoint = new Point();
            Point startControl = new Point(1, 2, 3);
            Point endPoint = new Point(4, 5, 6);
            Point endControl = new Point(3, 3, 4);

            SplinePoint splineStartPoint = new SplinePoint(startPoint, null, startControl);
            SplinePoint splineEndPoint = new SplinePoint(endPoint, endControl, null);
            Spline spline = new Spline();
            spline.Add(splineStartPoint);
            spline.Add(splineEndPoint);

            CubicBezier splineAsBezier = spline.GetBezierCurve(0);
            ClassicAssert.AreEqual(startPoint, splineAsBezier.StartPoint);
            ClassicAssert.AreEqual(startControl, splineAsBezier.StartControlPoint);
            ClassicAssert.AreEqual(endPoint, splineAsBezier.EndPoint);
            ClassicAssert.AreEqual(endControl, splineAsBezier.EndControlPoint);
        }

        [Test]
        public void FreeCurveDirectionsTest()
        {
            Spline spline =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            spline.FreeCurveDirections();

            Spline splineProcessedByPowershape =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("FreedByPowershape.pic")))[0];

            // Compare the directions
            ClassicAssert.IsTrue(spline[0].DirectionAfter.Equals(splineProcessedByPowershape[0].DirectionAfter, 5));
            ClassicAssert.IsTrue(
                spline[spline.Count - 1].DirectionBefore.Equals(
                    splineProcessedByPowershape[splineProcessedByPowershape.Count - 1].DirectionBefore,
                    5));
            for (int i = 1; i <= spline.Count - 2; i++)
            {
                ClassicAssert.IsTrue(spline[i].DirectionBefore.Equals(splineProcessedByPowershape[i].DirectionBefore, 5));
                ClassicAssert.IsTrue(spline[i].DirectionAfter.Equals(splineProcessedByPowershape[i].DirectionAfter, 5));
            }
        }

        [Test]
        public void FreeCurveMagnitudesTest()
        {
            Spline spline =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            spline.FreeCurveDirections();
            spline.FreeCurveMagnitudes();

            Spline splineProcessedByPowershape =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("FreedByPowershape.pic")))[0];

            // Compare the magnitudes
            ClassicAssert.AreEqual(spline[0].MagnitudeAfter.Value, splineProcessedByPowershape[0].MagnitudeAfter.Value, 1E-05);
            ClassicAssert.AreEqual(spline[spline.Count - 1].MagnitudeBefore.Value,
                            splineProcessedByPowershape[spline.Count - 1].MagnitudeBefore.Value,
                            1E-05);
            for (int i = 1; i <= spline.Count - 2; i++)
            {
                ClassicAssert.AreEqual(spline[i].MagnitudeBefore.Value, splineProcessedByPowershape[i].MagnitudeBefore.Value, 1E-05);
                ClassicAssert.AreEqual(spline[i].MagnitudeAfter.Value, splineProcessedByPowershape[i].MagnitudeAfter.Value, 1E-05);
            }
        }

        [Test]
        public void FreePointMagnitudesTest()
        {
            Spline splineProcessedByPowershape =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("FreedByPowershape.pic")))[0];

            Spline spline1 =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            spline1.FreeCurveDirections();
            for (int i = 0; i <= spline1.Count - 1; i++)
            {
                spline1.FreePointMagnitudes(i);
            }

            // Compare the magnitudes
            ClassicAssert.AreEqual(spline1[0].MagnitudeAfter.Value, splineProcessedByPowershape[0].MagnitudeAfter.Value, 0.0001);
            ClassicAssert.AreEqual(spline1[spline1.Count - 1].MagnitudeBefore.Value,
                            splineProcessedByPowershape[spline1.Count - 1].MagnitudeBefore.Value,
                            0.0001);
            for (int i = 1; i <= spline1.Count - 2; i++)
            {
                ClassicAssert.AreEqual(spline1[i].MagnitudeBefore.Value, splineProcessedByPowershape[i].MagnitudeBefore.Value, 0.0001);
                ClassicAssert.AreEqual(spline1[i].MagnitudeAfter.Value, splineProcessedByPowershape[i].MagnitudeAfter.Value, 0.0001);
            }
        }

        [Test]
        public void ExtendTest()
        {
            Spline spline =
                Spline.ReadFromDUCTPictureFile(
                    new FileSystem.File(TestFiles.FetchTestFile("RawPoints.pic")))[0];
            spline.FreeCurveDirections();
            spline.FreeCurveMagnitudes();

            Spline splineByExtension = new Spline();

            foreach (SplinePoint point in spline)
            {
                splineByExtension.AddPointToEndOfSpline(point.Clone(), true);
            }

            ClassicAssert.AreEqual(spline[0].MagnitudeAfter, splineByExtension[0].MagnitudeAfter);
            ClassicAssert.AreEqual(spline[spline.Count - 1].MagnitudeBefore, splineByExtension[spline.Count - 1].MagnitudeBefore);
            for (int i = 1; i <= spline.Count - 2; i++)
            {
                ClassicAssert.AreEqual(spline[i].MagnitudeBefore, splineByExtension[i].MagnitudeBefore);
                ClassicAssert.AreEqual(spline[i].MagnitudeAfter, splineByExtension[i].MagnitudeAfter);
            }
            ClassicAssert.AreEqual(spline[0].DirectionAfter, splineByExtension[0].DirectionAfter);
            ClassicAssert.AreEqual(spline[spline.Count - 1].DirectionBefore, splineByExtension[spline.Count - 1].DirectionBefore);
            for (int i = 1; i <= spline.Count - 2; i++)
            {
                ClassicAssert.AreEqual(spline[i].DirectionBefore, splineByExtension[i].DirectionBefore);
                ClassicAssert.AreEqual(spline[i].DirectionAfter, splineByExtension[i].DirectionAfter);
            }
        }

        [Test]
        public void ReverseTest()
        {
            Spline spline = new Spline();
            spline.Add(new SplinePoint(new Point(0, 0, 0), new Vector(0.5, 0.5, 0.5), 3, new Vector(0.6, 0.6, 0.6), 5));
            spline.Add(new SplinePoint(new Point(10, 20, 30), new Vector(0, 1, 0), 3, new Vector(0, 1, 0), 5));
            spline.Add(new SplinePoint(new Point(20, 40, 50), new Vector(-1, -1, -1), 3, new Vector(-0.9, -0.9, -0.9), 5));

            spline.Reverse();

            ClassicAssert.AreEqual(3, spline.Count);

            ClassicAssert.AreEqual((MM) (-0.6), spline[2].DirectionBefore.I);
            ClassicAssert.AreEqual((MM) (-0.6), spline[2].DirectionBefore.J);
            ClassicAssert.AreEqual((MM) (-0.6), spline[2].DirectionBefore.K);

            ClassicAssert.AreEqual((MM) 1, spline[0].DirectionAfter.I);
            ClassicAssert.AreEqual((MM) 1, spline[0].DirectionAfter.J);
            ClassicAssert.AreEqual((MM) 1, spline[0].DirectionAfter.K);
        }

        [Test]
        public void ReverseTest2()
        {
            Spline spline = new Spline();
            spline.Add(new SplinePoint(new Point(0, 0, 0), new Vector(0.5, 0.5, 0.5), 3, new Vector(0.6, 0.6, 0.6), 5));
            spline.Add(new SplinePoint(new Point(10, 20, 30), new Vector(0, 1, 0), 3, new Vector(0, 1, 0), 5));
            spline.Add(new SplinePoint(new Point(20, 40, 50), new Vector(-1, -1, -1), 3, new Vector(-0.9, -0.9, -0.9), 5));

            spline.Reverse(1, 2);

            ClassicAssert.AreEqual(3, spline.Count);

            ClassicAssert.AreEqual((MM) 0.0, spline[2].DirectionBefore.I);
            ClassicAssert.AreEqual((MM) (-1), spline[2].DirectionBefore.J);
            ClassicAssert.AreEqual((MM) 0, spline[2].DirectionBefore.K);

            ClassicAssert.AreEqual((MM) 0.6, spline[0].DirectionAfter.I);
            ClassicAssert.AreEqual((MM) 0.6, spline[0].DirectionAfter.J);
            ClassicAssert.AreEqual((MM) 0.6, spline[0].DirectionAfter.K);
        }
    }
}