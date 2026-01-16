// **********************************************************************
// *         © COPYRIGHT 2018 Autodesk, Inc.All Rights Reserved         *
// *                                                                    *
// *  Use of this software is subject to the terms of the Autodesk      *
// *  license agreement provided at the time of installation            *
// *  or download, or which otherwise accompanies this software         *
// *  in either electronic or hard copy form.                           *
// **********************************************************************

using System;
using System.Linq;
using Autodesk.FileSystem;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Autodesk.Geometry.Test.GeometricEntities
{
    [TestFixture]
    public class DMTModelReaderTest
    {
        [Test]
        public void WhenAppendingBinarySTLFile_ThenCheckOutput()
        {
            DMTModel importedModel = DMTModelReader.ReadFile(new File(TestFiles.NormalStl));

            // Ensure that model is loaded correctly
            ClassicAssert.AreEqual(importedModel.BoundingBox.MaxX, -54);
            ClassicAssert.AreEqual(importedModel.BoundingBox.MaxY, 111);
            ClassicAssert.AreEqual(importedModel.BoundingBox.MaxZ, 70);
            ClassicAssert.AreEqual(importedModel.BoundingBox.MinX, -124);
            ClassicAssert.AreEqual(importedModel.BoundingBox.MinY, 41);
            ClassicAssert.AreEqual(importedModel.BoundingBox.MinZ, 0);
            ClassicAssert.AreEqual(importedModel.TotalNoOfTriangles, 12);
            ClassicAssert.AreEqual(importedModel.TotalNoOfVertices, 8);
        }

        [Test]
        public void WhenAppendingDMTFile_ThenCheckOutput()
        {
            DMTModel importedModel = DMTModelReader.ReadFile(new File(TestFiles.NormalDmt));

            // Ensure that model is loaded correctly
            ClassicAssert.AreEqual(-54, importedModel.BoundingBox.MaxX.Value);
            ClassicAssert.AreEqual(111, importedModel.BoundingBox.MaxY.Value);
            ClassicAssert.AreEqual(70, importedModel.BoundingBox.MaxZ.Value);
            ClassicAssert.AreEqual(-124, importedModel.BoundingBox.MinX.Value);
            ClassicAssert.AreEqual(41, importedModel.BoundingBox.MinY.Value);
            ClassicAssert.AreEqual(0, importedModel.BoundingBox.MinZ.Value);
            ClassicAssert.AreEqual(12, importedModel.TotalNoOfTriangles);
            ClassicAssert.AreEqual(8, importedModel.TotalNoOfVertices);
            ClassicAssert.AreEqual(8, importedModel.TriangleBlocks.First().VertexNormals.Count);
        }

        [Test]
        public void WhenAppendingBinarySTLFile_ThenEnsureDuplicatePointsRemoval()
        {
            var inputFile = new File(TestFiles.FetchTestFile("GetTrianglesByMesh.stl"));
            DMTModel mainMesh = DMTModelReader.ReadFile(inputFile);

            ClassicAssert.AreEqual(944, mainMesh.TotalNoOfTriangles);
            ClassicAssert.AreEqual(944, mainMesh.TotalNoOfVertices);

            var exportedFile = File.CreateTemporaryFile("stl", true);
            DMTModelWriter.WriteFile(mainMesh, exportedFile);

            DMTModel exportedMesh = DMTModelReader.ReadFile(inputFile);
            ClassicAssert.AreEqual(944, exportedMesh.TotalNoOfTriangles);
            ClassicAssert.AreEqual(944, exportedMesh.TotalNoOfVertices);
        }

        [Test]
        public void WhenReadingDMTWithTwoTriangleBlocks_ThenCheckOutput()
        {
            var inputFile = new File(TestFiles.FetchTestFile("TwoBlocks.dmt"));
            DMTModel mainMesh = DMTModelReader.ReadFile(inputFile);

            ClassicAssert.AreEqual(4, mainMesh.TotalNoOfTriangles);
            ClassicAssert.AreEqual(8, mainMesh.TotalNoOfVertices);
            ClassicAssert.AreEqual(-62.08, Math.Round(mainMesh.BoundingBox.MinX, 2));
            ClassicAssert.AreEqual(78.64, Math.Round(mainMesh.BoundingBox.MaxX, 2));
            ClassicAssert.AreEqual(5.70, Math.Round(mainMesh.BoundingBox.MinY, 2));
            ClassicAssert.AreEqual(76.08, Math.Round(mainMesh.BoundingBox.MaxY, 2));
            ClassicAssert.AreEqual(0, Math.Truncate(mainMesh.BoundingBox.MinZ));
            ClassicAssert.AreEqual(0, Math.Truncate(mainMesh.BoundingBox.MaxZ));
        }
    }
}