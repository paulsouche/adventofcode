import implem from '.';

describe('Four dimensional adventure', () => {
  it('Should output the number of distinct constellations', () => {
    expect(implem([
      '0,0,0,0',
      '3,0,0,0',
      '0,3,0,0',
      '0,0,3,0',
      '0,0,0,3',
      '0,0,0,6',
      '9,0,0,0',
      '12,0,0,0',
    ])).toBe(2);
  });

  it('Should output the number of distinct constellations', () => {
    expect(implem([
      '-1,2,2,0',
      '0,0,2,-2',
      '0,0,0,-2',
      '-1,2,0,0',
      '-2,-2,-2,2',
      '3,0,2,-1',
      '-1,3,2,2',
      '-1,0,-1,0',
      '0,2,1,-2',
      '3,0,0,0',
    ])).toBe(4);
  });

  it('Should output the number of distinct constellations', () => {
    expect(implem([
      '1,-1,0,1',
      '2,0,-1,0',
      '3,2,-1,0',
      '0,0,3,1',
      '0,0,-1,-1',
      '2,3,-2,0',
      '-2,2,0,0',
      '2,-2,0,-1',
      '1,-1,0,-1',
      '3,2,0,2',
    ])).toBe(3);
  });

  it('Should output the number of distinct constellations', () => {
    expect(implem([
      '1,-1,-1,-2',
      '-2,-2,0,1',
      '0,2,1,3',
      '-2,3,-2,1',
      '0,2,3,-2',
      '-1,-1,1,-2',
      '0,-2,-1,0',
      '-2,2,3,-1',
      '1,2,2,0',
      '-1,-2,0,-2',
    ])).toBe(8);
  });
});
