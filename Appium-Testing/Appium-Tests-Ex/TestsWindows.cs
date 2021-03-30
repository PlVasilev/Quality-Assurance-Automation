using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Appium_Tests_Ex
{
    public class TestsWindows
    {
        private WindowsDriver<WindowsElement> _driver;
        private WindowsDriver<WindowsElement> _driverDesktop;
        private const string AppiumServerUri = "http://[::1]:4723/wd/hub";
        private const string AppForTesting = @"C:\Program Files\7-Zip\7zFM.exe";
        private const string PathForTesting = @"C:\Program Files\7-Zip\";
        private string _workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions {PlatformName = "Windows" };
            options.AddAdditionalCapability("app", AppForTesting);
            _driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUri), options);

            // Session for Root app (windows Desktop)
            var optionsDesktop = new AppiumOptions { PlatformName = "Windows" };
            optionsDesktop.AddAdditionalCapability("app", "Root");
            _driverDesktop = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUri), optionsDesktop);

            _workDir = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "workDir";
            if (Directory.Exists(_workDir))
                Directory.Delete(_workDir, true);
            Directory.CreateDirectory(_workDir);
        }

        [OneTimeTearDown]
        public void Test1()
        {
            _driver.Quit();
        }

        [Test]
        public void Test7Zip()
        {
            // var texBox = _driver.FindElementByXPath("/Window/Pane/Pane/ComboBox/Edit");
            var texBox = _driver.FindElementByClassName("Edit");
            texBox.SendKeys(@"C:\Program Files\7-Zip\");
            texBox.SendKeys(Keys.Enter);

            var listBoxFiles = _driver.FindElementByClassName("SysListView32");
            listBoxFiles.SendKeys(Keys.Control + "a");

            var buttonAdd = _driver.FindElementByName("Add");
            buttonAdd.Click();
            Thread.Sleep(500);
            
            // Make an Archive
            var windowCreateArchive =  _driverDesktop.FindElementByName("Add to Archive");

            var textBoxArchiveName = windowCreateArchive.FindElementByXPath( "/Window/ComboBox/Edit[@Name='Archive:']");
            string archiveName = _workDir + @"\" + "archive.7z";
            textBoxArchiveName.SendKeys(archiveName);

            var textBoxArchiveArchiveFormat = windowCreateArchive.FindElementByXPath("/Window/ComboBox[@Name='Archive format:']");
            textBoxArchiveArchiveFormat.SendKeys("7z");
            var textBoxCompressionLevel = windowCreateArchive.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']");
            textBoxCompressionLevel.SendKeys("Ultra");
            var textBoxCompressionMethod = windowCreateArchive.FindElementByXPath("/Window/ComboBox[@Name='Compression method:']");
            textBoxCompressionMethod.SendKeys(Keys.Home);
            var textBoxDictionarySize = windowCreateArchive.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']");
            textBoxDictionarySize.SendKeys(Keys.End);
            var textBoxWordSize = windowCreateArchive.FindElementByXPath("/Window/ComboBox[@Name='Word size:']");
            textBoxWordSize.SendKeys(Keys.End);
            var buttonArchiveOk = windowCreateArchive.FindElementByXPath("/Window/Button[@Name='OK']");
            buttonArchiveOk.Click();

            // Extract the Archive
            texBox.SendKeys(archiveName + Keys.Enter);
            var buttonExtract = _driver.FindElementByName("Extract");
            buttonExtract.Click();
            var buttonExtractOk = _driver.FindElementByName("OK");
            buttonExtractOk.Click();
            Thread.Sleep(500);

            //Assert Files are the same
            foreach (var fileOriginal in Directory.EnumerateFiles(PathForTesting, "*.*", SearchOption.AllDirectories))
            {
                var fileNameOnly = fileOriginal.Replace(PathForTesting, "");
                var fileCopy = _workDir + @"\" + fileNameOnly;
                FileAssert.AreEqual(fileCopy,fileOriginal);
            }
        }
    }
}