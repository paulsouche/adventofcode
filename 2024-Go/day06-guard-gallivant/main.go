package main

import (
	"fmt"
	"image"
	"os"
	"strings"
)

const (
	NORTH = iota
	EAST
	SOUTH
	WEST
)

type Guard struct {
	Coordinates image.Point
	Direction   int
}

func (guard Guard) TurnRight() (turnedGuard Guard) {
	turnedGuard.Coordinates = guard.Coordinates
	switch guard.Direction {
	case NORTH:
		turnedGuard.Direction = EAST
	case EAST:
		turnedGuard.Direction = SOUTH
	case SOUTH:
		turnedGuard.Direction = WEST
	case WEST:
		turnedGuard.Direction = NORTH
	default:
		panic(fmt.Sprintf("Invalid direction %d", guard.Direction))
	}
	return
}

func (guard Guard) Move() (movedGuard Guard) {
	movedGuard.Direction = guard.Direction
	switch guard.Direction {
	case NORTH:
		movedGuard.Coordinates = guard.Coordinates.Add(image.Point{
			X: 0,
			Y: -1,
		})
	case EAST:
		movedGuard.Coordinates = guard.Coordinates.Add(image.Point{
			X: 1,
			Y: 0,
		})
	case SOUTH:
		movedGuard.Coordinates = guard.Coordinates.Add(image.Point{
			X: 0,
			Y: 1,
		})
	case WEST:
		movedGuard.Coordinates = guard.Coordinates.Add(image.Point{
			X: -1,
			Y: 0,
		})
	default:
		panic(fmt.Sprintf("Invalid direction %d", guard.Direction))
	}
	return
}

func parse(data string) (obstacles map[image.Point]bool, guard Guard, maxXY image.Point) {
	maxXY = image.Point{X: 0, Y: 0}
	obstacles = make(map[image.Point]bool)
	for y, line := range strings.Split(data, "\n") {
		for x, char := range line {
			position := image.Point{
				X: x,
				Y: y,
			}
			if char == '#' {
				obstacles[position] = true
			} else if char == '^' {
				guard.Coordinates = position
				guard.Direction = NORTH
			}
			maxXY.X = max(maxXY.X, x)
		}
		maxXY.Y = max(maxXY.Y, y)
	}
	return
}

func run(guard Guard, obstacles map[image.Point]bool, max image.Point) (visited map[image.Point]bool, isCyclic bool) {
	isCyclic = false
	visited = make(map[image.Point]bool)
	alreadyThere := make(map[image.Point]map[int]bool)
	visited[guard.Coordinates] = true
	for {
		newGuard := guard.Move()
		newCoordinates := newGuard.Coordinates
		newDirection := newGuard.Direction

		if obstacles[newCoordinates] {
			guard = guard.TurnRight()
			continue
		} else if newCoordinates.X < 0 || newCoordinates.X > max.X || newCoordinates.Y < 0 || newCoordinates.Y > max.Y {
			break
		}

		guard = newGuard
		visited[newCoordinates] = true
		if alreadyThere[newCoordinates] == nil {
			alreadyThere[newCoordinates] = make(map[int]bool)
		}

		if alreadyThere[newCoordinates][newDirection] {
			isCyclic = true
			break
		}
		alreadyThere[newCoordinates][newDirection] = true
	}
	return
}

func Part1(data string) int {
	obstacles, guard, max := parse(data)

	visited, _ := run(guard, obstacles, max)

	return len(visited)
}

func Part2(data string) int {
	obstacles, guard, max := parse(data)

	visited, _ := run(guard, obstacles, max)

	var options = make(map[image.Point]bool)

	for y := 0; y <= max.Y; y++ {
		for x := 0; x <= max.X; x++ {
			newObstacle := image.Point{
				X: x,
				Y: y,
			}

			if !visited[newObstacle] {
				continue
			}

			obstacles[newObstacle] = true

			_, isCyclic := run(guard, obstacles, max)

			if isCyclic {
				options[newObstacle] = true
			}

			obstacles[newObstacle] = false
		}
	}

	return len(options)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
