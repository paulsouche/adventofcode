package main

import (
	"fmt"
	"os"
	"regexp"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parseMemory(data string, memoryRegex *regexp.Regexp) []string {
	return memoryRegex.FindAllString(data, -1)
}

func computeInstruction(instruction string, _ int) int {
	instructionsRegex, _ := regexp.Compile(`\d+`)
	return arrays.MultiplyInts(arrays.Map(instructionsRegex.FindAllString(instruction, -1), func(num string, _ int) int {
		return numbers.SafeConvertStrToInt(num)
	}))
}

func Part1(data string) int {
	memoryRegex, _ := regexp.Compile(`mul\(\d+,\d+\)`)
	instructions := parseMemory(data, memoryRegex)

	return arrays.SumInts(arrays.Map(instructions, computeInstruction))
}

func Part2(data string) int {
	memoryRegex, _ := regexp.Compile(`mul\(\d+,\d+\)|do\(\)|don't\(\)`)
	instructions := parseMemory(data, memoryRegex)

	var do bool
	do = true
	return arrays.SumInts(arrays.Map(instructions, func(instruction string, index int) (result int) {
		if instruction == "don't()" {
			do = false
		} else if instruction == "do()" {
			do = true
		} else if do {
			result = computeInstruction(instruction, index) // So sad
		}
		return
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
