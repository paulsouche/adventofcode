import implem from '.';

describe('Subterranean sustainability', () => {
  it('Should detect first crash coordinates', () => {
    expect(implem([
      '/->-\\        ',
      '|   |  /----\\',
      '| /-+--+-\\  | ',
      '| | |  | v  | ',
      '\\-+-/  \\-+--/ ',
      '  \\------/    ',
    ]).firstCrash).toBe('7,3');
  });

  it('Should detect last cart', () => {
    expect(implem([
      '/>-<\\  ',
      '|   |  ',
      '| /<+-\\',
      '| | | v',
      '\\>+</ |',
      '  |   ^',
      '  \\<->/',
    ]).lastCart).toBe('6,4');
  });
});
