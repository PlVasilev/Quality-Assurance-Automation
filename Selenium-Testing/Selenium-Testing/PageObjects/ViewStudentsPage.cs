using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium_Testing.PageObjects
{
    class ViewStudentsPage : BasePage
    {
        public ViewStudentsPage(RemoteWebDriver driver) : base(driver) { }

        public override string PageUrl => "https://mvc-app-node-express.nakov.repl.co/students";
        public IWebElement ListStudents => driver.FindElementByCssSelector("body > ul");
        public IReadOnlyCollection<IWebElement> ListOfStudents => driver.FindElementsByCssSelector("body > ul > li");
        public string[] GetRegisteredStudents => ListOfStudents.Select(s => s.Text).ToArray();

    }
}
