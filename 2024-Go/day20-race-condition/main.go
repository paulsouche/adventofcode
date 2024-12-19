package main

import (
	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
	"fmt"
	"image"
	"math"
	"os"
	"sort"
	"strings"
)

type Program struct {
	PicoSeconds int
	Position    image.Point
	Cheats      int
}

func (program Program) Move(cell image.Point) (next Program) {
	next.Position = cell
	next.PicoSeconds = program.PicoSeconds + 1
	next.Cheats = program.Cheats
	return
}

func (program Program) NextPrograms(isInMaze func(image.Point) bool, maze [][]string, alreadyThere *map[image.Point]int, alreadyCheatedAt *map[image.Point]bool) (nextPrograms []Program, hasCheated bool) {
	hasCheated = program.Cheats > 0
	cheatsOn := alreadyCheatedAt != nil
	nextPrograms = arrays.Reduce(coordinates.GetAdjacentCells(program.Position)(coordinates.CrossAdjacents, 0), func(programs []Program, neighbour image.Point, ind int) []Program {
		if !isInMaze(neighbour) {
			return programs
		}

		isWall := maze[neighbour.Y][neighbour.X] == "#"

		var nextProgram Program
		if !cheatsOn || (*alreadyCheatedAt)[neighbour] {
			if isWall {
				return programs
			}
		}

		nextProgram = program.Move(neighbour)

		if cheatsOn && !hasCheated && program.Cheats == 0 && isWall {
			(*alreadyCheatedAt)[neighbour] = true
			hasCheated = true
			nextProgram.Cheats = 1
		} else if nextProgram.Cheats == 1 {
			nextProgram.Cheats = 2
		} else if isWall {
			return programs
		}

		picoseconds, wasHere := (*alreadyThere)[nextProgram.Position]

		if !wasHere || nextProgram.PicoSeconds < picoseconds {
			(*alreadyThere)[nextProgram.Position] = nextProgram.PicoSeconds
			return append(programs, nextProgram)
		}

		return programs
	}, make([]Program, 0))
	return
}

func parse(input string) (maze [][]string, start Program, maxXY image.Point) {
	start.PicoSeconds = 0
	start.Cheats = 0
	maxXY = image.Pt(0, 0)
	maze = arrays.Map(strings.Split(input, "\n"), func(row string, y int) []string {
		maxXY.Y = max(maxXY.Y, y)
		return arrays.Map(strings.Split(row, ""), func(cell string, x int) string {
			maxXY.X = max(maxXY.X, x)
			if cell == "S" {
				start.Position = image.Pt(x, y)
				return "."
			}
			return cell
		})
	})
	return
}

func walk(maze [][]string, start Program, maxXY image.Point, cheats *map[image.Point]bool) (minPicoSeconds int, usedCheat bool) {
	mazeArea := image.Rectangle{
		Min: image.Point{
			X: 0,
			Y: 0,
		},
		Max: image.Point{
			X: maxXY.X + 1,
			Y: maxXY.Y + 1,
		},
	}

	queue := []Program{
		start,
	}
	alreadyThere := make(map[image.Point]int)
	alreadyThere[start.Position] = start.PicoSeconds

	minPicoSeconds = math.MaxInt

	isInMaze := func(cell image.Point) bool {
		return cell.In(mazeArea)
	}

	var program Program
	for {
		program, queue = queue[0], queue[1:]

		if maze[program.Position.Y][program.Position.X] == "E" {
			minPicoSeconds = min(minPicoSeconds, program.PicoSeconds)
		} else {
			nextPrograms, hasCheated := program.NextPrograms(isInMaze, maze, &alreadyThere, cheats)

			if hasCheated {
				usedCheat = true
			}

			queue = append(queue, nextPrograms...)

			// fmt.Println("")
			// for y := 0 ; y <= maxXY.Y ; y++ {
			// 	var line string
			// 	for x := 0 ; x <= maxXY.X ; x++ {
			// 		if picoSeconds, wasHere := alreadyThere[image.Pt(x, y)] ; wasHere {
			// 			line += strconv.Itoa(picoSeconds)
			// 		} else {
			// 			line += maze[y][x]
			// 		}
			// 	}
			// 	fmt.Println(line)
			// }
			// fmt.Println("")

			sort.Slice(queue, func(i, j int) bool {
				return queue[i].PicoSeconds < queue[j].PicoSeconds
			})
		}

		if len(queue) == 0 {
			break
		}
	}

	return
}

func Part1(data string) int {
	maze, start, maxXY := parse(data)

	minPicoSeconds, _ := walk(maze, start, maxXY, nil)

	fmt.Println(minPicoSeconds)

	spares := make(map[int]int)
	cheats := make(map[image.Point]bool)
	for {
		picoSeconds, hasCheated := walk(maze, start, maxXY, &cheats)

		if picoSeconds < minPicoSeconds {
			spares[picoSeconds]++
		}

		if !hasCheated {
			break
		}
	}

	fmt.Println(spares)

	return len(data)
}

func Part2(data string) int {
	return len(data)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
