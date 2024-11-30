package main

import (
	"fmt"
	"os"
	"regexp"
	"sort"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(data string) (locationIdsLeft []int, locationIdsRight []int) {
	lines := strings.Split(data, "\n")
	r, _ := regexp.Compile(`\d+`)
	for _, line := range lines {
		match := r.FindAllString(line, -1)
		locationIdsLeft = append(locationIdsLeft, numbers.SafeConvertStrToInt(match[0]))
		locationIdsRight = append(locationIdsRight, numbers.SafeConvertStrToInt(match[1]))
	}
	return
}

func Part1(data string) int {
	locationIdsLeft, locationIdsRight := parse(data)

	sort.Ints(locationIdsLeft)
	sort.Ints(locationIdsRight)

	distances := arrays.Map(locationIdsLeft, func(locationIdLeft int, index int) int {
		return numbers.AbsDiffInt(locationIdsRight[index], locationIdLeft)
	})

	return arrays.SumInts(distances)
}

func Part2(data string) int {
	locationIdsLeft, locationIdsRight := parse(data)

	similaritiesMap := make(map[int]int)
	for _, locationIdRight := range locationIdsRight {
		similaritiesMap[locationIdRight] = similaritiesMap[locationIdRight] + 1
	}

	similarities := arrays.Map(locationIdsLeft, func(locationIdLeft int, _ int) int {
		return locationIdLeft * similaritiesMap[locationIdLeft]
	})

	return arrays.SumInts(similarities)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
