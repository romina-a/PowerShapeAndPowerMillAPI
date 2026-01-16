// **********************************************************************
// *         � COPYRIGHT 2018 Autodesk, Inc.All Rights Reserved         *
// *                                                                    *
// *  Use of this software is subject to the terms of the Autodesk      *
// *  license agreement provided at the time of installation            *
// *  or download, or which otherwise accompanies this software         *
// *  in either electronic or hard copy form.                           *
// **********************************************************************

using System;
using System.Globalization;
using System.Threading;
using Autodesk.FileSystem;
using Autodesk.Geometry.Test.GeometricEntities;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Autodesk.Geometry.Test.MSRFile
{
    [TestFixture]
    public class MSRFileTest
    {
        [Test]
        public void MSRFileReadTest()
        {
            Geometry.MSRFile msrFile =
                new Geometry.MSRFile(TestFiles.FetchTestFile("G330 Untransformed.msr"), true);
            ClassicAssert.AreEqual(81, msrFile.Points.Count);
        }

        [Test]
        public void MSRFileDetransformTest()
        {
            Geometry.MSRFile msrFile =
                new Geometry.MSRFile(TestFiles.FetchTestFile("G330 Untransformed.msr"), true);
            File detransformedFile =
                new File(TestFiles.FetchTestFile("G330 Detransformed.msr"));
            ClassicAssert.AreEqual(detransformedFile.ReadText(), msrFile.ToString());
        }

        [Test]
        public void MSRFileCultureAgnosticTest()
        {
            // Switches system culture to French and attempts to parse file. If safeguards against culture disparities are in place, test should pass.
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr");

            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(
                        TestFiles.FetchTestFile("Culture Agnostic.msr"),
                        true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }

            Assert.Pass();
        }

        [Test]
        public void MSRFileEmptyFileTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("Empty File.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileNoDataTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("No Data.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileOddCharactersTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("Odd Characters.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileDuplicateControlCharacterTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("Extra Ordinate.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileMissingG800Test()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(
                        TestFiles.FetchTestFile("Missing G800 Line.msr"),
                        true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileMissingG801Test()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(
                        TestFiles.FetchTestFile("Missing G801 Line.msr"),
                        true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileLineNumberErrorTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(
                        TestFiles.FetchTestFile("Line Number Error.msr"),
                        true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileTwoStartsTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("TwoStarts.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileTwoEndsTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("TwoEnds.msr"), true);
            }
            catch (Exception ex)
            {
                Assert.Pass(ex.Message);
            }
            Assert.Fail("Failed to throw exception");
        }

        [Test]
        public void MSRFileTwoStartsTogetherTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("TwoStartsOK.msr"), true);
                Assert.That(msrFile.Points.Count, Is.EqualTo(6));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.Pass("File read ok");
        }

        [Test]
        public void MSRFileTwoEndsTogetherTest()
        {
            try
            {
                Geometry.MSRFile msrFile =
                    new Geometry.MSRFile(TestFiles.FetchTestFile("TwoEndsOK.msr"), true);
                Assert.That(msrFile.Points.Count, Is.EqualTo(6));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.Pass("Failed to throw exception");
        }

        [Test]
        public void WriteFile()
        {
            Geometry.MSRFile msrFile =
                new Geometry.MSRFile(TestFiles.FetchTestFile("G330 Untransformed.msr"), true);
            ClassicAssert.AreEqual(81, msrFile.Points.Count);

            var fileToSave = new File(string.Format("{0}\\\\test.msr", System.IO.Path.GetTempPath()));
            msrFile.WriteFile(fileToSave.Path);

            Geometry.MSRFile fileSaved = new Geometry.MSRFile(fileToSave.Path, true);
            ClassicAssert.AreEqual(81, fileSaved.Points.Count);

            fileToSave.Delete();
        }
    }
}