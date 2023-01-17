import implem from '.';

describe('Memory maneuver', () => {
  it('Should output the metadatas sum', () => {
    expect(implem('2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2').sum).toBe(138);
  });

  it('Should output the root node value', () => {
    expect(implem('2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2').value).toBe(66);
  });
});
