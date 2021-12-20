import re


class Polymer:
    def __init__(self, raw_polymer, raw_recipes):
        self.last_monomer = raw_polymer[len(raw_polymer) - 1]

        self.monomers = {}
        for i in range(len(raw_polymer) - 1):
            key = f'{raw_polymer[i]}{raw_polymer[i + 1]}'
            self.monomers[key] = 1 if not key in self.monomers else self.monomers[key] + 1

        self.recipes = {}
        for raw_recipe in raw_recipes:
            result = re.search('([A-Z]+)\s->\s([A-Z])', raw_recipe)
            self.recipes[result.group(1)] = result.group(2)

    def do_step(self):
        new_monomers = {}
        for key in self.monomers.keys():
            to_insert = self.recipes[key]
            key1 = f'{key[0]}{to_insert}'
            new_monomers[key1] = self.monomers[key] if not key1 in new_monomers else new_monomers[key1] + self.monomers[key]
            key2 = f'{to_insert}{key[1]}'
            new_monomers[key2] = self.monomers[key] if not key2 in new_monomers else new_monomers[key2] + self.monomers[key]

        self.monomers = new_monomers

    def result(self):
        monomers_by_key = {}
        monomers_by_key[self.last_monomer] = 1
        for key in self.monomers.keys():
            monomers_by_key[key[0]] = self.monomers[key] if key[0] not in monomers_by_key else monomers_by_key[key[0]] + self.monomers[key]

        return max(monomers_by_key.values()) - min(monomers_by_key.values())


def polymerization(raw_polymer, raw_recipes, steps):
    polymer = Polymer(raw_polymer, raw_recipes)
    for i in range(steps):
        polymer.do_step()

    return polymer.result()
