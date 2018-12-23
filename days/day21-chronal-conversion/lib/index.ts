import { parseIp, parseProg } from '../../day19-go-with-the-flow/lib';

export default (input: string[]) => {
  const ip = parseIp(input.slice(0).shift());
  const programMap = input.slice(1).map(parseProg);
  const register = [0, 0, 0, 0, 0, 0];
  let i = register[ip];
  let instruction = programMap[i];
  let fewestInstructionRegisterValue = 0;
  let mostInstructionRegisterValue = 0;
  const values: {
    [value: number]: boolean | undefined;
  } = {};

  while (instruction) {
    register[ip] = i;
    instruction(register);
    i = register[ip];
    i++;
    instruction = programMap[i];

    if (i === 28) {
      // This is the program specific part
      // when instruction 28 is reached
      // program breaks if register[0] === register[3]
      const valueToBreak = register[3];
      fewestInstructionRegisterValue = fewestInstructionRegisterValue || valueToBreak;
      if (values[valueToBreak]) {
        break;
      }
      mostInstructionRegisterValue = valueToBreak;
      values[valueToBreak] = true;
    }
  }

  return {
    fewestInstructionRegisterValue,
    mostInstructionRegisterValue,
  };
};
