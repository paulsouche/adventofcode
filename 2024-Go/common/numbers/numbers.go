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
