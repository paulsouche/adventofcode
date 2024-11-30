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
