use std::collections::VecDeque;
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day21-springdroid-adventure/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input));

    Ok(())
}

fn part1(input: &str) -> i64 {
    let memory = read_codes(input);
    let mut intcode = Intcode::new(memory);
    let script = r"NOT B J
NOT C T
OR T J
AND D J
NOT A T
OR T J
WALK
";
    springdroid(&mut intcode, script)
}

fn part2(input: &str) -> i64 {
    let memory = read_codes(input);
    let mut intcode = Intcode::new(memory);
    let script = r"NOT B J
NOT C T
OR T J
AND D J
AND H J
NOT A T
OR T J
RUN
";
    springdroid(&mut intcode, script)
}

fn springdroid(intcode: &mut Intcode, script: &str) -> i64 {
    for c in script.chars() {
        intcode.input.push_back(c as i64);
    }
    while intcode.finished != true {
        intcode.run_until_output();
    }
    if let Some(num) = intcode.output.back() {
        *num
    } else {
        0
    }
}

fn read_codes(input: &str) -> Vec<i64> {
    let mut result = Vec::new();

    for number_str in input.trim().split(',') {
        result.push(number_str.parse().unwrap());
    }
    result
}

fn decode_op(code: i64) -> (ParameterMode, ParameterMode, ParameterMode, i64) {
    let de = code % 100;
    let c = ParameterMode::decode(code / 100 % 10);
    let b = ParameterMode::decode(code / 1000 % 10);
    let a = ParameterMode::decode(code / 10000 % 10);

    (a, b, c, de)
}

enum ParameterMode {
    Position,
    Immediate,
    Relative,
}

impl ParameterMode {
    fn decode(n: i64) -> ParameterMode {
        match n {
            0 => ParameterMode::Position,
            1 => ParameterMode::Immediate,
            2 => ParameterMode::Relative,
            _ => panic!("Unexpected parameter mode"),
        }
    }
}

#[derive(Clone)]
struct Intcode {
    memory: Vec<i64>,
    ptr: usize,
    input: VecDeque<i64>,
    output: VecDeque<i64>,
    relative: i64,
    finished: bool,
}

impl Intcode {
    fn new(memory: Vec<i64>) -> Self {
        Intcode {
            memory,
            ptr: 0,
            relative: 0,
            input: VecDeque::new(),
            output: VecDeque::new(),
            finished: false,
        }
    }

    fn load_value(&mut self, index: usize, mode: ParameterMode) -> i64 {
        match mode {
            ParameterMode::Position => {
                let position = self.read_memory(index) as usize;
                self.read_memory(position)
            }
            ParameterMode::Immediate => self.read_memory(index),
            ParameterMode::Relative => {
                let position = self.read_memory(index) + self.relative;
                self.read_memory(position as usize)
            }
        }
    }

    fn save_value(&mut self, index: usize, mode: ParameterMode, value: i64) {
        match mode {
            ParameterMode::Position => {
                let position = self.read_memory(index) as usize;
                self.write_memory(position, value);
            }
            ParameterMode::Immediate => {
                self.write_memory(index, value);
            }
            ParameterMode::Relative => {
                let position = self.read_memory(index) + self.relative;
                self.write_memory(position as usize, value);
            }
        };
    }

    fn read_memory(&mut self, index: usize) -> i64 {
        if self.memory.len() <= index {
            self.memory.resize(index + 1, 0);
        }
        self.memory[index]
    }

    fn write_memory(&mut self, index: usize, value: i64) {
        if self.memory.len() <= index {
            self.memory.resize(index + 1, 0);
        }
        self.memory[index] = value;
    }

    fn run_instruction(&mut self) -> i64 {
        if self.finished {
            return 99;
        }

        let pointer = self.ptr;
        let code = self.memory[pointer];
        let (arg3_mode, arg2_mode, arg1_mode, op) = decode_op(code);
        let next_pointer = match op {
            1 | 2 => {
                let value1 = self.load_value(pointer + 1, arg1_mode);
                let value2 = self.load_value(pointer + 2, arg2_mode);
                let operation_result = match op {
                    1 => value1 + value2,
                    2 => value1 * value2,
                    _ => 0,
                };
                self.save_value(pointer + 3, arg3_mode, operation_result);
                pointer + 4
            }
            3 => {
                let input_value = self.input.pop_front().expect("Could not get input.");
                self.save_value(pointer + 1, arg1_mode, input_value);
                pointer + 2
            }
            4 => {
                let v = self.load_value(pointer + 1, arg1_mode);
                self.output.push_back(v);
                pointer + 2
            }
            5 => {
                let par1 = self.load_value(pointer + 1, arg1_mode);
                let par2 = self.load_value(pointer + 2, arg2_mode) as usize;
                if par1 != 0 {
                    par2
                } else {
                    pointer + 3
                }
            }
            6 => {
                let par1 = self.load_value(pointer + 1, arg1_mode);
                let par2 = self.load_value(pointer + 2, arg2_mode) as usize;
                if par1 == 0 {
                    par2
                } else {
                    pointer + 3
                }
            }
            7 => {
                let par1 = self.load_value(pointer + 1, arg1_mode);
                let par2 = self.load_value(pointer + 2, arg2_mode);

                let store_value = if par1 < par2 { 1 } else { 0 };
                self.save_value(pointer + 3, arg3_mode, store_value);
                pointer + 4
            }
            8 => {
                let par1 = self.load_value(pointer + 1, arg1_mode);
                let par2 = self.load_value(pointer + 2, arg2_mode);

                let store_value = if par1 == par2 { 1 } else { 0 };
                self.save_value(pointer + 3, arg3_mode, store_value);
                pointer + 4
            }
            9 => {
                let par1 = self.load_value(pointer + 1, arg1_mode);
                self.relative += par1;
                pointer + 2
            }
            99 => {
                self.finished = true;
                pointer + 1
            }
            _ => pointer + 1,
        };
        self.ptr = next_pointer;
        op
    }

    fn run_until_output(&mut self) {
        while self.ptr < self.memory.len() {
            let op = self.run_instruction();
            if op == 99 || op == 4 {
                break;
            }
        }
    }
}
