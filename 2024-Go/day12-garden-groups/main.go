package main

import (
	"fmt"
	"image"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
)

type Region struct {
	typeofPlants string
	plots        []image.Point
}

func (region Region) area() int {
	return len(region.plots)
}

func (region Region) perimeter(garden [][]string) (perimeter int) {
	for _, plot := range region.plots {
		neighbour := coordinates.GetAdjacentCells(plot)(coordinates.CrossAdjacents, 0)
		neighbourInGarden := arrays.Filter(neighbour, coordinates.IsInSquareArray(garden))
		perimeter += len(neighbour) - len(neighbourInGarden)
		perimeter += len(arrays.Filter(neighbourInGarden, func(adjacent image.Point, _ int) bool {
			return garden[adjacent.Y][adjacent.X] != region.typeofPlants
		}))
	}
	return
}

func (region Region) sides() (sides int) {
	coordinatesSet := arrays.ToSet(region.plots)

	for ind, plot := range region.plots {
		adjacents := coordinates.GetAdjacentCells(plot)(coordinates.Adjacents, ind)
		topLeft := coordinatesSet[adjacents[0]]
		left := coordinatesSet[adjacents[1]]
		bottomLeft := coordinatesSet[adjacents[2]]
		top := coordinatesSet[adjacents[3]]
		bottom := coordinatesSet[adjacents[4]]
		topRight := coordinatesSet[adjacents[5]]
		right := coordinatesSet[adjacents[6]]
		bottomRigth := coordinatesSet[adjacents[7]]

		if !left && !top {
			sides++
		}
		if !right && !top {
			sides++
		}
		if !left && !bottom {
			sides++
		}
		if !right && !bottom {
			sides++
		}
		if !topRight && top && right {
			sides++
		}
		if !topLeft && top && left {
			sides++
		}
		if !bottomLeft && bottom && left {
			sides++
		}
		if !bottomRigth && bottom && right {
			sides++
		}
	}

	return
}

func parse(input string) [][]string {
	return arrays.Map(strings.Split(input, "\n"), func(line string, _ int) []string {
		return strings.Split(line, "")
	})
}

func getRegions(garden [][]string) (regions []Region) {
	alreadyThere := make(map[image.Point]bool)

	for y, line := range garden {
		for x, rawPlot := range line {
			if alreadyThere[image.Point{
				X: x,
				Y: y,
			}] {
				continue
			}

			plot := image.Point{X: x, Y: y}
			alreadyThere[plot] = true
			region := Region{
				typeofPlants: rawPlot,
				plots:        []image.Point{},
			}
			region.plots = append(region.plots, plot)

			queue := []image.Point{
				plot,
			}

			var pos image.Point
			for {
				pos, queue = queue[0], queue[1:]

				neighboursWithSameTypeOfPlant := arrays.Filter(arrays.Filter(coordinates.GetAdjacentCells(pos)(coordinates.CrossAdjacents, 0), coordinates.IsInSquareArray(garden)), func(currentPos image.Point, _ int) bool {
					if alreadyThere[currentPos] {
						return false
					}
					return garden[currentPos.Y][currentPos.X] == rawPlot
				})

				for _, neighbour := range neighboursWithSameTypeOfPlant {
					alreadyThere[neighbour] = true
					region.plots = append(region.plots, neighbour)
				}

				queue = append(queue, neighboursWithSameTypeOfPlant...)

				if len(queue) == 0 {
					break
				}
			}

			regions = append(regions, region)
		}
	}
	return
}

func Part1(data string) int {
	garden := parse(data)

	regions := getRegions(garden)

	return arrays.SumInts(arrays.Map(regions, func(region Region, _ int) int {
		return region.area() * region.perimeter(garden)
	}))
}

func Part2(data string) int {
	garden := parse(data)

	regions := getRegions(garden)

	return arrays.SumInts(arrays.Map(regions, func(region Region, _ int) int {
		return region.area() * region.sides()
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
