const ruleReg = /^([\.#]{5})\s=>\s([\.#])$/;

export interface Input {
  initialState: string;
  rules: string[];
}

export interface RuleMap {
  [rule: string]: string;
}

function rulesToMap(rules: string[]) {
  const map: RuleMap = {};
  return rules.reduce((p, n) => {
    const match = ruleReg.exec(n);

    if (!match) {
      throw new Error(`Invalid rule ${n}`);
    }

    p[match[1]] = match[2];

    return p;
  }, map);
}

export default ({ initialState, rules }: Input, generations = 20) => {
  const rulesMap = rulesToMap(rules);

  let generation: number;
  let state = initialState;
  let start = 0;
  let sum: number | undefined;
  let gap: number | undefined;
  let prevSum = 0;
  for (generation = 0; generation < generations; generation++) {
    const stateLen = state.length;
    let slot: number;
    let output = '';
    for (slot = 0; slot < stateLen + 2; slot++) {
      output += rulesMap[`...${state}...`.slice(slot, slot + 5)] || '.';
    }

    if (output[0] === '#') {
      start -= 1;
    } else {
      output = output.slice(1);
    }

    if (output[output.length - 1] === '.') {
      output = output.slice(0, -1);
    }

    if (output.includes(initialState)) {
      break;
    }

    sum = output
      .split('')
      .reduce((p, n, i) => {
        if (n === '#') {
          p += i + start;
        }
        return p;
      }, 0);

    if (generation === 0) {
      prevSum = sum;
      state = output;
      continue;
    }

    gap = sum - prevSum;

    if (output === `.${state}`) {
      sum += (generations - generation - 1) * gap;
      break;
    }

    prevSum = sum;
    state = output;
  }

  if (!sum) {
    throw new Error(`No generation ? ${generations}`);
  }

  return sum;
};
