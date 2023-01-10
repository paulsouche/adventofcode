import re


class Program:
    def find_monad_boundaries(program):
        operations = []
        digit_operations = None
        for instruction in program.instructions:
            if instruction.group(1) == 'inp':
                if digit_operations:
                    operations.append(digit_operations)
                digit_operations = []
            else:
                digit_operations.append(instruction)
        operations.append(digit_operations)

        max_monad = [0] * 14
        min_monad = [0] * 14
        z = []
        for i, digit_operations in enumerate(operations):
            pop = int(digit_operations[3].group(4)) == 26
            x_add = int(digit_operations[4].group(4))
            y_add = int(digit_operations[14].group(4))
            if not pop:
                # push digit to z
                z.append([i, y_add])
            else:
                # apply restriction: last_z_digit == current_z_digit + difference
                j, y_add = z.pop()
                difference = x_add + y_add
                if difference < 0:
                    max_monad[i] = 9 + difference
                    max_monad[j] = 9
                    min_monad[i] = 1
                    min_monad[j] = 1 - difference
                elif difference > 0:
                    max_monad[i] = 9
                    max_monad[j] = 9 - difference
                    min_monad[i] = 1 + difference
                    min_monad[j] = 1
                else:
                    max_monad[i] = max_monad[j] = 9
                    min_monad[i] = min_monad[j] = 1

        return [min_monad, max_monad]

    def __init__(self, raw_instructions):
        self.instructions = list((map(lambda raw_instruction: re.search('^(inp|add|mul|div|mod|eql)\s(-?[0-9a-z]+)(\s(-?[0-9a-z]+))?$', raw_instruction), raw_instructions)))
        self.reset()

    def reset(self):
        self.memory = {
            'w': 0,
            'x': 0,
            'y': 0,
            'z': 0,
        }
        return self

    def run(self, input):
        input_ind = 0
        for instruction in self.instructions:
            command = instruction.group(1)
            memory_key = instruction.group(2)
            if command == 'inp':
                self.memory[memory_key] = int(input[input_ind])
                input_ind += 1
            elif command == 'add':
                to_add_key = instruction.group(4)
                to_add = self.memory[to_add_key] if to_add_key in self.memory else int(to_add_key)
                self.memory[memory_key] += to_add
            elif command == 'mul':
                to_mul_key = instruction.group(4)
                to_mul = self.memory[to_mul_key] if to_mul_key in self.memory else int(to_mul_key)
                self.memory[memory_key] *= to_mul
            elif command == 'div':
                to_div_key = instruction.group(4)
                to_div = self.memory[to_div_key] if to_div_key in self.memory else int(to_div_key)
                if to_div == 0:
                    raise 'To div is zero'
                self.memory[memory_key] = int(self.memory[memory_key] / to_div)
            elif command == 'mod':
                to_mod_key = instruction.group(4)
                to_mod = self.memory[to_mod_key] if to_mod_key in self.memory else int(to_mod_key)
                if to_mod == 0:
                    raise 'To mod is zero'
                self.memory[memory_key] = self.memory[memory_key] % to_mod
            elif command == 'eql':
                to_eql_key = instruction.group(4)
                to_eql = self.memory[to_eql_key] if to_eql_key in self.memory else int(to_eql_key)
                self.memory[memory_key] = 1 if self.memory[memory_key] == to_eql else 0

        return self

def part1(raw_instructions):
    program = Program(raw_instructions)
    _, max_monad = Program.find_monad_boundaries(program)
    return "".join(map(str, max_monad))

def part2(raw_instructions):
    program = Program(raw_instructions)
    min_monad, _ = Program.find_monad_boundaries(program)
    return "".join(map(str, min_monad))
