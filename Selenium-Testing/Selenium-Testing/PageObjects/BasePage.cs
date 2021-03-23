using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium_Testing.PageObjects
{
    class BasePage
    {
        protected RemoteWebDriver driver;
        public virtual string PageUrl { get; }

        public BasePage(RemoteWebDriver driver)
        {
            this.driver = driver;
            driver.Navigate().GoToUrl(PageUrl);
        }


        public IWebElement LinkHomePage =>
            driver.FindElementByXPath("//a[contains(.,'Home')]");

        public IWebElement LinkViewStudentsPage =>
            driver.FindElementByXPath("//a[contains(.,'View Student')]");

        public IWebElement LinkAddStudentsPage =>
            driver.FindElementByXPath("//a[contains(.,'Add Student')]");

        public IWebElement ElementTextHeading =>
            driver.FindElementByCssSelector("body > h1");

        
        public string PageHeading => ElementTextHeading.Text;
        public string GetPageTitle => driver.Title;
        
        public void Open() => driver.Navigate().GoToUrl(PageUrl);
        public bool IsCurrentlyOpen() => driver.Url == this.PageUrl;
        public string GetPageHeadingText() => ElementTextHeading.Text;
       
    }
}
