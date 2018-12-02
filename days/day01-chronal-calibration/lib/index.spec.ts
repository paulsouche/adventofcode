import implem from '.';

describe('Chronal calibration', () => {
  it('Should output the reduced array sum', () => {
    expect(implem([1, 1, 1]).resultingFrequency).toBe(3);
    expect(implem([1, 1, -2]).resultingFrequency).toBe(0);
    expect(implem([-1, -2, -3]).resultingFrequency).toBe(-6);
  });

  it('Should output the first frequency reached twice', () => {
    expect(implem([1, -1]).firstFrequencyReachedTwice).toBe(0);
    expect(implem([3, 3, 4, -2, -4]).firstFrequencyReachedTwice).toBe(10);
    expect(implem([-6, 3, 8, 5, -6]).firstFrequencyReachedTwice).toBe(5);
    expect(implem([7, 7, -2, -7, -4]).firstFrequencyReachedTwice).toBe(14);
  });
});
