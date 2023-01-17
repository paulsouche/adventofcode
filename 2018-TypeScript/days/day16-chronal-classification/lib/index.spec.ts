import { addi, exampleToInstructions, mulr, seti } from '.';

describe('Chronal classification', () => {
  it('Should filter CPU\'s instructions', () => {
    expect(exampleToInstructions([
      'Before: [3, 2, 1, 1]',
      '9 2 1 2',
      'After:  [3, 2, 2, 1]',
    ])).toEqual({
      code: 9,
      possibleInstructions: [
        addi,
        mulr,
        seti,
      ],
    });
  });
});
