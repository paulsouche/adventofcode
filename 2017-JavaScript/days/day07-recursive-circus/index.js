// DAY 7: Recursive Circus
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day7Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day7TestInput = [
  "pbga (66)",
  "xhth (57)",
  "ebii (61)",
  "havc (66)",
  "ktlj (57)",
  "fwft (72) -> ktlj, cntj, xhth",
  "qoyq (66)",
  "padx (45) -> pbga, havc, qoyq",
  "tknk (41) -> ugml, padx, fwft",
  "jptl (61)",
  "ugml (68) -> gyxo, ebii, jptl",
  "gyxo (61)",
  "cntj (57)",
];

export function rootProgram(input) {
  const programs = {};
  const childNames = new Set();

  for (const line of input) {
    const [program, children = ""] = line.split(" -> ");
    const [, name] = program.match(/(\w+).*\((\d+)/);
    children.split(", ").forEach((c) => childNames.add(c));
    programs[name] = true;
  }

  return Object.keys(programs).find((name) => !childNames.has(name));
}

function buildTree(input) {
  const programs = {};
  const childNames = new Set();

  for (const line of input) {
    const [programm, children = ""] = line.split(" -> ");
    const [_, name, weightString] = programm.match(/(\w+).*\((\d+)/);
    children.split(", ").forEach((c) => childNames.add(c));
    let weight = +weightString;
    programs[name] = {
      name,
      weight,
      getChildren: () =>
        children ? children.split(", ").map((x) => programs[x.trim()]) : [],
    };
  }

  function loadChildren(node) {
    node.children = node.getChildren();
    node.children.forEach(loadChildren);
  }

  function loadWeights(node) {
    if (!node.children.length) {
      node.totalWeight = node.weight;
      return node;
    }

    node.totalWeight =
      node.weight +
      node.children.reduce((acc, c) => acc + loadWeights(c).totalWeight, 0);

    return node;
  }

  const rootName = Object.keys(programs).find((name) => !childNames.has(name));
  const root = programs[rootName];

  loadChildren(root);
  loadWeights(root);

  return root;
}

function findImbalanced(node) {
  const children = node.children;
  const weightMap = new Map();
  children.forEach(({ totalWeight }) =>
    weightMap.set(totalWeight, (weightMap.get(totalWeight) || 0) + 1)
  );

  if (weightMap.size === 1) return null;

  const imbalanced = children.find((c) => weightMap.get(c.totalWeight) === 1);

  const faulty = findImbalanced(imbalanced);

  if (faulty === null) {
    const correctWeights = children.find(
      (c) => weightMap.get(c.totalWeight) > 1
    ).totalWeight;
    const correctWeight =
      correctWeights - imbalanced.totalWeight + imbalanced.weight;
    return [imbalanced, correctWeight];
  }

  return faulty;
}

export function weight(input) {
  const tree = buildTree(input);
  const [_, correctWeight] = findImbalanced(tree);
  return correctWeight;
}
