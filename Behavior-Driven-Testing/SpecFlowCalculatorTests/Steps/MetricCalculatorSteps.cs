using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SpecFlowCalculatorTests.Steps
{
    [Binding, Scope(Feature = "Metric Calculator")]
    public class MetricCalculatorSteps
    {
        private static RemoteWebDriver _driver;
        private static IWebElement _textBoxNum1;
        private static SelectElement _operation1;
        private static SelectElement _operation2;
        private static IWebElement _calcButton;
        private static IWebElement _resetButton;
        private static IWebElement _calculatorLink;


        [BeforeFeature] // OneTimeSetup
        public static void OpenCalculatorApp()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://js-calculator.nakov.repl.co/");
            _calculatorLink = _driver.FindElementByXPath("//a[@href='#'][contains(.,'Metric Calculator')]");
            _calculatorLink.Click();
            _textBoxNum1 = _driver.FindElementById("fromValue");
            _operation1 = new SelectElement(_driver.FindElementById("sourceMetric"));
            _operation2 = new SelectElement(_driver.FindElementById("destMetric"));
            _calcButton = _driver.FindElementByXPath("(//input[@type='button'])[2]");
            _resetButton = _driver.FindElementByXPath("(//input[contains(@type,'reset')])[2]");
        }

        [AfterFeature] private static void CloseCalculatorApp() => _driver.Quit();

        [BeforeScenario]
        public static void ResetCalculatorApp() =>_resetButton.Click();
        

        [Given("the first number is (.*)")]
        public void GivenInputValueIs(string inputValue) => _textBoxNum1.SendKeys(inputValue);
        

        [Given("the source metric is \"(.*)\"")]
        public void GivenTheSourceMetricIs(string sourceMetric) => _operation1.SelectByValue(sourceMetric);

        [Given("the destination metric is \"(.*)\"")]
        public void GivenTheDestinationMetricIs(string sourceMetric) => _operation2.SelectByValue(sourceMetric);


        [When("the conversion is performed")]
        public void WhenConversionIsPerformed()
        {
            _calcButton.Click();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(string expectedResult)
        {

            var resultText = _driver.FindElementByCssSelector("#screenMetricCalc > div").Text;
            var result = resultText.Substring("Result: ".Length);
            result.Should().Be(expectedResult);
            //  Assert.AreEqual(result,expectedResult);
        }
    }
}
