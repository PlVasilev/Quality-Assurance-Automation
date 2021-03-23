using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium_Testing.PageObjects
{
    class AddStudentPage : BasePage
    {
        public AddStudentPage(RemoteWebDriver driver) : base(driver) {}

        public override string PageUrl =>
            "https://mvc-app-node-express.nakov.repl.co/add-student";

        public IWebElement TextBoxName => driver.FindElementByCssSelector("input#name");
        public IWebElement TextBoxEmail => driver.FindElementByCssSelector("input#email");
        public IWebElement ButtonAdd => driver.FindElementByCssSelector("form > button");
        public IWebElement ErrorBoxText => driver.FindElementByXPath("//div[contains(.,'Cannot add student. Name and email fields are required!')]");
        public IWebElement ErrorBoxStyle => driver.FindElementByXPath("//div[contains(@style,'background:red')]");

        public void CreateNewStudent(string name, string email)
        {
            TextBoxName.Clear();
            TextBoxName.SendKeys(name);
            TextBoxEmail.Clear();
            TextBoxEmail.SendKeys(email);
            ButtonAdd.Click();
        }
    }
}
