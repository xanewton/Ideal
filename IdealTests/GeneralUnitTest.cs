using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdealTests
{
    /// <summary>
    /// General Tests
    /// </summary>
    [TestClass]
    public class GeneralUnitTest
    {
        private String sampleDB = "";
        private String modelDBPath = "";
        private String sampleModelDBPath = "";


        [TestInitialize]
        public void Initialization()
        {
            // Find the sample database path
            string proyectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            sampleDB = proyectPath.Replace("IdealTests", "Ideal\\SampleDatabase.mdf");
            modelDBPath = proyectPath.Replace("IdealTests", "Ideal\\DBModel");
            sampleModelDBPath = proyectPath.Replace("IdealTests", "Ideal\\SampleDBModel");
        }

        [TestCleanup]
        public void CleanUp()
        {
            sampleDB = "";
        }


        /// <summary>
        /// Checks that the sample database is there.
        /// </summary>
        [TestMethod]
        public void TestSampleDatabaseExists()
        {
            Assert.AreEqual(true, File.Exists(sampleDB));
        }

        /// <summary>
        /// Tests that the Sample database and the real database have the same tables, views.
        /// </summary>
        [TestMethod]
        public void TestDatabaseModelMatchSampleDatabase()
        {
            // Read the file names and ensure that they exist in both directories.
            foreach (string file in Directory.EnumerateFiles(modelDBPath, "*.cs"))
            {
                if (file.Contains("_TBL") || file.Contains("_VIEW"))
                {
                    String fileName = Path.GetFileName(file);
                    String sampleFile = sampleModelDBPath + "\\" + fileName;
                    Assert.AreEqual(true, File.Exists(sampleFile));
                }
            }
        }
    }
}
