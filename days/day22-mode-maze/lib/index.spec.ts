import implem from '.';

describe('Mode maze', () => {
  it('Should output the total risk level for a given system depth and target', () => {
    expect(implem(510, '10,10').totalRisk).toBe(114);
  });

  it('Should output the total minutes to reach the target', () => {
    expect(implem(510, '10,10').minMinutes).toBe(45);
  });
});
