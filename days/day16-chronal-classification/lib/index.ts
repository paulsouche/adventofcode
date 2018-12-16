type Register = [number, number, number, number];

type Instruction = (...args: number[]) => (register: Register) => Register;

interface OpCode {
  name: string;
  instruction: Instruction;
}

export const addr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] + register[b];
      return register;
    };
  },
  name: 'addr',
};

export const addi: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] + b;
      return register;
    };
  },
  name: 'addi',
};

export const mulr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] * register[b];
      return register;
    };
  },
  name: 'mulr',
};

export const muli: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] * b;
      return register;
    };
  },
  name: 'muli',
};

export const banr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] & register[b];
      return register;
    };
  },
  name: 'banr',
};

export const bani: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] & b;
      return register;
    };
  },
  name: 'bani',
};

export const borr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] | register[b];
      return register;
    };
  },
  name: 'borr',
};

export const bori: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] | b;
      return register;
    };
  },
  name: 'bori',
};

export const setr: OpCode = {
  instruction: (...args) => {
    const [, a, , c] = args;
    return (register) => {
      register[c] = register[a];
      return register;
    };
  },
  name: 'setr',
};

export const seti: OpCode = {
  instruction: (...args) => {
    const [, a, , c] = args;
    return (register) => {
      register[c] = a;
      return register;
    };
  },
  name: 'seti',
};

export const gtir: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = a > register[b] ? 1 : 0;
      return register;
    };
  },
  name: 'gtir',
};

export const gtri: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] > b ? 1 : 0;
      return register;
    };
  },
  name: 'gtri',
};

export const gtrr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] > register[b] ? 1 : 0;
      return register;
    };
  },
  name: 'gtrr',
};

export const eqir: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = a === register[b] ? 1 : 0;
      return register;
    };
  },
  name: 'eqir',
};

export const eqri: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] === b ? 1 : 0;
      return register;
    };
  },
  name: 'eqri',
};

export const eqrr: OpCode = {
  instruction: (...args) => {
    const [, a, b, c] = args;
    return (register) => {
      register[c] = register[a] === register[b] ? 1 : 0;
      return register;
    };
  },
  name: 'eqrr',
};

const instructions = [
  addr,
  addi,
  mulr,
  muli,
  banr,
  bani,
  borr,
  bori,
  setr,
  seti,
  gtir,
  gtri,
  gtrr,
  eqir,
  eqri,
  eqrr,
];

const beforeReg = /Before:\s\[(\d+),\s(\d+),\s(\d+),\s(\d+)\]/;

function toRegisterBefore(str: string): Register {
  const matchBefore = beforeReg.exec(str);
  if (!matchBefore) {
    throw new Error(`Invalid example before ${str}`);
  }
  return [
    +matchBefore[1],
    +matchBefore[2],
    +matchBefore[3],
    +matchBefore[4],
  ];
}

const opReg = /(\d+)\s(\d+)\s(\d+)\s(\d+)/;

function toOp(str: string): number[] {
  const opMatch = opReg.exec(str);
  if (!opMatch) {
    throw new Error(`Invalid example opCode ${str}`);
  }
  return [
    +opMatch[1],
    +opMatch[2],
    +opMatch[3],
    +opMatch[4],
  ];
}

const afterReg = /After:\s+\s\[(\d+),\s(\d+),\s(\d+),\s(\d+)\]/;

function toRegisterAfter(str: string): string {
  const matchAfter = afterReg.exec(str);
  if (!matchAfter) {
    throw new Error(`Invalid example after ${str}`);
  }
  return `${matchAfter[1]},${matchAfter[2]},${matchAfter[3]},${matchAfter[4]}`;
}

export function exampleToInstructions(example: string[]) {
  const registerBefore = toRegisterBefore(example[0]);
  const op = toOp(example[1]);
  const registerAfter = toRegisterAfter(example[2]);

  return {
    code: op[0],
    possibleInstructions: instructions
      .filter((i) => {
        const register = registerBefore.slice(0) as Register;
        return registerAfter === i.instruction(...op)(register).join();
      }),
  };
}

export default (examples: string[], program: string[]) => {
  let samples = 0;
  let i: number;
  let ilen = examples.length;
  const possiblesInstructionsByCode: {
    [opCode: number]: OpCode[];
    length: number;
  } = [];

  for (i = 0; i < ilen; i += 4) {
    const { code, possibleInstructions } = exampleToInstructions([
      examples[i],
      examples[i + 1],
      examples[i + 2],
    ]);

    if (possibleInstructions.length >= 3) {
      samples++;
    }

    if (!possiblesInstructionsByCode[code]) {
      possiblesInstructionsByCode[code] = possibleInstructions;
      continue;
    }

    possiblesInstructionsByCode[code] = possiblesInstructionsByCode[code]
      .filter((pi) => possibleInstructions.indexOf(pi) >= 0);
  }

  const instructionsByCode: {
    [opCode: number]: OpCode;
  } = [];
  const knownOpCode: OpCode[] = [];
  i = 0;
  ilen = possiblesInstructionsByCode.length;
  while (knownOpCode.length < ilen) {
    const ops = possiblesInstructionsByCode[i];
    if (ops.length > 1) {
      possiblesInstructionsByCode[i] = possiblesInstructionsByCode[i]
        .filter((o) => knownOpCode.indexOf(o) < 0);
    }

    if (ops.length === 1) {
      const op = ops.pop() as OpCode;
      knownOpCode.push(op);
      instructionsByCode[i] = op;
    }

    i = (i + 1) % ilen;
  }

  const register = program
    .map(toOp)
    .reduce((p: Register, n: number[]) => {
      const [opCode] = n;

      instructionsByCode[opCode].instruction(...n)(p);

      return p;
    }, [0, 0, 0, 0] as Register);

  const [result] = register;

  return {
    result,
    samples,
  };
};
