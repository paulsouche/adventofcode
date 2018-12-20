export interface Room {
  x: number;
  y: number;
  numberOfDoors: number;
}

export default (input: string) => {
  const map: {
    [coordinates: string]: Room;
  } = {};
  let currentRoom: Room = { x: 0, y: 0, numberOfDoors: 0 };
  const stack = [currentRoom];

  const set = (dx: number, dy: number) => {
    const x = currentRoom.x + dx;
    const y = currentRoom.y + dy;
    const room = map[`${x},${y}`] || { x, y, numberOfDoors: Infinity };
    room.numberOfDoors = Math.min(room.numberOfDoors, currentRoom.numberOfDoors + 1);
    currentRoom = room;
    map[`${x},${y}`] = room;
  };

  const walk = (char: string) => {
    switch (char) {
      case 'N':
        set(0, -1);
        break;
      case 'E':
        set(1, 0);
        break;
      case 'S':
        set(0, 1);
        break;
      case 'W':
        set(-1, 0);
        break;
      case '(':
        stack.push(currentRoom);
        break;
      case ')':
        const room = stack.pop();
        if (!room) {
          throw new Error(`Invalid regex input`);
        }
        currentRoom = room;
        break;
      case '|':
        currentRoom = stack[stack.length - 1];
        break;
      default:
        break;
    }
  };

  input.split('').forEach(walk);

  let furthestRoomNumberOfDoors = 0;
  let roomsTo1000Doors = 0;
  Object
    .values(map)
    .forEach(({ numberOfDoors }) => {
      if (numberOfDoors >= 1000) {
        roomsTo1000Doors++;
      }
      if (furthestRoomNumberOfDoors < numberOfDoors) {
        furthestRoomNumberOfDoors = numberOfDoors;
      }
    });

  return {
    furthestRoomNumberOfDoors,
    roomsTo1000Doors,
  };
};
