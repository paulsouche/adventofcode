package main

import (
	"fmt"
	"os"
)

func Part1(data string) int {
	return len(data)
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
