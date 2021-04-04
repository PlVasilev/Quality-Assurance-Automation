using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace DataDriven_Testing
{
    class SeleniumCalculator
    {
        private RemoteWebDriver _driver;
        private IWebElement _textBoxNum1;
        private IWebElement _textBoxOperator;
        private IWebElement _textBoxNum2;
        private IWebElement _buttonCalculate;
        private IWebElement _buttonReset;
        private IWebElement _result;


        [OneTimeSetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://number-calculator.nakov.repl.co/");
            _textBoxNum1 = _driver.FindElement(By.Id("number1"));
            _textBoxOperator = _driver.FindElement(By.Id("operation"));
            _textBoxNum2 = _driver.FindElement(By.Id("number2"));
            _buttonCalculate = _driver.FindElement(By.Id("calcButton"));
            _buttonReset = _driver.FindElement(By.Id("resetButton"));
            _result = _driver.FindElement(By.Id("result"));
        }

        //Valid Int
        [TestCase("5", "+" , "3", "Result: 8")]
        [TestCase("5", "*" , "3", "Result: 15")]
        [TestCase("5", "-" , "3", "Result: 2")]
        [TestCase("6", "/" , "3", "Result: 2")]

        //Valid Double
        [TestCase("5.11", "+", "3.32", "Result: 8.43")]
        [TestCase("5.5", "*", "3.5", "Result: 19.25")]
        [TestCase("5.13", "-", "2.53", "Result: 2.6")]
        [TestCase("0.75", "/", "2.5", "Result: 0.3")]
        
        //Invalid num1
        [TestCase("", "+" , "3", "Result: invalid input")]
        [TestCase("", "-" , "3", "Result: invalid input")]
        [TestCase("", "*" , "3", "Result: invalid input")]
        [TestCase("", "/" , "3", "Result: invalid input")]
        [TestCase("aaaa", "/" , "3", "Result: invalid input")]

        //Invalid operator
        [TestCase("2", "", "3", "Result: invalid operation")]
        [TestCase("2", "aaa", "3", "Result: invalid operation")]

        //Invalid num2
        [TestCase("3", "+", "", "Result: invalid input")]
        [TestCase("3", "-", "", "Result: invalid input")]
        [TestCase("3", "*", "", "Result: invalid input")]
        [TestCase("3", "/", "", "Result: invalid input")]
        [TestCase("3", "/", "aaaa", "Result: invalid input")]

        //Infinity num1
        [TestCase("Infinity", "+", "1", "Result: Infinity")]
        [TestCase("Infinity", "-", "1", "Result: Infinity")]
        [TestCase("Infinity", "*", "1", "Result: Infinity")]
        [TestCase("Infinity", "/", "1", "Result: Infinity")]

        //Infinity num2
        [TestCase("1", "+", "Infinity", "Result: Infinity")]
        [TestCase("1", "-", "Infinity", "Result: -Infinity")]
        [TestCase("1", "*", "Infinity", "Result: Infinity")]
        [TestCase("1", "/", "Infinity", "Result: 0")]

        //Infinity num1 num2
        [TestCase("Infinity", "+", "Infinity", "Result: Infinity")]
        [TestCase("Infinity", "-", "Infinity", "Result: invalid calculation")]
        [TestCase("Infinity", "*", "Infinity", "Result: Infinity")]
        [TestCase("Infinity", "/", "Infinity", "Result: invalid calculation")]

        //Valid Exponential
        [TestCase("2.5e53", "*", "2", "Result: 5e+53")]

        public void Test_CalculatorWebApp(string num1,string op, string num2, string expectedResult)
        {
            _buttonReset.Click();
            if (num1 != "") _textBoxNum1.SendKeys(num1);
            if (op != "") _textBoxOperator.SendKeys(op);
            if (num2 != "") _textBoxNum2.SendKeys(num2);
            _buttonCalculate.Click();
            var actualResult = _result.Text;
            Assert.AreEqual(expectedResult,actualResult);
        }

        [OneTimeTearDown]
        public void TearDown() => _driver.Quit();
        


    }
}
