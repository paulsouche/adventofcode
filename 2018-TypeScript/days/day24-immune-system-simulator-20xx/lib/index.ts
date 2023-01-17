interface Input {
  [team: string]: string[];
}

interface ParsedGroup {
  attack: number;
  damage: string;
  hitPoints: number;
  initiative: number;
  units: number;
}

interface Group extends ParsedGroup {
  claimed: boolean;
  immuneTo: string[];
  weakTo: string[];
  enemy?: Group[];
  target?: Group;
}

enum WINNERS {
  None,
  ImmuneSystem,
  Infection,
}

const GROUP_REGEX =
  // tslint:disable-next-line:max-line-length
  /^(\d+)\sunits\seach\swith\s(\d+)\shit\spoints\s*(\([^\)]*\)|\s)\s*with\san\sattack\sthat\sdoes\s(\d+)\s(\w+)\sdamage\sat\sinitiative\s(\d+)$/;
const IMMUNE_TO_REGEX = /immune\sto\s([\w\s,]+)/;
const WEAK_TO_REGEX = /weak\sto\s([\w\s,]+)/;

const createGroup = (parsedGroup: ParsedGroup, weakImmune: string): Group => {
  const [immuneTo, weakTo] = [IMMUNE_TO_REGEX, WEAK_TO_REGEX].map((regex) => {
    const m = regex.exec(weakImmune);
    return !!m ? m[1].split(`, `) : [];
  });

  return {
    ...parsedGroup,
    claimed: false,
    immuneTo,
    weakTo,
  };
};

const parse = (input: Input) =>
  Object.values(input).map((groups) =>
    groups.map((group) => {
      const m = GROUP_REGEX.exec(group);
      if (!m) {
        throw new Error(`Invalid input ${group}`);
      }

      const parsedGroup = {
        attack: +m[4],
        damage: m[5],
        hitPoints: +m[2],
        initiative: +m[6],
        units: +m[1],
      };

      return createGroup(parsedGroup, m[3]);
    }));

const attackDamage = (attacker: Group, target: Group) =>
  attacker.attack *
  attacker.units *
  (target.weakTo.includes(attacker.damage) ? 2 : 1);

const isAlive = (g: Group) => g.units > 0;

const clear = (group: Group, enemy: Group[]) => {
  group.claimed = false;
  group.enemy = enemy;
};

const initiativeSort = (a: Group, b: Group) => b.initiative - a.initiative;

const powerSort = (a: Group, b: Group) => {
  const aPower = a.attack * a.units;
  const bPower = b.attack * b.units;

  return bPower - aPower !== 0 ? bPower - aPower : initiativeSort(a, b);
};

const attackSort = (attacker: Group) => (a: Group, b: Group) => {
  const aAttackDamage = attackDamage(attacker, a);
  const bAttackDamage = attackDamage(attacker, b);

  return bAttackDamage - aAttackDamage !== 0
    ? bAttackDamage - aAttackDamage
    : powerSort(a, b);
};

const setTarget = (attacker: Group) => {
  const [target] = attacker
    .enemy!.filter((e) => !e.immuneTo.includes(attacker.damage) && !e.claimed)
    .sort(attackSort(attacker));

  if (!!target) {
    target.claimed = true;
  }
  attacker.target = target;

  return attacker;
};

const targetSelectionSorter = (groups: Group[]) => groups.sort(powerSort);

const attackSelectionSorter = (groups: Group[]) => groups.sort(initiativeSort);

const attackEnemy = (unitsKilled: number, attacker: Group) => {
  const target = attacker.target;
  if (attacker.units <= 0 || !target) {
    return unitsKilled;
  }
  const unitsDestroyed = Math.floor(attackDamage(attacker, target) / target.hitPoints);
  const killed = Math.min(unitsDestroyed, target.units);
  target.units = target.units - killed;
  return unitsKilled + killed;
};

const battle = (input: Input, boost = 0) => {
  const [immuneSystemGroups, infectionGroups] = parse(input);
  immuneSystemGroups.forEach((g) => (g.attack = g.attack + boost));

  let aliveImmuneSystemGroups = immuneSystemGroups.filter(isAlive);
  let aliveInfectionGroups = infectionGroups.filter(isAlive);

  while (
    aliveImmuneSystemGroups.length > 0 &&
    aliveInfectionGroups.length > 0
  ) {
    aliveImmuneSystemGroups.forEach((g) => clear(g, aliveInfectionGroups));
    infectionGroups.forEach((g) => clear(g, aliveImmuneSystemGroups));

    const everyone = targetSelectionSorter([
      ...aliveImmuneSystemGroups,
      ...aliveInfectionGroups,
    ]).map(setTarget);

    const killed = attackSelectionSorter(everyone).reduce(attackEnemy, 0);
    if (killed === 0) {
      return [WINNERS.None, 0];
    }

    aliveImmuneSystemGroups = aliveImmuneSystemGroups.filter(isAlive);
    aliveInfectionGroups = aliveInfectionGroups.filter(isAlive);
  }

  return aliveImmuneSystemGroups.length > 0
    ? [
        WINNERS.ImmuneSystem,
        aliveImmuneSystemGroups.reduce((acc, g) => g.units + acc, 0),
      ]
    : [
        WINNERS.Infection,
        aliveInfectionGroups.reduce((acc, g) => g.units + acc, 0),
      ];
};

const findBoostToWin = (input: Input) => {
  let boost = 1;
  while (true) {
    const [winner, remainingUnits] = battle(input, boost);
    if (winner === WINNERS.ImmuneSystem) {
      return remainingUnits;
    }
    boost++;
  }
};

export default (input: Input) => {
  const [, remainingUnitsWithNoBoost] = battle(input);

  return {
    remainingUnitsWithLowerBoost: findBoostToWin(input),
    remainingUnitsWithNoBoost,
  };
};
