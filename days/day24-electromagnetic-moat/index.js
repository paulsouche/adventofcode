// DAY 24: Electromagnetic Moat
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day24Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day24TestInput = [
  "0/2",
  "2/2",
  "2/3",
  "3/4",
  "3/5",
  "0/1",
  "10/1",
  "9/10",
];

class Component {
  constructor(inventory) {
    [this.pinA, this.pinB] = inventory.split("/");
  }
}

const remainings = (component, components) => {
  components = [...components];
  components.splice(components.indexOf(component), 1);
  return components;
};

const findBridges = (components) => {
  const bridges = [];
  const rootNode = {
    out: "0",
    strength: 0,
    length: 0,
  };

  const loadBridges = (node, possibleComponents) => {
    possibleComponents
      .filter(({ pinA, pinB }) => pinA === node.out || pinB === node.out)
      .forEach((component) => {
        const { pinA, pinB } = component;
        const out = pinA === node.out ? pinB : pinA;
        const strength = node.strength + +pinA + +pinB;
        const length = node.length + 1;
        loadBridges(
          { out, strength, length },
          remainings(component, possibleComponents)
        );

        bridges.push({ strength, length });
      });
  };

  loadBridges(rootNode, components);

  return bridges;
};

export const strongestBridge = (input) => {
  const bridges = findBridges(
    input.map((inventory) => new Component(inventory))
  );

  const [{ strength: maxStrength }] = bridges.sort(
    (a, b) => b.strength - a.strength
  );

  return maxStrength;
};

export const bestBridge = (input) => {
  const bridges = findBridges(
    input.map((inventory) => new Component(inventory))
  );

  const [{ strength: maxStrength }] = bridges.sort((a, b) => {
    if (b.length - a.length === 0) {
      return b.strength - a.strength;
    }
    return b.length - a.length;
  });

  return maxStrength;
};
