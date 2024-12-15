package main

import (
	"fmt"
	"image"
	"math"
	"os"
	"slices"
	"sort"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
)

const (
	NORTH = iota
	EAST
	SOUTH
	WEST
)

type Reindeer struct {
	Position  image.Point
	Weight    int
	Direction int
	Ancestors []image.Point
}

func (reindeer Reindeer) ToHash() string {
	return fmt.Sprintf("x=%d,y=%d,d=%d", reindeer.Position.X, reindeer.Position.Y, reindeer.Direction)
}

func (reindeer Reindeer) Move(cell image.Point) (nextReindeer Reindeer) {
	nextReindeer.Position = cell
	nextReindeer.Ancestors = append(slices.Clone(reindeer.Ancestors), reindeer.Position)

	movedUp := (reindeer.Position.Y - cell.Y) > 0
	movedRight := (reindeer.Position.X - cell.X) < 0
	movedDown := (reindeer.Position.Y - cell.Y) < 0
	movedLeft := (reindeer.Position.X - cell.X) > 0

	switch reindeer.Direction {
	case NORTH:
		if movedUp {
			nextReindeer.Weight = reindeer.Weight + 1
			nextReindeer.Direction = NORTH
		} else if movedRight {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = EAST
		} else if movedDown {
			nextReindeer.Weight = reindeer.Weight + 2001
			nextReindeer.Direction = SOUTH
		} else if movedLeft {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = WEST
		} else {
			panic("invalid move NORTH")
		}
	case EAST:
		if movedUp {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = NORTH
		} else if movedRight {
			nextReindeer.Weight = reindeer.Weight + 1
			nextReindeer.Direction = EAST
		} else if movedDown {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = SOUTH
		} else if movedLeft {
			nextReindeer.Weight = reindeer.Weight + 2001
			nextReindeer.Direction = WEST
		} else {
			panic("invalid move EAST")
		}
	case SOUTH:
		if movedUp {
			nextReindeer.Weight = reindeer.Weight + 2001
			nextReindeer.Direction = NORTH
		} else if movedRight {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = EAST
		} else if movedDown {
			nextReindeer.Weight = reindeer.Weight + 1
			nextReindeer.Direction = SOUTH
		} else if movedLeft {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = WEST
		} else {
			panic("invalid move SOUTH")
		}
	case WEST:
		if movedUp {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = NORTH
		} else if movedRight {
			nextReindeer.Weight = reindeer.Weight + 2001
			nextReindeer.Direction = EAST
		} else if movedDown {
			nextReindeer.Weight = reindeer.Weight + 1001
			nextReindeer.Direction = SOUTH
		} else if movedLeft {
			nextReindeer.Weight = reindeer.Weight + 1
			nextReindeer.Direction = WEST
		} else {
			panic("invalid move WEST")
		}
	default:
		panic("invalid direction")
	}
	return
}

func parse(input string) (maze [][]string, reindeer Reindeer) {
	reindeer.Weight = 0
	reindeer.Direction = EAST
	reindeer.Ancestors = make([]image.Point, 0)

	maze = make([][]string, 0)
	for y, line := range strings.Split(input, "\n") {
		maze = append(maze, make([]string, 0))
		for x, cell := range strings.Split(line, "") {
			maze[y] = append(maze[y], cell)

			if cell == "S" {
				reindeer.Position = image.Point{
					X: x,
					Y: y,
				}
			}
		}
	}
	return
}

func walk(maze [][]string, start Reindeer) (minWeight int, cellsByCost map[int]map[image.Point]bool) {
	queue := []Reindeer{
		start,
	}
	cellsByCost = make(map[int]map[image.Point]bool)
	alreadyThere := make(map[string]int)

	minWeight = math.MaxInt

	isWalkable := func(cell image.Point, _ int) bool {
		return maze[cell.Y][cell.X] != "#"
	}

	var reindeer Reindeer
	for {
		reindeer, queue = queue[0], queue[1:]

		alreadyThere[reindeer.ToHash()] = reindeer.Weight

		if maze[reindeer.Position.Y][reindeer.Position.X] == "E" {
			minWeight = min(minWeight, reindeer.Weight)

			if cellsByCost[reindeer.Weight] == nil {
				cellsByCost[reindeer.Weight] = make(map[image.Point]bool)
				cellsByCost[reindeer.Weight][reindeer.Position] = true
			}

			for _, ancestor := range reindeer.Ancestors {
				cellsByCost[reindeer.Weight][ancestor] = true
			}
		} else {
			neighbours := arrays.Filter(coordinates.GetAdjacentCells(reindeer.Position)(coordinates.CrossAdjacents, 0), isWalkable)
			nextReindeers := arrays.Map(neighbours, func(neighbour image.Point, _ int) Reindeer {
				return reindeer.Move(neighbour)
			})

			queue = append(queue, arrays.Filter(nextReindeers, func(nextReindeer Reindeer, _ int) bool {
				weight, wasHere := alreadyThere[nextReindeer.ToHash()]

				if !wasHere || nextReindeer.Weight <= weight {
					alreadyThere[nextReindeer.ToHash()] = nextReindeer.Weight
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

func Part1(data string) int {
	maze, reindeer := parse(data)

	minCost, _ := walk(maze, reindeer)

	return minCost
}

func Part2(data string) int {
	maze, reindeer := parse(data)

	minCost, cellsByCost := walk(maze, reindeer)

	return len(cellsByCost[minCost])
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
