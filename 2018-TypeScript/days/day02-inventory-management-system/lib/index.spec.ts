import { checkSum, commonLetters } from '.';

describe('Inventory management system', () => {
  it('Should output the checksum', () => {
    expect(checkSum([
      'abcdef',
      'bababc',
      'abbcde',
      'abcccd',
      'aabcdd',
      'abcdee',
      'ababab',
    ])).toBe(12);
  });

  it('Should output the common letters between correct boxes ids', () => {
    expect(commonLetters([
      'abcde',
      'fghij',
      'klmno',
      'pqrst',
      'fguij',
      'axcye',
      'wvxyz',
    ])).toBe('fgij');
  });
});
