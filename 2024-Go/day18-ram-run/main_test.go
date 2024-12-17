package main

import (
	"os"
	"testing"
)

func TestPart1(t *testing.T) {
	want := 22
	file, _ := os.ReadFile("input_test.txt")
	part1Result := Part1(string(file), 12)
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart2(t *testing.T) {
	want := "6,1"
	file, _ := os.ReadFile("input_test.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test.txt") = %s, want match for %s`, part2Result, want)
	}
}
