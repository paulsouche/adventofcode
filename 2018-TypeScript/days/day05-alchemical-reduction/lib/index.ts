export const units = (input: string) => {
  for (let i = 0; i < input.length - 1; i++) {
    const asciiPairDiff = Math.abs(input.charCodeAt(i) - input.charCodeAt(i + 1));
    if (asciiPairDiff === 32) {
      input = `${input.slice(0, i)}${input.slice(i + 2)}`;
      i -= 2;
    }
  }

  return input.length;
};

export const shortestsUnits = (input: string) => {
  const monomerMap: {
    [char: string]: RegExp;
  } = {};

  let polymer = input;
  while (polymer.length) {
    const char = polymer.charAt(0);
    const reg = new RegExp(char, 'ig');
    monomerMap[char] = reg;
    polymer = polymer.replace(reg, '');
  }

  let shortestUnit = Infinity;

  Object.values(monomerMap)
    .map((reg) => units(input.replace(reg, '')))
    .forEach((monomers) => {
      if (monomers < shortestUnit) {
        shortestUnit = monomers;
      }
    });

  return shortestUnit;
};
