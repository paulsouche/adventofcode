import implem from '.';

describe('A regular map', () => {
  it('Should output the number of doors of the shortest path to the furthest room', () => {
    expect(implem('^WNE$').furthestRoomNumberOfDoors).toBe(3);
  });

  it('Should output the number of doors of the shortest path to the furthest room', () => {
    expect(implem('^ENWWW(NEEE|SSE(EE|N))$').furthestRoomNumberOfDoors).toBe(10);
  });

  it('Should output the number of doors of the shortest path to the furthest room', () => {
    expect(implem('^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$').furthestRoomNumberOfDoors).toBe(18);
  });

  it('Should output the number of doors of the shortest path to the furthest room', () => {
    expect(implem('^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$').furthestRoomNumberOfDoors).toBe(23);
  });

  it('Should output the number of doors of the shortest path to the furthest room', () => {
    expect(implem('^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$').furthestRoomNumberOfDoors)
      .toBe(31);
  });
});
