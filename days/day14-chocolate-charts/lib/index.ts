interface RecipesParams {
  elve1: number;
  elve2: number;
  recipes: number[];
  chain?: string;
}

function createRecipes({ elve1, elve2, recipes, chain }: RecipesParams) {
  const elve1Recipe = recipes[elve1];
  const elve2Recipe = recipes[elve2];
  const addedRecipes = (elve1Recipe + elve2Recipe).toString().split('').map((r) => +r);
  recipes.push(...addedRecipes);
  elve1 = (elve1 + elve1Recipe + 1) % recipes.length;
  elve2 = (elve2 + elve2Recipe + 1) % recipes.length;
  return {
    chain: `${chain || ''}${addedRecipes.join('')}`,
    elve1,
    elve2,
  };
}

export const nextTenRecipes = (input: number) => {
  const recipes = [3, 7];
  let elve1 = 0;
  let elve2 = 1;

  while (recipes.length < (+input + 10)) {
    ({ elve1, elve2 } = createRecipes({ elve1, elve2, recipes }));
  }

  return recipes.slice(+input, +input + 10).join('');
};

export const occurrenceOfInput = (input: string) => {
  const recipes = [3, 7];
  const inputLen = input.length;
  let elve1 = 0;
  let elve2 = 1;
  let firstOccurrence: number | undefined;
  let chain = '37';
  let index = 0;

  while (typeof firstOccurrence === 'undefined') {
    ({ elve1, elve2, chain } = createRecipes({ elve1, elve2, recipes, chain }));

    while (chain.length >= inputLen) {
      if (chain.indexOf(input) === 0) {
        firstOccurrence = index;
        break;
      }
      chain = chain.slice(1);
      index++;
    }
  }

  return firstOccurrence;
};
