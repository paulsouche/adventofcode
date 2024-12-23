package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
)

type Gate struct {
	Wire1     string
	Wire2     string
	Operation string
	WireOut   string
}

func (gate Gate) Operate(wire1Val bool, wire2Val bool) bool {
	switch gate.Operation {
	case "OR":
		return wire1Val || wire2Val
	case "AND":
		return wire1Val && wire2Val
	case "XOR":
		return wire1Val != wire2Val
	default:
		panic(fmt.Sprintf("Invalid operation %s", gate.Operation))
	}
}

func parse(input string) (wires map[string]bool, gates []Gate, wiresCount int) {
	wireRegex, _ := regexp.Compile(`^([a-z0-9]{3}):\s([0-1])$`)
	gateRegex, _ := regexp.Compile(`^([a-z0-9]{3})\s(AND|OR|XOR)\s([a-z0-9]{3})\s->\s([a-z0-9]{3})$`)

	rawInput := strings.Split(input, "\n\n")

	wires = make(map[string]bool)
	for _, rawWire := range strings.Split(rawInput[0], "\n") {
		matches := wireRegex.FindAllStringSubmatch(rawWire, -1)
		wires[matches[0][1]] = matches[0][2] == "1"
	}

	wiresMap := make(map[string]bool)
	for _, rawGate := range strings.Split(rawInput[1], "\n") {
		matches := gateRegex.FindAllStringSubmatch(rawGate, -1)
		gates = append(gates, Gate{
			Wire1:     matches[0][1],
			Wire2:     matches[0][3],
			Operation: matches[0][2],
			WireOut:   matches[0][4],
		})

		wiresMap[matches[0][1]] = true
		wiresMap[matches[0][3]] = true
		wiresMap[matches[0][4]] = true
	}

	wiresCount = len(wiresMap)

	return
}

func Part1(data string) int {
	wires, gates, wiresCount := parse(data)

	for {
		for _, gate := range gates {
			wire1Val, hasWire1 := wires[gate.Wire1]
			wire2Val, hasWire2 := wires[gate.Wire2]

			if hasWire1 && hasWire2 {
				wires[gate.WireOut] = gate.Operate(wire1Val, wire2Val)
			}
		}

		if len(wires) == wiresCount {
			break
		}
	}

	var binaryNumber string
	i := 0
	for {
		wireVal, hasWire := wires[fmt.Sprintf("z%02d", i)]

		if !hasWire {
			break
		}

		if wireVal {
			binaryNumber = "1" + binaryNumber
		} else {
			binaryNumber = "0" + binaryNumber
		}
		i++
	}

	result, _ := strconv.ParseInt(binaryNumber, 2, 64)

	return int(result)
}

func Part2(data string) int {
	return len(data)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
