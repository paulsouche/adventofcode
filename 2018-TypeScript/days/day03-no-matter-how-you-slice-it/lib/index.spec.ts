import implem from '.';

describe('No matter how you slice it', () => {
  it('Should output the overlapped square inches', () => {
    expect(implem([
      '#1 @ 1,3: 4x4',
      '#2 @ 3,1: 4x4',
      '#3 @ 5,5: 2x2',
    ]).overlapingClaimsCount).toBe(4);
  });

  it('Should output non overlapping claim id', () => {
    expect(implem([
      '#1 @ 1,3: 4x4',
      '#2 @ 3,1: 4x4',
      '#3 @ 5,5: 2x2',
    ]).nonOverlapingClaimId).toBe('3');
  });
});
