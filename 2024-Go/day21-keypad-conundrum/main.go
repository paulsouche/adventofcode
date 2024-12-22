package main

import (
	"adventofcode/common/numbers"
	"fmt"
	"os"
	"regexp"
	"strings"
)

var NUMERIC_KEYPAD_MAP = map[string]string{
	"A0": "<",
	"A1": "^<<",
	"A2": "^<",
	"A3": "^",
	"A4": "^^<<",
	"A5": "^^<",
	"A6": "^^",
	"A7": "^^^<<",
	"A8": "^^^<",
	"A9": "^^^",

	"0A": ">",
	"01": "^<",
	"02": "^",
	"03": "^>",
	"04": "^^<",
	"05": "^^",
	"06": "^^>",
	"07": "^^^<",
	"08": "^^^",
	"09": "^^^>",

	"10": ">v",
	"1A": ">>v",
	"12": ">",
	"13": ">>",
	"14": "^",
	"15": "^>",
	"16": "^>>",
	"17": "^^",
	"18": "^^>",
	"19": "^^>>",

	"20": "v",
	"2A": ">v",
	"21": "<",
	"23": ">",
	"24": "^<",
	"25": "^",
	"26": "^>",
	"27": "^^<",
	"28": "^^",
	"29": "^^>",

	"30": "v<",
	"3A": "v",
	"31": "<<",
	"32": "<",
	"34": "^<<",
	"35": "^<",
	"36": "^",
	"37": "^^<<",
	"38": "^^<",
	"39": "^^",

	"40": ">vv",
	"4A": ">>vv",
	"41": "v",
	"42": ">v",
	"43": ">>v",
	"45": ">",
	"46": ">>",
	"47": "^",
	"48": "^>",
	"49": "^>>",

	"50": "vv",
	"5A": ">vv",
	"51": "v<",
	"52": "v",
	"53": ">v",
	"54": "<",
	"56": ">",
	"57": "^<",
	"58": "^",
	"59": "^>",

	"60": "vv<",
	"6A": "vv",
	"61": "v<<",
	"62": "v<",
	"63": "v",
	"64": "<<",
	"65": "<",
	"67": "^<<",
	"68": "^<",
	"69": "^",

	"70": ">vvv",
	"7A": ">>vvv",
	"71": "vv",
	"72": ">vv",
	"73": ">>vv",
	"74": "v",
	"75": ">v",
	"76": ">>v",
	"78": ">",
	"79": ">>",

	"80": "vvv",
	"8A": ">vvv",
	"81": "vv<",
	"82": "vv",
	"83": ">vv",
	"84": "v<",
	"85": "v",
	"86": "v>",
	"87": "<",
	"89": ">",

	"90": "vvv<",
	"9A": "vvv",
	"91": "vv<<",
	"92": "vv<",
	"93": "vv",
	"94": "v<<",
	"95": "v<",
	"96": "v",
	"97": "<<",
	"98": "<",
}

// var NUMERIC_KEYPAD_MAP = map[string][]string{
// 	"A0": {"<"},
// 	"A1": {"^<<"},
// 	"A2": {"^<", "<^"},
// 	"A3": {"^"},
// 	"A4": {"^^<<"},
// 	"A5": {"^^<","<^^"},
// 	"A6": {"^^"},
// 	"A7": {"^^^<<"},
// 	"A8": {"^^^<", "<^^^"},
// 	"A9": {"^^^"},

// 	"0A": {">"},
// 	"01": {"^<"},
// 	"02": {"^"},
// 	"03": {"^>", ">^"},
// 	"04": {"^^<"},
// 	"05": {"^^"},
// 	"06": {"^^>", ">^^"},
// 	"07": {"^^^<"},
// 	"08": {"^^^"},
// 	"09": {"^^^>", ">^^^"},

// 	"10": {">v"},
// 	"1A": {">>v"},
// 	"12": {">"},
// 	"13": {">>"},
// 	"14": {"^"},
// 	"15": {"^>", ">^"},
// 	"16": {"^>>", ">>^"},
// 	"17": {"^^"},
// 	"18": {"^^>", ">^^"},
// 	"19": {"^^>>", ">>^^"},

// 	"20": {"v"},
// 	"2A": {">v", "v>"},
// 	"21": {"<"},
// 	"23": {">"},
// 	"24": {"^<", "<^"},
// 	"25": {"^"},
// 	"26": {"^>", ">^"},
// 	"27": {"^^<", "<^^"},
// 	"28": {"^^"},
// 	"29": {"^^>", ">^^"},

