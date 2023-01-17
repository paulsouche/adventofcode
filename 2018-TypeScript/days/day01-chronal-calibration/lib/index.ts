export default (input: number[]) => {
  let firstFrequencyReachedTwice: number | undefined;
  const frequenciesReached = [0];

  const reduceInput = (a: number[], start = 0) => a.reduce((p, n) => {
    const newFrequency = p + n;
    if (typeof firstFrequencyReachedTwice === 'undefined') {
      if (frequenciesReached.includes(newFrequency)) {
        firstFrequencyReachedTwice = newFrequency;
      } else {
        frequenciesReached.push(newFrequency);
      }
    }
    return newFrequency;
  }, start);

  const resultingFrequency = reduceInput(input);

  let sum = resultingFrequency;
  while (typeof firstFrequencyReachedTwice === 'undefined') {
    sum = reduceInput(input, sum);

    // This is ugly as divergence condition but it's here for tests
    if (frequenciesReached.every((item, i) => Math.abs(item) >= Math.abs(frequenciesReached[i - 1] || 0))) {
      firstFrequencyReachedTwice = Infinity * frequenciesReached[1];
    }
  }

  return {
    firstFrequencyReachedTwice,
    resultingFrequency,
  };
};
