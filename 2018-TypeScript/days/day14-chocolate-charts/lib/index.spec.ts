import { nextTenRecipes, occurrenceOfInput } from '.';

describe('Chocolate charts', () => {
  it('Should output the next 10 recipes after input', () => {
    expect(nextTenRecipes(5)).toBe('0124515891');
    expect(nextTenRecipes(9)).toBe('5158916779');
    expect(nextTenRecipes(18)).toBe('9251071085');
    expect(nextTenRecipes(2018)).toBe('5941429882');
  });

  it('Should detect input first occurrence', () => {
    expect(occurrenceOfInput('01245')).toBe(5);
    expect(occurrenceOfInput('51589')).toBe(9);
    expect(occurrenceOfInput('92510')).toBe(18);
    expect(occurrenceOfInput('59414')).toBe(2018);
  });
});
