﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.StorageClient;
using Sitereactor.CloudProviders.Azure;

namespace Sitereactor.CloudProviders.Tests
{
    /// <summary>
    /// Summary description for AzureTests
    /// </summary>
    [TestClass]
    public class AzureTests
    {
        private BlobFactory _factory;
        public AzureTests()
        {
            _factory = new BlobFactory("account name", "key");
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Test_Create_New_Container()
        {
            string container = _factory.CreatePublicContainer("cdn");
            Assert.IsNotNull(container);
        }

        [TestMethod]
        public void Test_Get_Containers()
        {
            List<CloudBlobContainer> containers = _factory.GetAllBlobContainers();

            Assert.IsTrue(containers.Any());
        }
    }
}
