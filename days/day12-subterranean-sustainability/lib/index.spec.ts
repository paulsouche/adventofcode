import implem from '.';

describe('Subterranean sustainability', () => {
  it('Should sum the number of all pots which contain a plant after 20 generations', () => {
    expect(implem({
      initialState: '#..#.#..##......###...###',
      rules: [
        '...## => #',
        '..#.. => #',
        '.#... => #',
        '.#.#. => #',
        '.#.## => #',
        '.##.. => #',
        '.#### => #',
        '#.#.# => #',
        '#.### => #',
        '##.#. => #',
        '##.## => #',
        '###.. => #',
        '###.# => #',
        '####. => #',
      ],
    })).toBe(325);
  });

  it('Should sum the number of all pots which contain a plant after 50000000000 generations', () => {
    expect(implem({
      initialState: '#..#.#..##......###...###',
      rules: [
        '...## => #',
        '..#.. => #',
        '.#... => #',
        '.#.#. => #',
        '.#.## => #',
        '.##.. => #',
        '.#### => #',
        '#.#.# => #',
        '#.### => #',
        '##.#. => #',
        '##.## => #',
        '###.. => #',
        '###.# => #',
        '####. => #',
      ],
    }, 50000000000)).toBe(999999999374);
  });
});
