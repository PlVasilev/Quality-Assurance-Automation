using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SpecFlowCalculatorTests.Steps
{
    [Binding, Scope(Feature = "Number Calculator")]
    public class NumberCalculatorSteps
    {
        private static RemoteWebDriver _driver;
        private static IWebElement _textBoxNum1;
        private static IWebElement _textBoxNum2;
        private static IWebElement _operation;
        private static IWebElement _calcButton;
        private static IWebElement _resetButton;
        private static IWebElement _calculatorLink;


        [BeforeFeature] // OneTimeSetup
        public static void OpenCalculatorApp()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://js-calculator.nakov.repl.co/");
            _calculatorLink = _driver.FindElementByXPath("//a[@href='#'][contains(.,'Number Calculator')]");
            _calculatorLink.Click();
            _textBoxNum1 = _driver.FindElementById("number1");
            _textBoxNum2 = _driver.FindElementById("number2");
            _operation = _driver.FindElementById("operation");
            _calcButton = _driver.FindElementByXPath("(//input[contains(@value,'Calculate')])[1]");
            _resetButton = _driver.FindElementByXPath("(//input[contains(@type,'reset')])[1]");
        }

        [AfterFeature] private static void CloseCalculatorApp() => _driver.Quit();

        [BeforeScenario]
        public static void ResetCalculatorApp() =>_resetButton.Click();
        

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(string firstNum) => _textBoxNum1.SendKeys(firstNum);
        

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(string secondNum) => _textBoxNum2.SendKeys(secondNum);
        

        [When("the two numbers are (.*)")]
        public void WhenTheTwoNumbersAreAdded(string operation)
        {
            var selectedOperation = new SelectElement(_operation);
            switch (operation)
            {
                case "added": selectedOperation.SelectByValue("+"); break;
                case "subtracted": selectedOperation.SelectByValue("-"); break;
                case "multiplied": selectedOperation.SelectByValue("*"); break;
                case "divided": selectedOperation.SelectByValue("/"); break;
                default: throw new InvalidOperationException($"{operation} - is Invalid Operation");
            }
            _calcButton.Click();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
            var result = _driver.FindElementByCssSelector("#screenNumberCalc > div").Text.Substring("Result: ".Length);
            result.Should().Be(expectedResult);
            //  Assert.AreEqual(result,expectedResult);
        }
    }
}
