import implem from '.';

describe('Reservoir research', () => {
  it('Should output the number of flooded cells', () => {
    expect(implem([
      'x=495, y=2..7',
      'y=7, x=495..501',
      'x=501, y=3..7',
      'x=498, y=2..4',
      'x=506, y=1..2',
      'x=498, y=10..13',
      'x=504, y=10..13',
      'y=13, x=498..504',
    ]).floodedCells).toBe(57);
  });

  it('Should output the number of retained cells', () => {
    expect(implem([
      'x=495, y=2..7',
      'y=7, x=495..501',
      'x=501, y=3..7',
      'x=498, y=2..4',
      'x=506, y=1..2',
      'x=498, y=10..13',
      'x=504, y=10..13',
      'y=13, x=498..504',
    ]).retainedCells).toBe(29);
  });
});
