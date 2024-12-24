package main

import (
	"fmt"
	"image"
	"os"
	"reflect"
	"strings"

	"adventofcode/common/arrays"
)

const (
	LOCK = iota
	KEY
)

type Schematic struct {
	Raw  [][]string
	Kind int
	Max  image.Point
}

func (schematic Schematic) ToHeight() (heights []int) {
	for x := 0; x <= schematic.Max.X; x++ {
		for y := 0; y <= schematic.Max.Y-1; y++ {
			if schematic.Raw[y+1][x] == "." {
				heights = append(heights, y)
				break
			}
		}
	}
	return
}

func reverse(s interface{}) {
	n := reflect.ValueOf(s).Len()
	swap := reflect.Swapper(s)
	for i, j := 0, n-1; i < j; i, j = i+1, j-1 {
		swap(i, j)
	}
}

func parse(input string) (schematics []Schematic) {
	for _, rawSchematic := range strings.Split(input, "\n\n") {
		schematic := Schematic{
			Raw: make([][]string, 0),
		}

		for y, line := range strings.Split(rawSchematic, "\n") {
			if y == 0 {
				if !strings.Contains(line, ".") {
					schematic.Kind = LOCK
				} else {
					schematic.Kind = KEY
				}
			}

			schematic.Raw = append(schematic.Raw, make([]string, 0))
			for x, cell := range strings.Split(line, "") {
				schematic.Raw[y] = append(schematic.Raw[y], cell)
				schematic.Max.Y = max(schematic.Max.Y, y)
				schematic.Max.X = max(schematic.Max.X, x)
			}
		}

		if schematic.Kind == KEY {
			reverse(schematic.Raw)
		}

		schematics = append(schematics, schematic)
	}
	return
}

func schematicsToHeights(schematics []Schematic, kind int) [][]int {
	return arrays.Map(arrays.Filter(schematics, func(schematic Schematic, _ int) bool {
		return schematic.Kind == kind
	}), func(schematic Schematic, _ int) []int {
		return schematic.ToHeight()
	})
}

func Part1(data string) int {
	schematics := parse(data)

	locks := schematicsToHeights(schematics, LOCK)

	keys := schematicsToHeights(schematics, KEY)

	return arrays.SumInts(arrays.Map(locks, func(lock []int, _ int) int {
		return len(arrays.Filter(keys, func(key []int, _ int) bool {
			return arrays.Every(key, func(height int, ind int) bool {
				return (height + lock[ind]) < 6
			})
		}))
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
}
