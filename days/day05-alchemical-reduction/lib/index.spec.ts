import { shortestsUnits, units } from '.';

describe('Alchemical reduction', () => {
  it('Should output the resulting polymer', () => {
    expect(units('dabAcCaCBAcCcaDA')).toBe(10);
  });

  it('Should find the shortest resulting polymer', () => {
    expect(shortestsUnits('dabAcCaCBAcCcaDA')).toBe(4);
  });
});
