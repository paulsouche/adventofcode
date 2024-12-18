package main

import (
	"adventofcode/common/arrays"
	"fmt"
	"os"
	"strings"
)

func parse(input string) (availableTowels map[string]bool, desiredPatterns []string) {
	rawInput := strings.Split(input, "\n\n")
	availableTowels = arrays.ToSet(strings.Split(rawInput[0], ", "))
	desiredPatterns = strings.Split(rawInput[1], "\n")
	return
}

func isPatternPossible(pattern string, availableTowels *map[string]bool) bool {
	val, exists := (*availableTowels)[pattern]

	if exists {
		return val
	}

	for chars := len(pattern); chars > 0; chars-- {
		subPattern := pattern[:chars]

		if (*availableTowels)[subPattern] {
			if chars == len(pattern) {
				return true
			} else if isPatternPossible(pattern[chars:], availableTowels) {
				(*availableTowels)[pattern[chars:]] = true
				return true
			}
		}
	}

	(*availableTowels)[pattern] = false

	return false
}

func countPossibleCombinations(pattern string, availableTowels *map[string]bool, countByTowels *map[string]int) int {
	val, exists := (*countByTowels)[pattern]

	if exists {
		return val
	}

	for chars := len(pattern); chars > 0; chars-- {
		subPattern := pattern[:chars]

		if (*availableTowels)[subPattern] {
			if chars == len(pattern) {
				(*countByTowels)[pattern]++
			} else {
				(*countByTowels)[pattern] += countPossibleCombinations(pattern[chars:], availableTowels, countByTowels)
			}
		}
	}

	return (*countByTowels)[pattern]
}

func Part1(data string) int {
	availableTowels, desiredPatterns := parse(data)

	return len(arrays.Filter(desiredPatterns, func(pattern string, _ int) bool {
		return isPatternPossible(pattern, &availableTowels)
	}))
}

func Part2(data string) int {
	availableTowels, desiredPatterns := parse(data)

	return arrays.SumInts(arrays.Map(desiredPatterns, func(pattern string, _ int) int {
		countByTowels := make(map[string]int)
		return countPossibleCombinations(pattern, &availableTowels, &countByTowels)
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
