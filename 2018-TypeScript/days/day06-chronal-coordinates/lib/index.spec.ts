import implem from '.';

describe('Chronal coordinates', () => {
  it('Should output the largest area', () => {
    expect(implem([
      '1, 1',
      '1, 6',
      '8, 3',
      '3, 4',
      '5, 5',
      '8, 9',
    ]).largestArea).toBe(17);
  });

  it('Should output the safest area', () => {
    expect(implem([
      '1, 1',
      '1, 6',
      '8, 3',
      '3, 4',
      '5, 5',
      '8, 9',
    ], 32).safestArea).toBe(16);
  });
});
