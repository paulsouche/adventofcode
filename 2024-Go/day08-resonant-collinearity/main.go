package main

import (
	"fmt"
	"image"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/coordinates"
)

func CentralSymetric(point image.Point, center image.Point) image.Point {
	return image.Point{
		X: 2*center.X - point.X,
		Y: 2*center.Y - point.Y,
	}
}

type AntennaTuple struct {
	antennaA image.Point
	antennaB image.Point
}

func (tuple AntennaTuple) buildAntiNodesGenerator(maze [][]rune) func(bool) []image.Point {
	isInMaze := coordinates.IsInSquareArray(maze)

	symetryTuples := []AntennaTuple{
		{
			antennaA: tuple.antennaA,
			antennaB: tuple.antennaB,
		},
		{
			antennaA: tuple.antennaB,
			antennaB: tuple.antennaA,
		},
	}

	return func(resonantHarmonics bool) (antinodes []image.Point) {
		var generateAntiNodes func(AntennaTuple, int) []image.Point

		generateAntiNodes = func(tuple AntennaTuple, ind int) (antinodes []image.Point) {
			center := tuple.antennaB
			next := CentralSymetric(tuple.antennaA, center)

			if !isInMaze(next, ind) {
				return
			}

			antinodes = append(antinodes, next)

			if resonantHarmonics {
				antinodes = append(antinodes, generateAntiNodes(AntennaTuple{
					antennaA: center,
					antennaB: next,
				}, ind)...)
			}
			return
		}

		return arrays.FlatMap(symetryTuples, generateAntiNodes)
	}
}

func parse(data string) (maze [][]rune, antennasMap map[rune][]image.Point) {
	antennasMap = make(map[rune][]image.Point)
	maze = arrays.Map(strings.Split(data, "\n"), func(rawLine string, y int) (line []rune) {
		line = make([]rune, 0)
		for x, cell := range rawLine {
			line = append(line, cell)
			if cell != '.' {
				antennasMap[cell] = append(antennasMap[cell], image.Point{X: x, Y: y})
			}
		}
		return
	})
	return
}

func findAntiNodes(maze [][]rune, antennasMap map[rune][]image.Point, resonantHarmonics bool) (antinodes map[image.Point]bool) {
	antinodes = make(map[image.Point]bool)

	for _, antennas := range antennasMap {
		for i := 0; i < len(antennas)-1; i++ {
			for j := i + 1; j < len(antennas); j++ {
				tuple := AntennaTuple{
					antennaA: antennas[i],
					antennaB: antennas[j],
				}

				for _, antinode := range tuple.buildAntiNodesGenerator(maze)(resonantHarmonics) {
					antinodes[antinode] = true
				}

				if resonantHarmonics {
					antinodes[antennas[i]] = true
					antinodes[antennas[j]] = true
				}
			}
		}
	}
	return
}

func Part1(data string) int {
	maze, antennasMap := parse(data)

	antinodes := findAntiNodes(maze, antennasMap, false)

	return len(antinodes)
}

func Part2(data string) int {
	maze, antennasMap := parse(data)

	antinodes := findAntiNodes(maze, antennasMap, true)

	return len(antinodes)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
