using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium_Testing.PageObjects
{
    class HomePage : BasePage
    {
        public HomePage(RemoteWebDriver driver) : base(driver) { }

        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/";
        public IWebElement ElementStudentsCount => driver.FindElementByCssSelector("body > p > b");
        public int GetStudentsCount => int.Parse(ElementStudentsCount.Text);
        
    }
}
