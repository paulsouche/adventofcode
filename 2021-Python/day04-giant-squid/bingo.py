class Board:
    def __init__(self, board):
        self.board = board
        self.hdrawn = []
        for line in board:
            self.hdrawn.append([])
        self.vdrawn = []
        for column in board[0]:
            self.vdrawn.append([])

    def draw(self, number):
        for i,line in enumerate(self.board):
            for j,col in enumerate(line):
                if self.board[i][j] == number:
                    self.hdrawn[i].append(number)
                    self.vdrawn[j].append(number)

    def bingo(self):
        bingo = False
        for line in self.hdrawn:
            if len(line) == len(self.board[0]):
                bingo = True
                break
        for column in self.vdrawn:
            if len(column) == len(self.board):
                bingo = True
                break
        return bingo

    def result(self):
        sum = 0
        for i,line in enumerate(self.board):
            for number in line:
                if number not in self.hdrawn[i]:
                    sum += number
        return sum

def part1(draws, raw_boards):
    boards = list(map(lambda b: Board(b), raw_boards))
    for number in draws:
        for board in boards:
            board.draw(number)
            if board.bingo():
                return number * board.result()

def part2(draws, raw_boards):
    boards = list(map(lambda b: Board(b), raw_boards))
    remaining_boards = list(boards)
    for number in draws:
        boards = list(remaining_boards)
        for board in boards:
            board.draw(number)
            if board.bingo():
                if len(remaining_boards) > 1:
                   remaining_boards.remove(board)
                else:
                    return number * board.result()
