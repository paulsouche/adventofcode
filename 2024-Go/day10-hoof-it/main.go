package main

import (
	"fmt"
	"image"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
	"adventofcode/common/numbers"
)

func parse(input string) (topographicMap [][]int, starts []image.Point) {
	starts = make([]image.Point, 0)
	topographicMap = arrays.Map(strings.Split(input, "\n"), func(rawLine string, y int) (line []int) {
		line = make([]int, 0)
		for x, rawCell := range rawLine {
			cell := numbers.SafeConvertStrToInt(string(rawCell))
			line = append(line, cell)
			if cell == 0 {
				starts = append(starts, image.Point{X: x, Y: y})
			}
		}
		return
	})
	return
}

func trailHeadsScore(start image.Point, topographicMap [][]int, rating bool) (score int) {
	alreadyThere := make(map[image.Point]bool)
	queue := make([]image.Point, 0)
	queue = append(queue, start)

	var pos image.Point
	isInTopographicMap := coordinates.IsInSquareArray(topographicMap)
	for {
		pos, queue = queue[0], queue[1:]

		if rating {
			alreadyThere[pos] = true
		}

		newPositions := arrays.Filter(coordinates.GetAdjacentCells(pos)(coordinates.CrossAdjacents, 0), func(possiblePos image.Point, i int) bool {
			if !isInTopographicMap(possiblePos, i) {
				return false
			}
			if alreadyThere[possiblePos] {
				return false
			}
			return topographicMap[possiblePos.Y][possiblePos.X] == topographicMap[pos.Y][pos.X]+1
		})

		for _, newPosition := range newPositions {
			if !rating {
				alreadyThere[newPosition] = true
			}
			if topographicMap[newPosition.Y][newPosition.X] == 9 {
				score++
			} else {
				queue = append(queue, newPosition)
			}
		}

		if len(queue) == 0 {
			break
		}
	}
	return
}

func Part1(data string) int {
	topographicMap, starts := parse(data)

	return arrays.SumInts(arrays.Map(starts, func(start image.Point, _ int) int {
		return trailHeadsScore(start, topographicMap, false)
	}))
}

func Part2(data string) int {
	topographicMap, starts := parse(data)

	return arrays.SumInts(arrays.Map(starts, func(start image.Point, _ int) int {
		return trailHeadsScore(start, topographicMap, true)
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
