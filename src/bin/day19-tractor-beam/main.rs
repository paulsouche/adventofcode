use std::collections::{HashMap, VecDeque};
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day19-tractor-beam/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input));

    Ok(())
}

fn part1(prog_txt: &str) -> usize {
    let mut vm = IntcodeVM::new();

    let dim = 50;
    let mut sum = 0;
    for r in 0..dim {
        for c in 0..dim {
            vm.load(prog_txt.trim());
            vm.write_to_buff(c as i64);
            vm.write_to_buff(r as i64);
            vm.run();
            if vm.output_buffer == 1 {
                sum += 1;
            }
        }
    }
    sum
}

fn part2(prog_txt: &str) -> i64 {
    let mut x = 100;
    let mut y = 300;
    loop {
        while !pt_in_beam(x, y, &prog_txt) {
            x += 1;
        }

        while pt_in_beam(x + 99, y, &prog_txt) {
            if pt_in_beam(x, y + 99, &prog_txt) && pt_in_beam(x + 99, y + 99, &prog_txt) {
                return x * 10_000 + y;
            }
            x += 1;
        }

        y += 1;
    }
}

#[derive(Debug, PartialEq)]
enum VMState {
    Initialized,
    Ready,
    Running,
    Halted,
    AwaitInput,
    Paused,
}

#[derive(Debug)]
struct IntcodeVM {
    memory: HashMap<u64, i64>,
    ptr: i64,
    input_buffer: VecDeque<i64>,
    output_buffer: i64,
    state: VMState,
    rel_base: i64,
}

impl IntcodeVM {
    fn read(&self, loc: i64) -> i64 {
        match self.memory.get(&(loc as u64)) {
            Some(v) => *v,
            None => 0,
        }
    }

    fn write_to_buff(&mut self, v: i64) {
        self.input_buffer.push_front(v);
    }

    fn write(&mut self, loc: i64, val: i64) {
        *self.memory.entry(loc as u64).or_insert(val) = val;
    }

    fn get_write_dest(&self, d: i64, param_mode: i64) -> i64 {
        if param_mode == 0 {
            d
        } else {
            d + self.rel_base
        }
    }

    fn get_val(&self, p: i64, param_mode: i64) -> i64 {
        match param_mode {
            0 => self.read(p),
            1 => p,
            2 => self.read(p + self.rel_base),
            _ => panic!("Illegal parameter mode :o"),
        }
    }

    fn fetch_two_params(&self, loc: i64) -> (i64, i64) {
        (self.read(loc + 1), self.read(loc + 2))
    }

    fn fetch_three_params(&self, loc: i64) -> (i64, i64, i64) {
        (self.read(loc + 1), self.read(loc + 2), self.read(loc + 3))
    }

    fn run(&mut self) {
        self.state = VMState::Running;
        while self.state == VMState::Running {
            let instr = self.read(self.ptr);
            let opcode = instr - instr / 100 * 100;
            let mode1 = (instr - instr / 1000 * 1000) / 100 % 3;
            let mode2 = (instr - instr / 10000 * 10000) / 1000 % 3;
            let mode3 = instr / 10000 % 3;

            match opcode {
                1 => {
                    let (a, b, dest) = self.fetch_three_params(self.ptr);
                    self.write(
                        self.get_write_dest(dest, mode3),
                        self.get_val(a, mode1) + self.get_val(b, mode2),
                    );
                    self.ptr += 4;
                }
                2 => {
                    let (a, b, dest) = self.fetch_three_params(self.ptr);
                    self.write(
                        self.get_write_dest(dest, mode3),
                        self.get_val(a, mode1) * self.get_val(b, mode2),
                    );
                    self.ptr += 4;
                }
                3 => {
                    let mut dest = self.read(self.ptr + 1);
                    if mode1 == 2 {
                        dest += self.rel_base;
                    }

                    match self.input_buffer.pop_back() {
                        Some(v) => {
                            self.write(dest, v);
                            self.ptr += 2;
                        }
                        None => self.state = VMState::AwaitInput,
                    }
                }
                4 => {
                    let a = self.read(self.ptr + 1);
                    self.output_buffer = self.get_val(a, mode1);
                    self.ptr += 2;
                    self.state = VMState::Paused;
                }
                5 => {
                    let (a, jmp) = self.fetch_two_params(self.ptr);
                    if self.get_val(a, mode1) != 0 {
                        self.ptr = self.get_val(jmp, mode2);
                    } else {
                        self.ptr += 3;
                    }
                }
                6 => {
                    let (a, jmp) = self.fetch_two_params(self.ptr);
                    if self.get_val(a, mode1) == 0 {
                        self.ptr = self.get_val(jmp, mode2);
                    } else {
                        self.ptr += 3;
                    }
                }
                7 => {
                    let (a, b, dest) = self.fetch_three_params(self.ptr);
                    if self.get_val(a, mode1) < self.get_val(b, mode2) {
                        self.write(self.get_write_dest(dest, mode3), 1);
                    } else {
                        self.write(self.get_write_dest(dest, mode3), 0);
                    }
                    self.ptr += 4;
                }
                8 => {
                    let (a, b, dest) = self.fetch_three_params(self.ptr);
                    if self.get_val(a, mode1) == self.get_val(b, mode2) {
                        self.write(self.get_write_dest(dest, mode3), 1);
                    } else {
                        self.write(self.get_write_dest(dest, mode3), 0);
                    }
                    self.ptr += 4;
                }
                9 => {
                    let a = self.read(self.ptr + 1);
                    self.rel_base += self.get_val(a, mode1);
                    self.ptr += 2;
                }
                99 => self.state = VMState::Halted,
                _ => panic!("Hmm this shouldn't happen..."),
            }
        }
    }

    fn new() -> IntcodeVM {
        IntcodeVM {
            ptr: 0,
            memory: HashMap::new(),
            input_buffer: VecDeque::new(),
            output_buffer: 0,
            state: VMState::Initialized,
            rel_base: 0,
        }
    }

    fn load(&mut self, prog_txt: &str) {
        let arr: Vec<i64> = prog_txt
            .split(",")
            .map(|a| a.parse::<i64>().unwrap())
            .collect();
        for loc in 0..arr.len() {
            self.memory.insert(loc as u64, arr[loc]);
        }

        self.ptr = 0;
        self.state = VMState::Ready;
    }
}

fn pt_in_beam(x: i64, y: i64, prog_txt: &str) -> bool {
    let mut vm = IntcodeVM::new();
    vm.load(prog_txt);
    vm.write_to_buff(x);
    vm.write_to_buff(y);
    vm.run();
    vm.output_buffer == 1
}
