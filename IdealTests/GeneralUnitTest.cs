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
        private String SAMPLE_DATABASE = "";


        [TestInitialize]
        public void Initialization()
        {
            // Find the sample database path
            string proyectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            SAMPLE_DATABASE = proyectPath.Replace("IdealTests", "Ideal\\SampleDatabase.mdf");
        }

        [TestCleanup]
        public void CleanUp()
        {
            SAMPLE_DATABASE = "";
        }


        /// <summary>
        /// Checks that the sample database is there.
        /// </summary>
        [TestMethod]
        public void TestSampleDatabaseExists()
        {
            Assert.AreEqual(true, File.Exists(SAMPLE_DATABASE));
        }
    }
}
