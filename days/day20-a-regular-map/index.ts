import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { furthestRoomNumberOfDoors, roomsTo1000Doors } = implem(input.regex);
console.info(colors
  .yellow('Day 20 A regular map part 1 result :'), colors
    .green(furthestRoomNumberOfDoors.toString()));

console.info(colors
  .yellow('Day 20 A regular map part 2 result :'), colors
    .green(roomsTo1000Doors.toString()));