// 	"30": {"v<", "<v"},
// 	"3A": {"v"},
// 	"31": {"<<"},
// 	"32": {"<"},
// 	"34": {"^<<", "<<^"},
// 	"35": {"^<"},
// 	"36": {"^"},
// 	"37": {"^^<<", "<<^^"},
// 	"38": {"^^<", "<^^"},
// 	"39": {"^^"},

// 	"40": ">vv",
// 	"4A": ">>vv",
// 	"41": "v",
// 	"42": ">v",
// 	"43": ">>v",
// 	"45": ">",
// 	"46": ">>",
// 	"47": "^",
// 	"48": "^>",
// 	"49": "^>>",

// 	"50": "vv",
// 	"5A": ">vv",
// 	"51": "v<",
// 	"52": "v",
// 	"53": ">v",
// 	"54": "<",
// 	"56": ">",
// 	"57": "^<",
// 	"58": "^",
// 	"59": "^>",

// 	"60": "vv<",
// 	"6A": "vv",
// 	"61": "v<<",
// 	"62": "v<",
// 	"63": "v",
// 	"64": "<<",
// 	"65": "<",
// 	"67": "^<<",
// 	"68": "^<",
// 	"69": "^",

// 	"70": ">vvv",
// 	"7A": ">>vvv",
// 	"71": "vv",
// 	"72": ">vv",
// 	"73": ">>vv",
// 	"74": "v",
// 	"75": ">v",
// 	"76": ">>v",
// 	"78": ">",
// 	"79": ">>",

// 	"80": "vvv",
// 	"8A": ">vvv",
// 	"81": "vv<",
// 	"82": "vv",
// 	"83": ">vv",
// 	"84": "v<",
// 	"85": "v",
// 	"86": "v>",
// 	"87": "<",
// 	"89": ">",

// 	"90": "vvv<",
// 	"9A": "vvv",
// 	"91": "vv<<",
// 	"92": "vv<",
// 	"93": "vv",
// 	"94": "v<<",
// 	"95": "v<",
// 	"96": "v",
// 	"97": "<<",
// 	"98": "<",
// }

var DIRECTIONAL_KEYPAD_MAP = map[string][]string{
	"<v": {">"},
	"<>": {">>"},
	"<^": {">^"},
	"<A": {">>^", ">^>"},

	"v<": {"<"},
	"v>": {">"},
	"v^": {"^"},
	"vA": {">^", "^>"},

	"><": {"<<"},
	">v": {"<"},
	">^": {"^<", "<^"},
	">A": {"^"},

	"^<": {"v<"},
	"^v": {"v"},
	"^>": {">v", "v>"},
	"^A": {">"},

	"A<": {"v<<", "<v<"},
	"Av": {"v<", "<v"},
	"A>": {"v"},
	"A^": {"<"},
}

func numericKeypadToDirectionalKeypad(code string) (out string) {
	goal := "A" + code
	for i := 0; i < len(goal)-1; i++ {
		out += NUMERIC_KEYPAD_MAP[goal[i:i+2]]
		out += "A"
	}
	return
}

func directionalKeypadToDirectionalKeypad(code string) (out []string) {
	goal := "A" + code
	for i := 0; i < len(goal)-1; i++ {
		// out += DIRECTIONAL_KEYPAD_MAP[goal[i:i+2]]
		// out += "A"
	}
	return
}

func parse(input string) (out []string) {
	return strings.Split(input, "\n")
}

func codeNumericPart(code string) (out int) {
	r, _ := regexp.Compile(`\d+`)
	return numbers.SafeConvertStrToInt(strings.Join(r.FindAllString(code, -1), ""))
}

func Part1(data string) int {
	// codes := parse(data)

	fmt.Println(numericKeypadToDirectionalKeypad("029A"))
	fmt.Println(directionalKeypadToDirectionalKeypad(numericKeypadToDirectionalKeypad("029A")))
	// fmt.Println(directionalKeypadToDirectionalKeypad(directionalKeypadToDirectionalKeypad(numericKeypadToDirectionalKeypad("029A"))))

	// return arrays.SumInts(arrays.Map(codes, func (code string, _ int) int {
	// 	input1 := numericKeypadToDirectionalKeypad(code)
	// 	input2 := directionalKeypadToDirectionalKeypad(input1)
	// 	input3 := directionalKeypadToDirectionalKeypad(input2)
	// 	return codeNumericPart(code) * len(input3)
	// }))

	return len(data)
}

func Part2(data string) int {
	return len(data)
}

func main() {
	file, _ := os.ReadFile("input.txt")
	data := string(file)
	fmt.Println(Part1(data))
	fmt.Println(Part2(data))
}
