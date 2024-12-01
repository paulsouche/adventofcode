package main

import (
	"fmt"
	"os"
	"regexp"
	"slices"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(data string) [][]int {
	lines := strings.Split(data, "\n")
	r, _ := regexp.Compile(`\d+`)
	return arrays.Map(lines, func(line string, _ int) []int {
		return arrays.Map(r.FindAllString(line, -1), func(level string, _ int) int {
			return numbers.SafeConvertStrToInt(level)
		})
	})
}

func areLevelsSafe(levels []int, _ int) (valid bool) {
	asc := levels[0] < levels[1]
	desc := levels[0] > levels[1]

	valid = asc || desc

	for i := 0; i < len(levels)-1; i++ {
		current := levels[i]
		next := levels[i+1]
		diff := numbers.AbsDiffInt(current, next)

		if asc && next <= current {
			valid = false
			break
		}

		if desc && next >= current {
			valid = false
			break
		}

		if diff < 1 || diff > 3 {
			valid = false
			break
		}
	}

	return
}

func generateTestCases(levels []int) (testCases [][]int) {
	testCases = append(testCases, levels)
	testCases = append(testCases, arrays.Map(levels, func(_ int, i int) []int {
		return slices.Delete(slices.Clone(levels), i, i+1)
	})...)
	return
}

func Part1(data string) int {
	reports := parse(data)
	valids := arrays.Filter(reports, areLevelsSafe)
	return len(valids)
}

func Part2(data string) int {
	reports := parse(data)
	valids := arrays.Filter(reports, func(levels []int, _ int) bool {
		return arrays.Some(generateTestCases(levels), areLevelsSafe)
	})
	return len(valids)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
