const instructionReg = /^Step\s(\w)\smust\sbe\sfinished\sbefore\sstep\s(\w)\scan\sbegin.$/;

interface IImplemParams {
  stepsBaseTime?: number;
  workersNum?: number;
}

interface IWorker {
  step: string;
  timeOnStep: number;
}

function instructionToStepsMap(input: string[]) {
  const map: { [step: string]: string[] } = {};
  return input
    .reduce((p, n) => {
      const match = instructionReg.exec(n);

      if (!match) {
        throw new Error(`Invalid instruction ${n}`);
      }

      const stepBefore = match[1];
      const stepAfter = match[2];

      p[stepBefore] = p[stepBefore] || [];
      p[stepAfter] = p[stepAfter] || [];

      if (p[stepAfter].includes(stepBefore)) {
        console.warn('Same instruction again ?');
      } else {
        p[stepAfter].push(stepBefore);
      }

      return p;
    }, map);
}

export default (input: string[], { workersNum, stepsBaseTime }: IImplemParams = {}) => {
  workersNum = workersNum || 1;
  stepsBaseTime = stepsBaseTime || 0;
  const stepsMap = instructionToStepsMap(input);
  const workersMap: IWorker[] = new Array(workersNum)
    .fill(0)
    .map(() => ({
      step: '',
      timeOnStep: 0,
    }));
  let steps = Object.keys(stepsMap);
  let orderedSteps: string[] = [];
  let totalTime = 0;

  while (steps.length) {
    const doableSteps = steps
      .filter((step) => stepsMap[step]
        .every((previousStep) => orderedSteps
          .includes(previousStep)))
      .sort();

    const freeWorkers = workersMap
      .filter((w) => w.timeOnStep === 0);

    const availableAssignments = Math.min(doableSteps.length, freeWorkers.length);

    let i: number;
    for (i = 0; i < availableAssignments; i++) {
      const step = doableSteps[i];
      const timeToDoStep = stepsBaseTime + step.charCodeAt(0) - 64;
      const affectedWorker = freeWorkers[i];
      affectedWorker.step = step;
      affectedWorker.timeOnStep += timeToDoStep;
      delete stepsMap[step];
    }

    let firstStepOver: string | undefined;
    let timeSpent = Infinity;

    workersMap
      .forEach(({ timeOnStep, step }) => {
        if (timeOnStep > 0 && timeOnStep < timeSpent) {
          timeSpent = timeOnStep;
          firstStepOver = step;
        }
      });

    totalTime += timeSpent;

    workersMap
      .forEach((w) => w.timeOnStep = Math.max(0, w.timeOnStep - timeSpent));

    if (firstStepOver) {
      orderedSteps = orderedSteps.concat(firstStepOver);
    }

    steps = Object.keys(stepsMap);
  }

  return {
    steps: orderedSteps.join(''),
    time: totalTime,
  };
};
