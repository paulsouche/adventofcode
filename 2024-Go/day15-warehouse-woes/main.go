package main

import (
	"fmt"
	"image"
	"os"
	"strings"

	"adventofcode/common/arrays"
)

func funcForMove(move string) func(image.Point) image.Point {
	switch move {
	case "^":
		return func(point image.Point) (newPoint image.Point) {
			newPoint.X = point.X
			newPoint.Y = point.Y - 1
			return
		}
	case ">":
		return func(point image.Point) (newPoint image.Point) {
			newPoint.X = point.X + 1
			newPoint.Y = point.Y
			return
		}
	case "v":
		return func(point image.Point) (newPoint image.Point) {
			newPoint.X = point.X
			newPoint.Y = point.Y + 1
			return
		}
	case "<":
		return func(point image.Point) (newPoint image.Point) {
			newPoint.X = point.X - 1
			newPoint.Y = point.Y
			return
		}
	}
	panic(fmt.Sprintf("Invalid move %s", move))
}

type Crate struct {
	Position image.Point
}

func (crate Crate) GPSCoordinates() int {
	return 100*crate.Position.Y + crate.Position.X
}

type Robot struct {
	Position image.Point
}

func (robot Robot) move(move string, walls map[image.Point]bool, crates []Crate) (Robot, []Crate) {
	moveFunc := funcForMove(move)
	cratesSet := arrays.ToSet(crates)
	movedCrates := make(map[Crate]bool)

	nextRobot := Robot{
		Position: moveFunc(robot.Position),
	}

	isWall := walls[nextRobot.Position]
	if isWall {
		return robot, crates
	}

	crate := Crate(nextRobot)
	isCrate := cratesSet[crate]

	if isCrate {
		movedCrates[crate] = true
		loopRobot := nextRobot
		for {
			loopRobot = Robot{
				Position: moveFunc(loopRobot.Position),
			}

			isWall := walls[loopRobot.Position]
			if isWall {
				return robot, crates
			}

			crate := Crate(loopRobot)
			isCrate := cratesSet[crate]

			if isCrate {
				movedCrates[crate] = true
				continue
			}

			break
		}

		return nextRobot, arrays.Map(crates, func(crate Crate, _ int) Crate {
			if movedCrates[crate] {
				return Crate{
					Position: moveFunc(crate.Position),
				}
			}
			return crate
		})
	}

	return nextRobot, crates
}

func parse(input string) (walls map[image.Point]bool, crates []Crate, robot Robot, moves []string, maxXY image.Point) {
	walls = make(map[image.Point]bool)
	rawInput := strings.Split(input, "\n\n")
	maxXY.X = 0
	maxXY.Y = 0

	for y, line := range strings.Split(rawInput[0], "\n") {
		maxXY.Y = max(maxXY.Y, y+1)
		for x, cell := range strings.Split(line, "") {
			maxXY.X = max(maxXY.X, x+1)
			position := image.Point{
				X: x,
				Y: y,
			}

			switch cell {
			case "#":
				walls[position] = true
			case "O":
				crates = append(crates, Crate{
					Position: position,
				})
			case "@":
				robot.Position = position
			default:
			}
		}
	}

	moves = strings.Split(strings.Join(strings.Split(rawInput[1], "\n"), ""), "")

	return
}

func print(walls map[image.Point]bool, crates []Crate, robot Robot, max image.Point) {
	cratesSet := arrays.ToSet(crates)

	for y := 0; y < max.Y; y++ {
		var line string

		for x := 0; x < max.X; x++ {
			position := image.Point{
				X: x,
				Y: y,
			}

			if walls[position] {
				line += "#"
				continue
			}

			crate := Crate{
				Position: position,
			}

			if cratesSet[crate] {
				line += "O"
				continue
			}

			robotPos := Robot(crate)

			if robotPos == robot {
				line += "@"
				continue
			}

			line += "."
		}
		fmt.Println(line)
	}
}

func walk(walls map[image.Point]bool, crates []Crate, robot Robot, moves []string, max image.Point) (movedCrates []Crate) {
	movedRobot := robot
	movedCrates = crates
	// print(walls,crates,robot, max)
	for _, move := range moves {
		movedRobot, movedCrates = movedRobot.move(move, walls, movedCrates)
		// fmt.Println(move)
		// print(walls,movedCrates,movedRobot, max)
	}
	return
}

func Part1(data string) int {
	walls, crates, robot, moves, max := parse(data)

	crates = walk(walls, crates, robot, moves, max)

	return arrays.SumInts(arrays.Map(crates, func(crate Crate, _ int) int {
		return crate.GPSCoordinates()
	}))
}

func run(input, moves string) int {
	grid, robot := map[image.Point]rune{}, image.Point{}
	for y, s := range strings.Fields(input) {
		for x, r := range s {
			if r == '@' {
				robot = image.Point{x, y}
				r = '.'
			}
			grid[image.Point{x, y}] = r
		}
	}

	delta := map[rune]image.Point{
		'^': {0, -1}, '>': {1, 0}, 'v': {0, 1}, '<': {-1, 0},
		'[': {1, 0}, ']': {-1, 0},
	}

loop:
	for _, r := range strings.ReplaceAll(moves, "\n", "") {
		queue, boxes := []image.Point{robot}, map[image.Point]rune{}
		for len(queue) > 0 {
			p := queue[0]
			queue = queue[1:]

			if _, ok := boxes[p]; ok {
				continue
			}
			boxes[p] = grid[p]

			switch n := p.Add(delta[r]); grid[n] {
			case '#':
				continue loop
			case '[', ']':
				queue = append(queue, n.Add(delta[grid[n]]))
				fallthrough
			case 'O':
				queue = append(queue, n)
			}
		}

		for b := range boxes {
			grid[b] = '.'
		}
		for b := range boxes {
			grid[b.Add(delta[r])] = boxes[b]
		}
		robot = robot.Add(delta[r])
	}

	gps := 0
	for p, r := range grid {
		if r == 'O' || r == '[' {
			gps += 100*p.Y + p.X
		}
	}
	return gps
}

func Part2(data string) int {
	split := strings.Split(strings.TrimSpace(string(data)), "\n\n")
	r := strings.NewReplacer("#", "##", "O", "[]", ".", "..", "@", "@.")

	return run(r.Replace(split[0]), split[1])
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
