Feature: Metric Calculator
	User can convert between metrics using Calculator Web App:
	https://js-calculator.nakov.repl.co/ from the [Metric Calculator] tab.

Scenario: Convert meters to centimeters
	Given the first number is 5.3
	And the source metric is "m"
	And the destination metric is "cm"
	When the conversion is performed
	Then the result should be 530

Scenario: Convert meters to milimeters
	Given the first number is 5.3
	And the source metric is "m"
	And the destination metric is "mm"
	When the conversion is performed
	Then the result should be 5300

Scenario: Convert kilometers to meters
	Given the first number is 53
	And the source metric is "m"
	And the destination metric is "km"
	When the conversion is performed
	Then the result should be 0.053

Scenario: Convert milimetres to kilometers
	Given the first number is 1
	And the source metric is "mm"
	And the destination metric is "km"
	When the conversion is performed
	Then the result should be 0.000001