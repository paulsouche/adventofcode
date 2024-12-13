package numbers

import "strconv"

func AbsDiffInt(x, y int) int {
	if x < y {
		return y - x
	} else {
		return x - y
	}
}

func SafeConvertStrToInt(str string) (integer int) {
	integer, err := strconv.Atoi(str)
	if err != nil {
		panic(err)
	}
	return
}

func GCD(a, b int) int {
	for b != 0 {
		t := b
		b = a % b
		a = t
	}
	return a
}

func LCM(integers ...int) int {
	result := integers[0] * integers[1] / GCD(integers[0], integers[1])

	for i := 0; i < len(integers[2:]); i++ {
		result = LCM(result, integers[i+2])
	}

	return result
}
