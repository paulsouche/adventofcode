import implem from '.';

describe('Marble mania', () => {
  it('Should output the max score for players and marbles input', () => {
    expect(implem('9 players; last marble is worth 25 points')).toBe(32);
    expect(implem('10 players; last marble is worth 1618 points')).toBe(8317);
    expect(implem('13 players; last marble is worth 7999 points')).toBe(146373);
    expect(implem('17 players; last marble is worth 1104 points')).toBe(2764);
    expect(implem('21 players; last marble is worth 6111 points')).toBe(54718);
    expect(implem('30 players; last marble is worth 5807 points')).toBe(37305);
  });
});
