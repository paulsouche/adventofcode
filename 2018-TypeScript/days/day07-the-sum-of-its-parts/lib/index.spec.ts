import implem from '.';

describe('The sum of its parts', () => {
  it('Should output the steps in order', () => {
    expect(implem([
      'Step C must be finished before step A can begin.',
      'Step C must be finished before step F can begin.',
      'Step A must be finished before step B can begin.',
      'Step A must be finished before step D can begin.',
      'Step B must be finished before step E can begin.',
      'Step D must be finished before step E can begin.',
      'Step F must be finished before step E can begin.',
    ]).steps).toBe('CABDFE');
  });

  it('Should output the time spent on steps', () => {
    expect(implem([
      'Step C must be finished before step A can begin.',
      'Step C must be finished before step F can begin.',
      'Step A must be finished before step B can begin.',
      'Step A must be finished before step D can begin.',
      'Step B must be finished before step E can begin.',
      'Step D must be finished before step E can begin.',
      'Step F must be finished before step E can begin.',
    ], { workersNum: 2 }).time).toBe(15);
  });
});
