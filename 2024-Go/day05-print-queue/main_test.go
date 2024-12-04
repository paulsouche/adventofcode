package main

import (
	"os"
	"testing"
)

func TestPart1(t *testing.T) {
	want := 143
	file, _ := os.ReadFile("input_test.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart2(t *testing.T) {
	want := 123
	file, _ := os.ReadFile("input_test.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test.txt") = %d, want match for %d`, part2Result, want)
	}
}
