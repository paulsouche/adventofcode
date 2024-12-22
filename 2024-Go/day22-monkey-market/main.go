package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"

	"adventofcode/common/arrays"
	"adventofcode/common/numbers"
)

func parse(intput string) []int {
	return arrays.Map(strings.Split(intput, "\n"), func(line string, _ int) int {
		return numbers.SafeConvertStrToInt(line)
	})
}

func secretSequence(secretCode int) int {
	value := secretCode * 64
	secretCode ^= value
	secretCode %= 16777216
	value = secretCode / 32
	secretCode ^= value
	secretCode %= 16777216
	value = secretCode * 2048
	secretCode ^= value
	secretCode %= 16777216
	return secretCode
}

func secretBananas(secretCode int) int {
	return secretCode % 10
}

func secretOffer(secretCode int, sequences int) (bananasForVariations map[string]int) {
	bananas := make([]int, sequences+1)
	variations := make([]int, sequences+1)
	firstPrice := secretBananas(secretCode)
	bananas[0] = firstPrice
	variations[0] = firstPrice
	for s := 1; s < sequences+1; s++ {
		secretCode = secretSequence(secretCode)
		babanasResult := secretBananas(secretCode)
		bananas[s] = babanasResult
		variations[s] = babanasResult - bananas[s-1]
	}
	bananasForVariations = make(map[string]int)
	for index := 3; index < sequences+1; index++ {
		key := variationsToKey(variations[index-3 : index+1])
		_, hasPrice := bananasForVariations[key]

		if !hasPrice {
			bananasForVariations[key] = bananas[index]
		}
	}
	return
}

func variationsToKey(variations []int) string {
	if len(variations) != 4 {
		panic("4 variations expected")
	}

	return strings.Join(arrays.Map(variations, func(variation int, _ int) string {
		return strconv.Itoa(variation)
	}), ",")
}

func Part1(data string, sequences int) int {
	secretsCode := parse(data)

	return arrays.SumInts(arrays.Map(secretsCode, func(secretCode int, _ int) int {
		for i := 0; i < sequences; i++ {
			secretCode = secretSequence(secretCode)
		}
		return secretCode
	}))
}

func Part2(data string, sequences int) int {
	secretsCode := parse(data)
	maxBananas := 0

	offers := arrays.Map(secretsCode, func(secretCode int, _ int) map[string]int {
		return secretOffer(secretCode, sequences)
	})

	memoize := make(map[string]int)
	for _, offer := range offers {
		for variationsSequence := range offer {
			_, knowScore := memoize[variationsSequence]

			if knowScore {
				continue
			}

			bananas := 0
			for _, bananasForVariation := range offers {
				bananas += bananasForVariation[variationsSequence]
			}

			memoize[variationsSequence] = bananas

			maxBananas = max(maxBananas, bananas)
		}
	}

	return maxBananas
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data, 2000))
	fmt.Println(Part2(data, 2000))
}
