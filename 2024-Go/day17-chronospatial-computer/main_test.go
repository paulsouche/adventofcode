package main

import (
	"os"
	"testing"
)

func TestPart11(t *testing.T) {
	want := "0,1,2"
	file, _ := os.ReadFile("input_test1.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test1.txt") = %s, want match for %s`, part1Result, want)
	}
}

func TestPart12(t *testing.T) {
	want := "4,2,5,6,7,7,7,7,3,1,0"
	file, _ := os.ReadFile("input_test2.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test2.txt") = %s, want match for %s`, part1Result, want)
	}
}

func TestPart13(t *testing.T) {
	want := "4,6,3,5,6,3,5,2,1,0"
	file, _ := os.ReadFile("input_test3.txt")
	part1Result := Part1(string(file))
	if want != part1Result {
		t.Fatalf(`part1("input_test3.txt") = %s, want match for %s`, part1Result, want)
	}
}

func TestPart2(t *testing.T) {
	want := 117440
	file, _ := os.ReadFile("input_test4.txt")
	part2Result := Part2(string(file))
	if want != part2Result {
		t.Fatalf(`part2("input_test4.txt") = %d, want match for %d`, part2Result, want)
	}
}
