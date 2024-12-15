package main

import (
	"os"
	"testing"
)

func TestPart11(t *testing.T) {
	want := 7036
	file, _ := os.ReadFile("input_test1.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test1.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart12(t *testing.T) {
	want := 11048
	file, _ := os.ReadFile("input_test2.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test2.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart21(t *testing.T) {
	want := 45
	file, _ := os.ReadFile("input_test1.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test1.txt") = %d, want match for %d`, part2Result, want)
	}
}

func TestPart22(t *testing.T) {
	want := 64
	file, _ := os.ReadFile("input_test2.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test2.txt") = %d, want match for %d`, part2Result, want)
	}
}
