import implem, { toCell } from '.';

describe('Chronal charge', () => {
  it('Should know how to calculate power', () => {
    expect(toCell(8)(null, 1202).powerLevel).toBe(4);
    expect(toCell(57)(null, 23521).powerLevel).toBe(-5);
    expect(toCell(39)(null, 58716).powerLevel).toBe(0);
    expect(toCell(71)(null, 45700).powerLevel).toBe(4);
  });

  it('Should output the coordinates of the best 3x3 power square', () => {
    expect(implem(18).largestSquare3x3).toBe('33,45');
    expect(implem(42).largestSquare3x3).toBe('21,61');
  });

  it('Should output the coordinates of the best NxN power square', () => {
    expect(implem(18).largestSquareNxN).toBe('90,269,16');
    expect(implem(42).largestSquareNxN).toBe('232,251,12');
  });
});
