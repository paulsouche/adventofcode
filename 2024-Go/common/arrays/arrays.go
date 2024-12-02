package arrays

func Map[T, U any](array []T, transform func(T, int) U) (output []U) {
	for index, item := range array {
		output = append(output, transform(item, index))
	}
	return
}

func SumInts(s []int) (total int) {
	total = 0
	for _, val := range s {
		total += val
	}
	return
}

func Filter[T any](array []T, predicate func(T, int) bool) (output []T) {
	for index, item := range array {
		if predicate(item, index) {
			output = append(output, item)
		}
	}
	return
}

func Some[T any](array []T, predicate func(T, int) bool) (output bool) {
	output = false
	for index, item := range array {
		if predicate(item, index) {
			output = true
			break
		}
	}
	return
}

func MultiplyInts(s []int) (total int) {
	total = 1
	for _, val := range s {
		total *= val
	}
	return
}
