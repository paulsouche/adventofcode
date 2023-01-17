const recordRegex =
  /^\[(\d{4})-(\d{2})-(\d{2})\s(\d{2}):(\d{2})\]\s?(wakes\sup|falls\sasleep|Guard\s#(\d+)\sbegins\sshift)$/;

function parseRecord(record: string) {
  const match = recordRegex.exec(record);

  if (!match) {
    throw new Error(`Invalid record ${record}`);
  }

  return {
    day: `${match[2]}-${match[3]}`,
    guardId: match[6].includes('Guard') ? match[7] : undefined,
    minute: +match[5],
  };
}

function reduceGardTimes(times: number[]) {
  const ilen = times.length;
  let total = 0;
  for (let i = 0; i < ilen; i += 2) {
    total += times[i + 1] - times[i];
  }
  return total;
}

function findMostAsleepMinutes(guardData: { [day: string]: number[] }) {
  const minutesCount = 60;
  const minutesAsleep: number[] = new Array(minutesCount).fill(0);

  Object.keys(guardData)
    .forEach((day) => {
      const ilen = minutesCount;
      const minutes = guardData[day];
      let minute = 0;
      let isAsleep = false;
      let i: number;
      for (i = 0; i <= ilen; i++) {
        if (i >= minutes[minute]) {
          minute++;
          isAsleep = !isAsleep;
        }
        if (isAsleep) {
          minutesAsleep[i]++;
        }
      }
    });

  let timeMinuteAsleep = 0;
  let mostMinuteAsleep = 0;
  minutesAsleep
    .forEach((minuteAsleep, i) => {
      if (timeMinuteAsleep < minuteAsleep) {
        timeMinuteAsleep = minuteAsleep;
        mostMinuteAsleep = i;
      }
    });

  return {
    mostMinuteAsleep,
    timeMinuteAsleep,
  };
}

function findMaxAsleepGuardId(timeAsleepMap: { [guardId: string]: number }) {
  let maxTimeAsleep = 0;
  let maxAsleepGuardId: string | undefined;

  Object.keys(timeAsleepMap)
    .forEach((guardId) => {
      const timeAsleep = timeAsleepMap[guardId];
      if (timeAsleep > maxTimeAsleep) {
        maxAsleepGuardId = guardId;
        maxTimeAsleep = timeAsleep;
      }
    });

  if (!maxAsleepGuardId) {
    throw new Error('Cannot find strategy 1 max asleep guard');
  }

  return maxAsleepGuardId;
}

function findStrategy1(timeAsleepMap: { [guardId: string]: number }, minuteAsleepMap: {
  [guardId: string]: {
    mostMinuteAsleep: number;
    timeMinuteAsleep: number;
  };
}) {
  const stgGuardId = findMaxAsleepGuardId(timeAsleepMap);
  const stgMostMinuteAsleep = minuteAsleepMap[stgGuardId].mostMinuteAsleep;

  return {
    stgGuardId,
    stgMostMinuteAsleep,
  };
}

function findStrategy2(minuteAsleepMap: {
  [guardId: string]: {
    mostMinuteAsleep: number;
    timeMinuteAsleep: number;
  };
}) {
  let stgGuardId: string | undefined;
  let stgMostMinuteAsleep: number | undefined;
  let strategy2TimeAsleep = 0;
  Object
    .keys(minuteAsleepMap)
    .forEach((guardId: string) => {
      const { mostMinuteAsleep, timeMinuteAsleep } = minuteAsleepMap[guardId];
      if (timeMinuteAsleep > strategy2TimeAsleep) {
        strategy2TimeAsleep = timeMinuteAsleep;
        stgGuardId = guardId;
        stgMostMinuteAsleep = mostMinuteAsleep;
      }
    });

  if (!stgGuardId) {
    throw new Error('Cannot find strategy 2 max asleep guard');
  }

  if (!stgMostMinuteAsleep) {
    throw new Error('Cannot find strategy 2 most minute asleep');
  }

  return {
    stgGuardId,
    stgMostMinuteAsleep,
  };
}

export default (input: string[]) => {
  const guardsMap: {
    [guardId: string]: {
      [day: string]: number[];
    };
  } = {};

  let currentGuardId: string;

  input
    .sort((a, b) => a.substr(1, 16) > b.substr(1, 16) ? 1 : -1)
    .forEach((r) => {
      const { day, guardId, minute } = parseRecord(r);
      if (guardId) {
        currentGuardId = guardId;
        guardsMap[guardId] = guardsMap[guardId] || {};
      } else {
        const minutes = guardsMap[guardId || currentGuardId][day] = guardsMap[guardId || currentGuardId][day] || [];
        minutes.push(minute);
      }
    });

  const timeAsleepMap: {
    [guardId: string]: number;
  } = {};

  Object
    .keys(guardsMap)
    .forEach((guardId) => Object
      .keys(guardsMap[guardId])
      .forEach((date) =>
        timeAsleepMap[guardId] = (timeAsleepMap[guardId] || 0) + reduceGardTimes(guardsMap[guardId][date])));

  const minuteAsleepMap: {
    [guardId: string]: {
      mostMinuteAsleep: number,
      timeMinuteAsleep: number,
    };
  } = {};

  Object
    .keys(guardsMap)
    .forEach((guardId) => minuteAsleepMap[guardId] = findMostAsleepMinutes(guardsMap[guardId]));

  const strategy1 = findStrategy1(timeAsleepMap, minuteAsleepMap);
  const strategy2 = findStrategy2(minuteAsleepMap);

  return {
    strategy1: +strategy1.stgGuardId * strategy1.stgMostMinuteAsleep,
    strategy2: +strategy2.stgGuardId * strategy2.stgMostMinuteAsleep,
  };
};
