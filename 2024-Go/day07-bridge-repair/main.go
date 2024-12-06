package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

type Equation struct {
	TestValue int
	Operators []int
}

func parse(data string) []Equation {
	return arrays.Map(strings.Split(data, "\n"), func(line string, _ int) (equation Equation) {
		rawLine := strings.Split(line, ": ")
		equation = Equation{
			TestValue: numbers.SafeConvertStrToInt(rawLine[0]),
			Operators: arrays.Map(strings.Split(rawLine[1], " "), func(number string, _ int) int {
				return numbers.SafeConvertStrToInt(number)
			}),
		}
		return
	})
}

func testResults(testValue int, operators []int, withConcatenation bool) bool {
	if testValue < 0 {
		return false
	}

	if len(operators) == 0 {
		return testValue == 0
	}

	current := operators[0]
	next := testValue / current
	isDivisible := testValue == next*current
	if isDivisible && testResults(next, operators[1:], withConcatenation) {
		return true
	}

	if withConcatenation {
		next, found := strings.CutSuffix(strconv.Itoa(testValue), strconv.Itoa(current))
		if found && len(next) != 0 {
			if testResults(numbers.SafeConvertStrToInt(next), operators[1:], withConcatenation) {
				return true
			}
		}
	}

	return testResults(testValue-current, operators[1:], withConcatenation)
}

func buildIsValid(withConcatenation bool) func(Equation, int) bool {
	return func(equation Equation, _ int) bool {
		return testResults(equation.TestValue, arrays.Reverse(equation.Operators), withConcatenation)
	}
}

func solution(valids []Equation) int {
	return arrays.SumInts(arrays.Map(valids, func(equation Equation, _ int) int {
		return equation.TestValue
	}))
}

func Part1(data string) int {
	equations := parse(data)

	valids := arrays.Filter(equations, buildIsValid(false))

	return solution(valids)
}

func Part2(data string) int {
	equations := parse(data)

	valids := arrays.Filter(equations, buildIsValid(true))

	return solution(valids)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
