package main

import (
	"os"
	"testing"
)

func TestPart11(t *testing.T) {
	want := 140
	file, _ := os.ReadFile("input_test1.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart12(t *testing.T) {
	want := 772
	file, _ := os.ReadFile("input_test2.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart13(t *testing.T) {
	want := 1930
	file, _ := os.ReadFile("input_test3.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test.txt") = %d, want match for %d`, part1Result, want)
	}
}

func TestPart21(t *testing.T) {
	want := 80
	file, _ := os.ReadFile("input_test1.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test.txt") = %d, want match for %d`, part2Result, want)
	}
}

func TestPart22(t *testing.T) {
	want := 436
	file, _ := os.ReadFile("input_test2.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test.txt") = %d, want match for %d`, part2Result, want)
	}
}

func TestPart23(t *testing.T) {
	want := 1206
	file, _ := os.ReadFile("input_test3.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test.txt") = %d, want match for %d`, part2Result, want)
	}
}
