package coordinates

import (
	"image"

	"adventofcode/common/arrays"
)

var Adjacents = []image.Point{
	{
		X: -1,
		Y: -1,
	},
	{
		X: -1,
		Y: 0,
	},
	{
		X: -1,
		Y: 1,
	},
	{
		X: 0,
		Y: -1,
	},
	{
		X: 0,
		Y: 1,
	},
	{
		X: 1,
		Y: -1,
	},
	{
		X: 1,
		Y: 0,
	},
	{
		X: 1,
		Y: 1,
	},
}

var CrossBranches = [][]image.Point{
	{
		{
			X: -1,
			Y: -1,
		},
		{
			X: 1,
			Y: 1,
		},
	},
	{
		{
			X: -1,
			Y: 1,
		},
		{
			X: 1,
			Y: -1,
		},
	},
}

func IsInSquareArray[T any](array [][]T) func(image.Point, int) bool {
	return func(point image.Point, _ int) bool {
		if point.Y < 0 || point.Y >= len(array) {
			return false
		}

		if point.X < 0 || point.X >= len(array[point.Y]) {
			return false
		}

		return true
	}
}

func GetAdjacentCells(cell image.Point) func([]image.Point, int) []image.Point {
	return func(adjacents []image.Point, _ int) []image.Point {
		return arrays.Map(adjacents, func(adjacent image.Point, _ int) image.Point {
			return cell.Add(adjacent)
		})
	}
}
