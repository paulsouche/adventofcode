package main

import (
	"fmt"
	"image"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
)

func parse(data string) (wordSearch [][]byte) {
	lines := strings.Split(data, "\n")
	return arrays.Map(lines, func(line string, _ int) (chars []byte) {
		for i := 0; i < len(line); i++ {
			chars = append(chars, line[i])
		}
		return
	})
}

func Part1(data string) int {
	toFind := []byte("XMAS")
	wordSearch := parse(data)

	var words = 0
	for y, line := range wordSearch {
		for x := range line {
			for ind, adjacent := range coordinates.Adjacents {
				var found = true
				for char := 0; char < len(toFind); char++ {
					point := image.Point{
						X: x + char*adjacent.X,
						Y: y + char*adjacent.Y,
					}

					if !coordinates.IsInSquareArray(wordSearch)(point, ind) { // Sad
						found = false
						break
					}

					if wordSearch[point.Y][point.X] != toFind[char] {
						found = false
						break
					}
				}

				if found {
					words++
				}
			}
		}
	}

	return words
}

func Part2(data string) int {
	wordSearch := parse(data)
	var center = byte('A')
	var m = byte('M')
	var s = byte('S')

	var words = 0
	for y, line := range wordSearch {
		for x := range line {
			if wordSearch[y][x] != center {
				continue
			}

			var validBranch = 0
			for _, adjacents := range arrays.Map(coordinates.CrossBranches, coordinates.GetAdjacentCells(image.Point{X: x, Y: y})) {
				var mCount = 0
				var sCount = 0
				for _, adjacent := range arrays.Filter(adjacents, coordinates.IsInSquareArray(wordSearch)) {
					if wordSearch[adjacent.Y][adjacent.X] == m {
						mCount++
					} else if wordSearch[adjacent.Y][adjacent.X] == s {
						sCount++
					}
				}

				if mCount == 1 && sCount == 1 {
					validBranch++
				}
			}

			if validBranch == 2 {
				words++
			}
		}
	}

	return words
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
