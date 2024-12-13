package main

import (
	"os"
	"testing"
)

func TestPart1(t *testing.T) {
	want := 12
	file, _ := os.ReadFile("input_test.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}
