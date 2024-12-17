package main

import (
	"fmt"
	"image"
	"math"
	"os"
	"sort"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
	"adventofcode/common/numbers"
)

type Historians struct {
	Position image.Point
	Weight   int
}

func (historians Historians) Move(cell image.Point) (next Historians) {
	next.Position = cell
	next.Weight = historians.Weight + 1
	return
}

func (historians Historians) PossibleNeighbours(isInMaze func(image.Point) bool, bits map[image.Point]bool) []image.Point {
	return arrays.Filter(coordinates.GetAdjacentCells(historians.Position)(coordinates.CrossAdjacents, 0), func(neighbour image.Point, ind int) bool {
		return isInMaze(neighbour) && !bits[neighbour]
	})
}

func parse(input string) (bits []image.Point, historians Historians, maxXY image.Point) {
	bits = arrays.Map(strings.Split(input, "\n"), func(line string, _ int) image.Point {
		coordinates := arrays.Map(strings.Split(line, ","), func(coordinate string, _ int) int {
			return numbers.SafeConvertStrToInt(coordinate)
		})

		maxXY.X = max(maxXY.X, coordinates[0])
		maxXY.Y = max(maxXY.Y, coordinates[1])

		return image.Point{
			X: coordinates[0],
			Y: coordinates[1],
		}
	})

	historians = Historians{
		Position: image.Point{
			X: 0,
			Y: 0,
		},
		Weight: 0,
	}

	return
}

func walk(bits map[image.Point]bool, start Historians, maxXY image.Point) (minWeight int, reached bool) {
	maze := image.Rectangle{
		Min: image.Point{
			X: 0,
			Y: 0,
		},
		Max: image.Point{
			X: maxXY.X + 1,
			Y: maxXY.Y + 1,
		},
	}

	queue := []Historians{
		start,
	}
	alreadyThere := make(map[image.Point]int)
	alreadyThere[start.Position] = start.Weight

	minWeight = math.MaxInt

	isInMaze := func(cell image.Point) bool {
		return cell.In(maze)
	}

	var historians Historians
	for {
		historians, queue = queue[0], queue[1:]

		if historians.Position.Eq(maxXY) {
			minWeight = min(minWeight, historians.Weight)
			reached = true
		} else {
			neighbours := historians.PossibleNeighbours(isInMaze, bits)
			nextHistorians := arrays.Map(neighbours, func(neighbour image.Point, _ int) Historians {
				return historians.Move(neighbour)
			})

			queue = append(queue, arrays.Filter(nextHistorians, func(next Historians, _ int) bool {
				weight, wasHere := alreadyThere[next.Position]

				if !wasHere || next.Weight < weight {
					alreadyThere[next.Position] = next.Weight
					return true
				}
				return false
			})...)

			sort.Slice(queue, func(i, j int) bool {
				return queue[i].Weight < queue[j].Weight
			})
		}

		if len(queue) == 0 {
			break
		}
	}

	return
}

func Part1(data string, bitsCount int) int {
	bits, historians, max := parse(data)

	minWeght, _ := walk(arrays.ToSet(bits[0:bitsCount]), historians, max)

	return minWeght
}

func Part2(data string) string {
	bits, historians, max := parse(data)

	bitMin := 0
	bitMax := len(bits)
	for {
		bit := bitMin + (bitMax-bitMin)/2
		_, reached := walk(arrays.ToSet(bits[0:bit]), historians, max)

		if !reached {
			bitMax = bit
		} else {
			bitMin = bit
		}

		if bitMax == bitMin+1 {
			break
		}
	}

	return fmt.Sprintf("%d,%d", bits[bitMin].X, bits[bitMin].Y)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data, 1024))
	fmt.Println(Part2(data))
}
