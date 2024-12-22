package main

import (
	"adventofcode/common/arrays"
	"fmt"
	"os"
	"sort"
	"strings"
)

func parse(input string) (computers map[string]map[string]bool) {
	computers = make(map[string]map[string]bool)
	for _, rawConnection := range strings.Split(input, "\n") {
		connectedComputers := strings.Split(rawConnection, "-")
		computer1 := connectedComputers[0]
		computer2 := connectedComputers[1]

		_, hasComputer1 := computers[computer1]
		_, hasComputer2 := computers[computer2]

		if !hasComputer1 {
			computers[computer1] = map[string]bool{}
		}

		if !hasComputer2 {
			computers[computer2] = map[string]bool{}
		}

		computers[computer1][computer2] = true
		computers[computer2][computer1] = true
	}
	return
}

func Part1(data string) int {
	computers := parse(data)

	setsOfThree := make(map[string]bool)
	for computer1Name, computer1Connections := range computers {
		for computer2Name := range computer1Connections {
			computer2Connections := computers[computer2Name]
			if !computer2Connections[computer1Name] {
				continue
			}

			for computer3Name := range computer2Connections {
				if computer3Name == computer1Name {
					continue
				}

				computer3Connections := computers[computer3Name]

				if computer3Connections[computer1Name] {
					setOfThree := []string{computer1Name, computer2Name, computer3Name}
					if arrays.Some(setOfThree, func(computerName string, _ int) bool {
						return strings.HasPrefix(computerName, "t")
					}) {
						sort.Strings(setOfThree)
						setsOfThree[strings.Join(setOfThree, ",")] = true
					}
				}
			}
		}
	}
	return len(setsOfThree)
}

func Part2(data string) string {
	computers := parse(data)

	setsSizes := make(map[string]int)
	maxConnection := 0
	for computer1Name := range computers {
		queue := []string{
			computer1Name,
		}
		visited := make(map[string]bool)
		visited[computer1Name] = true

		var computerName string
		for {
			computerName, queue = queue[0], queue[1:]
			computerConnections := computers[computerName]

			for connection := range computerConnections {
				if visited[connection] {
					continue
				}

				needsConnectionWith := arrays.ToArray(visited)
				nextConnections := computers[connection]

				if arrays.Every(needsConnectionWith, func(neededConnection string, _ int) bool {
					return nextConnections[neededConnection]
				}) {
					visited[connection] = true
					queue = append(queue, connection)
				}
			}

			if len(queue) == 0 {
				break
			}
		}

		setOfComputers := arrays.ToArray(visited)
		sort.Strings(setOfComputers)

		setsSizes[strings.Join(setOfComputers, ",")] = len(setOfComputers)
		maxConnection = max(maxConnection, len(setOfComputers))
	}

	var password string
	for pass, size := range setsSizes {
		if size == maxConnection {
			password = pass
		}
	}

	return password
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
