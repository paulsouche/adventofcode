const partyReg = /^(\d+)\splayers;\slast\smarble\sis\sworth\s(\d+)\spoints$/;

function strToParty(input: string, coeff: number) {
  const match = partyReg.exec(input);

  if (!match) {
    throw new Error(`Invalid party ${input}`);
  }

  return {
    marbles: +match[2] * coeff,
    players: +match[1],
  };
}

class Marble {
  constructor(public value: number, private _next?: Marble, private _prev?: Marble) { }

  get next() {
    return this._next || this;
  }

  set next(marble: Marble) {
    this._next = marble;
  }

  get prev() {
    return this._prev || this;
  }

  set prev(marble: Marble) {
    this._prev = marble;
  }
}

export default (input: string, coeff = 1) => {
  const { marbles, players } = strToParty(input, coeff);
  const scores = new Array(players).fill(0);

  let current = new Marble(0);

  let marble: number;
  let player = 0;
  for (marble = 1; marble <= marbles; marble++) {
    if (marble % 23 === 0) {
      scores[player] += marble;
      current = current.prev.prev.prev.prev.prev.prev;
      scores[player] += current.prev.value;
      current.prev.prev.next = current;
      current.prev = current.prev.prev;
    } else {
      const next = new Marble(marble, current.next.next, current.next);
      current.next.next.prev = next;
      current.next.next = next;
      current = next;
    }

    player = ++player % players;
  }

  return Math.max(...scores);
};
