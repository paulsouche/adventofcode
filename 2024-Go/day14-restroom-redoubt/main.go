package main

import (
	"fmt"
	"image"
	"math"
	"os"
	"regexp"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

type Robot struct {
	Position image.Point
	Velocity image.Point
}

func (robot Robot) move(times int, max image.Point) (newRobot Robot) {
	newRobot.Velocity.X = robot.Velocity.X
	newRobot.Velocity.Y = robot.Velocity.Y

	newRobot.Position.X = (robot.Position.X + robot.Velocity.X*times) % max.X
	if newRobot.Position.X < 0 {
		newRobot.Position.X += max.X
	}

	newRobot.Position.Y = (robot.Position.Y + robot.Velocity.Y*times) % max.Y
	if newRobot.Position.Y < 0 {
		newRobot.Position.Y += max.Y
	}

	return
}

func (robot Robot) timetoReset(max image.Point) (times int) {
	startPos := robot.Position
	times = 1
	for {
		robot = robot.move(1, max)

		if robot.Position == startPos {
			break
		}

		times++
	}
	return
}

func parse(input string) ([]Robot, image.Point) {
	maxX := 0
	maxY := 0
	robotRegex, _ := regexp.Compile(`^p=(\d+),(\d+)\sv=(-?\d+),(-?\d+)$`)
	return arrays.Map(strings.Split(input, "\n"), func(rawLine string, _ int) (robot Robot) {
			matches := robotRegex.FindAllStringSubmatch(rawLine, -1)

			robot.Position.X = numbers.SafeConvertStrToInt(matches[0][1])
			robot.Position.Y = numbers.SafeConvertStrToInt(matches[0][2])
			robot.Velocity.X = numbers.SafeConvertStrToInt(matches[0][3])
			robot.Velocity.Y = numbers.SafeConvertStrToInt(matches[0][4])

			maxX = max(maxX, robot.Position.X+1)
			maxY = max(maxY, robot.Position.Y+1)

			return
		}), image.Point{
			X: maxX,
			Y: maxY,
		}
}

func safetyFactor(robots []Robot, max image.Point) int {
	midX := max.X / 2
	midY := max.Y / 2

	topLeft := 0
	topRight := 0
	bottomLeft := 0
	bottomRight := 0

	for _, robot := range robots {
		if robot.Position.Y < midY && robot.Position.X < midX {
			topLeft++
		}

		if robot.Position.Y < midY && robot.Position.X > midX {
			topRight++
		}

		if robot.Position.Y > midY && robot.Position.X < midX {
			bottomLeft++
		}

		if robot.Position.Y > midY && robot.Position.X > midX {
			bottomRight++
		}
	}

	return topLeft * topRight * bottomLeft * bottomRight
}

func Part1(data string) int {
	robots, max := parse(data)

	robots = arrays.Map(robots, func(robot Robot, _ int) Robot {
		return robot.move(100, max)
	})

	return safetyFactor(robots, max)
}

func Part2(data string) int {
	robots, max := parse(data)

	timesToReset := arrays.Map(robots, func(robot Robot, _ int) int {
		return robot.timetoReset(max)
	})

	timeToReset := numbers.LCM(timesToReset...)

	minSafety := math.MaxInt
	minTimes := 0

	times := 1
	for times < timeToReset {
		robots = arrays.Map(robots, func(robot Robot, _ int) Robot {
			return robot.move(1, max)
		})

		safety := safetyFactor(robots, max)
		if safety < minSafety {
			minSafety = safety
			minTimes = times
		} else if safety == minSafety {
			break
		}

		times++
	}
	return minTimes
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
