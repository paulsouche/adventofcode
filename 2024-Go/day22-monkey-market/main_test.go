package main

import (
	"os"
	"testing"
)

func TestPart1(t *testing.T) {
	want := 37327623
	file, _ := os.ReadFile("input_test1.txt")
	part1Result := Part1(string(file), 2000)
	if want != part1Result {
		t.Fatalf(`part1("input_test1.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart2(t *testing.T) {
	want := 23
	file, _ := os.ReadFile("input_test2.txt")
	part2Result := Part2(string(file), 2000)
	if want != part2Result {
		t.Fatalf(`part2("input_test2.txt") = %d, want match for %d`, part2Result, want)
	}
}
