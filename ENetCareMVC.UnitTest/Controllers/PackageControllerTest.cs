using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Web.Controllers;
using System.Web.Mvc;
using ENetCareMVC.Web.Models;

namespace ENetCareMVC.UnitTest.Controllers
{
    [TestClass]
    public class PackageControllerTest
    {
        [TestInitialize]
        public void TestInitialize()
        {           
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../App_Data"));
            // rest of initialize implementation ...
        }
        
        [TestMethod]
        public void TestRegister()
        {
            var controller = new PackageController();

            var result = controller.Register() as ViewResult;

            var model = result.Model as PackageRegisterViewModel;

            Assert.AreEqual("Register", result.ViewName);
            Assert.IsTrue(model.StandardPackageTypes.Any());
        }
    }
}
