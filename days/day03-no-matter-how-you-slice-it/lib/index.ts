
const claimRegex = /#(\d+)\s@\s(\d+),(\d+):\s(\d+)x(\d+)/;

function strToClaim(claim: string) {
  const match = claimRegex.exec(claim);

  if (!match) {
    throw new Error(`Invalid claim ${claim}`);
  }

  return {
    height: +match[5],
    id: match[1],
    width: +match[4],
    x: +match[2],
    y: +match[3],
  };
}

export default (input: string[]) => {
  const fabric: {
    [key: string]: {
      ids: string[]
      claimsCount: number;
    };
  } = {};

  const claims = input
    .map(strToClaim);

  const claimIdsMap: {
    [id: string]: string;
  } = {};

  claims
    .forEach(({ id, x, y, height, width }) => {
      claimIdsMap[id] = id;
      const ilen = x + width;
      const jlen = y + height;
      for (let i = x; i < ilen; i++) {
        for (let j = y; j < jlen; j++) {
          const inch = fabric[`${i},${j}`] = fabric[`${i},${j}`] || {
            claimsCount: 0,
            ids: [],
          };
          inch.claimsCount++;
          inch.ids.push(id);

          const { ids } = inch;

          if (ids.length >= 2) {
            ids.forEach((overlapsId) => delete claimIdsMap[overlapsId]);
          }
        }
      }
    });

  const claimIds = Object.keys(claimIdsMap);
  if (claimIds.length > 1) {
    throw new Error('More than 1 claim non overlapping');
  }

  const [nonOverlapingClaimId] = claimIds;

  return {
    nonOverlapingClaimId,
    overlapingClaimsCount: Object.values(fabric).filter((v) => v.claimsCount > 1).length,
  };
};
