package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(input string) []int {
	return arrays.Map(strings.Split(input, " "), func(rawStone string, _ int) int {
		return numbers.SafeConvertStrToInt(rawStone)
	})
}

func solve(stone int, blinks int, memory map[string]int) int {
	if blinks == 0 {
		return 1
	}

	memoryKey := fmt.Sprintf("stone:%d,times:%d", stone, blinks)
	memoryValue, hasMemoryValue := memory[memoryKey]
	if hasMemoryValue {
		return memoryValue
	}

	stoneString := strconv.Itoa(stone)
	var value int
	if stone == 0 {
		value = solve(1, blinks-1, memory)
	} else if len(stoneString)%2 == 0 {
		value = solve(numbers.SafeConvertStrToInt(stoneString[:len(stoneString)/2]), blinks-1, memory) +
			solve(numbers.SafeConvertStrToInt(stoneString[len(stoneString)/2:]), blinks-1, memory)
	} else {
		value = solve(stone*2024, blinks-1, memory)
	}

	memory[memoryKey] = value

	return value
}

func Part1(data string) int {
	stones := parse(data)
	memory := make(map[string]int)

	return arrays.SumInts(arrays.Map(stones, func(stone int, _ int) int {
		return solve(stone, 25, memory)
	}))
}

func Part2(data string) int {
	stones := parse(data)
	memory := make(map[string]int)

	return arrays.SumInts(arrays.Map(stones, func(stone int, _ int) int {
		return solve(stone, 75, memory)
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
