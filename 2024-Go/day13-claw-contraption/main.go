package main

import (
	"errors"
	"fmt"
	"image"
	"os"
	"regexp"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

type Game struct {
	ButtonA image.Point
	ButtonB image.Point
	Prize   image.Point
}

func (game Game) tokensTowin(check100 bool) (int, error) {
	// A = (8400 * 67 - 5400 * 22) / (94 * 67 - 34 * 22) for first example
	aPressedNum := (game.Prize.X*game.ButtonB.Y - game.Prize.Y*game.ButtonB.X)
	aPressedDen := (game.ButtonA.X*game.ButtonB.Y - game.ButtonA.Y*game.ButtonB.X)

	if aPressedDen == 0 || aPressedNum%aPressedDen != 0 {
		return 0, errors.New("no solution")
	}

	aPressed := aPressedNum / aPressedDen

	// B = (8400 * 34 - 5400 * 94) / (22 * 34 - 67 * 94) for first example
	bPressedNum := (game.Prize.X*game.ButtonA.Y - game.Prize.Y*game.ButtonA.X)
	bPressedDen := (game.ButtonB.X*game.ButtonA.Y - game.ButtonB.Y*game.ButtonA.X)

	if bPressedDen == 0 || bPressedNum%bPressedDen != 0 {
		return 0, errors.New("no solution")
	}

	bPressed := bPressedNum / bPressedDen

	if check100 && (aPressed > 100 || bPressed > 100) {
		return 0, errors.New("pressed more than 100 times")
	}

	return 3*aPressed + bPressed, nil
}

func parse(input string) []Game {
	gameRegex, _ := regexp.Compile(`\d+`)
	return arrays.Map(strings.Split(input, "\n\n"), func(rawGame string, _ int) (game Game) {
		gameLines := strings.Split(rawGame, "\n")
		buttonA := arrays.Map(gameRegex.FindAllString(gameLines[0], -1), func(num string, _ int) int {
			return numbers.SafeConvertStrToInt(num)
		})
		buttonB := arrays.Map(gameRegex.FindAllString(gameLines[1], -1), func(num string, _ int) int {
			return numbers.SafeConvertStrToInt(num)
		})
		prize := arrays.Map(gameRegex.FindAllString(gameLines[2], -1), func(num string, _ int) int {
			return numbers.SafeConvertStrToInt(num)
		})

		game.ButtonA = image.Point{X: buttonA[0], Y: buttonA[1]}
		game.ButtonB = image.Point{X: buttonB[0], Y: buttonB[1]}
		game.Prize = image.Point{X: prize[0], Y: prize[1]}

		return
	})
}

func Part1(data string) int {
	games := parse(data)

	return arrays.SumInts(arrays.Map(games, func(game Game, _ int) int {
		tokensTowin, err := game.tokensTowin(true)

		if err != nil {
			return 0
		}

		return tokensTowin
	}))
}

func Part2(data string) int {
	games := parse(data)

	games = arrays.Map(games, func(game Game, _ int) Game {
		game.Prize.X += 10000000000000
		game.Prize.Y += 10000000000000
		return game
	})

	return arrays.SumInts(arrays.Map(games, func(game Game, _ int) int {
		tokensTowin, err := game.tokensTowin(false)

		if err != nil {
			return 0
		}

		return tokensTowin
	}))
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
