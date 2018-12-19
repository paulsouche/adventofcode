import {
  addi,
  addr,
  bani,
  banr,
  bori,
  borr,
  eqir,
  eqri,
  eqrr,
  gtir,
  gtri,
  gtrr,
  muli,
  mulr,
  OpCode,
  seti,
  setr,
} from '../../day16-chronal-classification/lib';

const ipReg = /^#ip\s(\d+)$/;
const progReg = /^(\w+)\s(\d+)\s(\d+)\s(\d+)$/;
const operationMap: {
  [opCode: string]: OpCode,
} = {
  addi,
  addr,
  bani,
  banr,
  bori,
  borr,
  eqir,
  eqri,
  eqrr,
  gtir,
  gtri,
  gtrr,
  muli,
  mulr,
  seti,
  setr,
};

function parseIp(str: string | undefined) {
  const match = ipReg.exec(str || '');

  if (!match) {
    throw new Error(`Invalid ip input ${str}`);
  }

  return +match[1];
}

function parseProg(str: string) {
  const match = progReg.exec(str || '');

  if (!match) {
    throw new Error(`Invalid program input ${str}`);
  }

  const op = operationMap[match[1]];

  if (!op) {
    throw new Error(`No op found ${match[1]}`);
  }

  return op.instruction(...match.slice(1).map((m) => +m));
}

export default (input: string[], registerStart = 0) => {
  const ip = parseIp(input.slice(0).shift());
  const programMap = input.slice(1).map(parseProg);
  const register = [registerStart, 0, 0, 0, 0, 0];
  let i = register[ip];
  let instruction = programMap[i];
  while (instruction) {
    register[ip] = i;
    instruction(register);
    i = register[ip];
    i++;
    instruction = programMap[i];

    if (i === 1) {
      // This is the first program specific part when instruction 1 is reached number is known
      break;
    }
  }

  // This is the second program specific part my number is in register 1
  const largeNumber = register[1];
  let sum = 0;
  for (i = 1; i <= largeNumber; i++) {
    if (largeNumber % i === 0) {
      sum += i;
    }
  }
  return sum;
};
