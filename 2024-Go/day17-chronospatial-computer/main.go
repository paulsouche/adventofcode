package main

import (
	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
	"fmt"
	"math"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func parse(input string) (registers map[string]int, program []int) {
	registers = make(map[string]int)
	bitRegex, _ := regexp.Compile(`\d+`)
	rawInput := strings.Split(input, "\n\n")
	rawRegisters := arrays.Map(strings.Split(rawInput[0], "\n"), func(rawRegister string, _ int) string {
		return bitRegex.FindAllString(rawRegister, -1)[0]
	})

	registers["A"] = numbers.SafeConvertStrToInt(rawRegisters[0])
	registers["B"] = numbers.SafeConvertStrToInt(rawRegisters[1])
	registers["C"] = numbers.SafeConvertStrToInt(rawRegisters[2])

	program = arrays.Map(bitRegex.FindAllString(rawInput[1], -1), func(bit string, _ int) int {
		return numbers.SafeConvertStrToInt(bit)
	})

	return
}

func runProgram(registers map[string]int, program []int) string {
	instructionPointer := 0
	output := make([]string, 0)

	for {
		instruction := program[instructionPointer]
		operand := program[instructionPointer+1]

		var combo int
		if operand <= 3 || operand == 7 {
			combo = operand
		} else if operand == 4 {
			combo = registers["A"]
		} else if operand == 5 {
			combo = registers["B"]
		} else if operand == 6 {
			combo = registers["C"]
		} else {
			panic(fmt.Sprintf("Invalid combo %d", combo))
		}

		switch instruction {
		case 0:
			registers["A"] /= int(math.Pow(2, float64(combo)))
		case 1:
			registers["B"] ^= operand
		case 2:
			registers["B"] = combo % 8
		case 3:
			if registers["A"] != 0 {
				instructionPointer = operand
				continue
			}
		case 4:
			registers["B"] ^= registers["C"]
		case 5:
			output = append(output, strconv.Itoa(combo%8))
		case 6:
			registers["B"] = registers["A"] / int(math.Pow(2, float64(combo)))
		case 7:
			registers["C"] = registers["A"] / int(math.Pow(2, float64(combo)))
		}

		instructionPointer += 2

		if instructionPointer >= len(program) {
			break
		}
	}

	return strings.Join(output, ",")
}

func Part1(data string) string {
	registers, program := parse(data)

	return runProgram(registers, program)
}

// while a != 0
//
//	b = a % 8
//	b = b ^ 5
//	c = a / 2 pow B
//	b = b ^ c
//	a = a / 2 pow 3
//	b = b ^ 6
//	out.add(b % 8)
func Part2(data string) int {
	registers, program := parse(data)

	expected := strings.Join(arrays.Map(program, func(i int, _ int) string {
		return strconv.Itoa(i)
	}), ",")

	i := 1
	for {
		registers["A"] = i
		registers["B"] = 0
		registers["C"] = 0

		output := runProgram(registers, program)

		if expected == output {
			break
		}

		outputProgram := arrays.Map(strings.Split(output, ","), func(i string, _ int) int {
			return numbers.SafeConvertStrToInt(i)
		})

		if len(program) > len(outputProgram) {
			i *= 2
			continue
		}

		if len(program) == len(outputProgram) {
			for j := len(outputProgram) - 1; j >= 0; j-- {
				if program[j] != outputProgram[j] {
					i += int(math.Pow(8, float64(j)))
					break
				}
			}
		}

		if len(program) < len(outputProgram) {
			i /= 2
			continue
		}
	}

	return i
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
