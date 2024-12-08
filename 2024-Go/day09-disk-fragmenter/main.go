package main

import (
	"fmt"
	"os"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(input string) []int {
	return arrays.Map(strings.Split(input, ""), func(rawBlock string, _ int) int {
		return numbers.SafeConvertStrToInt(rawBlock)
	})
}

func Part1(data string) int {
	fileSystem := parse(data)

	fileBlocksLength := arrays.SumInts(arrays.Filter(fileSystem, func(_ int, index int) bool {
		return index%2 == 0
	}))

	fileBlocks := make([]int, fileBlocksLength)
	i := 0
	id := len(fileSystem) - 1

blocksLoop:
	for j := 0; j < len(fileSystem); j++ {
		if j%2 == 0 {
			for k := fileSystem[j]; k > 0; k-- {
				fileBlocks[i] = j / 2
				i++
			}
		} else {
			emptyFields := fileSystem[j]
			for k := 0; k < emptyFields; k++ {
				if fileSystem[id] == 0 {
					id -= 2
				}
				if i >= fileBlocksLength {
					break blocksLoop
				}
				fileBlocks[i] = id / 2
				fileSystem[id]--
				i++
			}
		}
	}

	return arrays.SumInts(arrays.Map(fileBlocks, func(block int, id int) int {
		return block * id
	}))
}

func Part2(data string) int {
	fileSystem := parse(data)

	fileBlocks := make([]int, arrays.SumInts(fileSystem))
	recs := make(map[int]int)
	i := 0

	for j := 0; j < len(fileSystem); j++ {
		recs[j] = i
		if j%2 == 0 {
			for k := fileSystem[j]; k > 0; k-- {
				fileBlocks[i] = j / 2
				i++
			}
		} else {
			i += fileSystem[j]
		}
	}

	id := 1
	for j := len(fileSystem) - 1; j >= 0; j -= 2 {
		for k := id; k <= j; k += 2 {
			if fileSystem[k] >= fileSystem[j] {
				i := recs[k]
				for l := 0; l < fileSystem[j]; l++ {
					fileBlocks[i] = j / 2
					i++
				}

				i = recs[j]
				for l := 0; l < fileSystem[j]; l++ {
					fileBlocks[i] = 0
					i++
				}

				fileSystem[k] -= fileSystem[j]
				recs[k] += fileSystem[j]
				fileSystem[j] = 0

				if fileSystem[id] == 0 {
					id += 2
				}
				break
			}
		}
	}

	return arrays.SumInts(arrays.Map(fileBlocks, func(block int, id int) int {
		return block * id
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
