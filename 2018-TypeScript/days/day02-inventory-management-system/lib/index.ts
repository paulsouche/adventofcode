interface WordMap {
  [char: string]: number;
}

export const checkSum = (input: string[]) => {
  const scan = input
    .map((word) => {
      const map: WordMap = {};
      [...word]
        .forEach((l) => map[l] = (map[l] || 0) + 1);
      return map;
    })
    .reduce((p, n) => {
      const numberOfChars = Object.values(n);

      if (numberOfChars.some((c) => c === 2)) {
        p.two++;
      }

      if (numberOfChars.some((c) => c === 3)) {
        p.three++;
      }

      return p;
    },
      {
        three: 0,
        two: 0,
      });

  return scan.three * scan.two;
};

export const commonLetters = (input: string[]) => {

  const findCorrectBoxesId = (): [string, string] => {
    let correctId2: string | undefined;
    const correctId1 = input
      .find((id1, i) => {
        correctId2 = input
          .slice(i + 1)
          .find((id) => [...id]
            .reduce((p, n, ind) => {
              if (n !== id1[ind]) {
                p++;
              }
              return p;
            }, 0) === 1);
        return !!correctId2;
      });

    if (!correctId1 || !correctId2) {
      throw new Error(`No correct boxes in input`);
    }

    return [correctId1, correctId2];
  };

  const matchingChars = ([str1, str2]: [string, string]) => {
    return [...str1]
      .filter((c, i) => str2[i] === c)
      .join('');
  };

  return matchingChars(findCorrectBoxesId());
};
