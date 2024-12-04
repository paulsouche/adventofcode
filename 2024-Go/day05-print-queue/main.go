package main

import (
	"fmt"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(data string) (sequences [][]string, rawRules [][]string, rules map[string][]string) {
	rawData := strings.Split(data, "\n\n")

	sequences = arrays.Map(strings.Split(rawData[1], "\n"), func(rawLine string, _ int) []string {
		return strings.Split(rawLine, ",")
	})

	rawRules = arrays.Map(strings.Split(rawData[0], "\n"), func(rawLine string, _ int) []string {
		return strings.Split(rawLine, "|")
	})

	rules = make(map[string][]string)
	for _, line := range rawRules {
		rules[line[0]] = append(rules[line[0]], line[1])
	}

	return
}

func buildIsOrdered(rules map[string][]string) func([]string, int) bool {
	return func(array []string, _ int) bool {
		return arrays.Every(array, func(page string, index int) bool {
			return arrays.Every(rules[page], func(before string, _ int) (valid bool) {
				valid = true
				for i := 0; i < index; i++ {
					if array[i] == before {
						valid = false
						break
					}
				}
				return
			})
		})
	}
}

func buildSortSequence(rules map[string][]string) func([]string, int) []string {
	return func(sequenceToSort []string, _ int) (sortedSequence []string) {
		sequenceSet := arrays.ToSet(sequenceToSort)

		var page string
		for {
			page, sequenceToSort = sequenceToSort[0], sequenceToSort[1:]

			hasAncestor := arrays.Some(rules[page], func(pageBefore string, _ int) bool {
				_, hasPage := sequenceSet[pageBefore]
				return !arrays.Includes(sortedSequence, pageBefore) && hasPage
			})

			if hasAncestor {
				sequenceToSort = append(sequenceToSort, page)
			} else {
				sortedSequence = append(sortedSequence, page)
			}

			if len(sequenceToSort) == 0 {
				break
			}
		}
		return
	}
}

func solution(sequence [][]string) int {
	return arrays.SumInts(arrays.Map(sequence, func(seq []string, _ int) int {
		return numbers.SafeConvertStrToInt(seq[len(seq)/2])
	}))
}

func Part1(data string) int {
	sequences, _, rules := parse(data)

	isOrdered := buildIsOrdered(rules)

	valids := arrays.Filter(sequences, isOrdered)

	return solution(valids)
}

func Part2(data string) int {
	sequences, rawRules, rules := parse(data)

	isOrdered := buildIsOrdered(rules)

	invalids := arrays.Filter(sequences, func(sequence []string, ind int) bool {
		return !isOrdered(sequence, ind)
	})

	reversedRules := make(map[string][]string)
	for _, line := range rawRules {
		reversedRules[line[1]] = append(reversedRules[line[1]], line[0])
	}

	sortSequence := buildSortSequence(reversedRules)

	return solution(arrays.Map(invalids, sortSequence))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
