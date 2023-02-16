use std::collections::BTreeSet;
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day24-planet-of-discord/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input, &200));

    Ok(())
}

fn part1(input: &str) -> u32 {
    let mut states = BTreeSet::new();
    let mut current = read_input1(input);

    while states.insert(current) != false {
        current = step1(current);
    }
    current
}

fn part2(input: &str, minutes: &i32) -> u32 {
    let mut grid = read_input2(input);

    for _ in 0..*minutes {
        grid = step2(grid);
    }
    grid.len() as u32
}

fn read_input1(input: &str) -> u32 {
    let mut result = 0;
    let mut idx = 0;

    for line in input.lines() {
        for c in line.chars() {
            if c == '#' {
                result += 1 << idx;
            }
            idx += 1;
        }
    }
    result
}

fn step1(state: u32) -> u32 {
    let mut result = 0;

    for idx in 0..25 {
        let infested = state >> idx & 1 == 1;
        let n = neighbours1(state, idx);
        match (infested, n) {
            (true, 1) | (false, 1) | (false, 2) => {
                result += 1 << idx;
            }
            _ => {}
        }
    }
    result
}

fn neighbours1(state: u32, idx: u32) -> u32 {
    let mut result = 0;
    if !(0..5).any(|i| i * 5 == idx) {
        result += state << 1 >> idx & 1;
    }

    if !(0..5).any(|i| i * 5 + 4 == idx) {
        result += state >> 1 >> idx & 1;
    }

    result += state << 5 >> idx & 1;
    result += state >> idx + 5 & 1;

    result
}

type Grid = BTreeSet<(i32, i32, i32)>;

fn read_input2(input: &str) -> Grid {
    let mut grid = Grid::new();

    for (row, line) in input.lines().enumerate() {
        for (column, c) in line.trim().chars().enumerate() {
            if c == '#' {
                grid.insert((0, row as i32, column as i32));
            }
        }
    }
    grid
}

enum Direction {
    Up,
    Down,
    Left,
    Right,
}

impl Direction {
    fn flip(&self) -> Direction {
        match self {
            Direction::Up => Direction::Down,
            Direction::Down => Direction::Up,
            Direction::Left => Direction::Right,
            Direction::Right => Direction::Left,
        }
    }

    fn move_tile(&self, row: i32, col: i32) -> (i32, i32) {
        match self {
            Direction::Left => (row, col - 1),
            Direction::Right => (row, col + 1),
            Direction::Up => (row - 1, col),
            Direction::Down => (row + 1, col),
        }
    }
}

fn count_edge(grid: &Grid, depth: i32, side: Direction) -> u32 {
    let mut result = 0;
    match side {
        Direction::Left => {
            for row in 0..5 {
                if grid.contains(&(depth, row, 0)) {
                    result += 1;
                }
            }
        }
        Direction::Right => {
            for row in 0..5 {
                if grid.contains(&(depth, row, 4)) {
                    result += 1;
                }
            }
        }
        Direction::Up => {
            for column in 0..5 {
                if grid.contains(&(depth, 0, column)) {
                    result += 1;
                }
            }
        }
        Direction::Down => {
            for column in 0..5 {
                if grid.contains(&(depth, 4, column)) {
                    result += 1;
                }
            }
        }
    }

    result
}

fn step2(grid: Grid) -> Grid {
    let mut next = Grid::new();
    let min_depth = grid.iter().next().unwrap().0 - 1;
    let max_depth = grid.iter().rev().next().unwrap().0 + 1;

    for depth in min_depth..=max_depth {
        for row in 0..5 {
            for col in 0..5 {
                if row == 2 && col == 2 {
                    continue;
                }
                let n = neighbours2(&grid, depth, row, col);
                let infested = grid.contains(&(depth, row, col));
                match (infested, n) {
                    (true, 1) | (false, 1) | (false, 2) => {
                        next.insert((depth, row, col));
                    }
                    _ => {}
                }
            }
        }
    }

    next
}

fn neighbours2(grid: &Grid, depth: i32, row: i32, column: i32) -> u32 {
    let dirs = &[
        Direction::Up,
        Direction::Down,
        Direction::Left,
        Direction::Right,
    ];
    let mut result = 0;
    for dir in dirs.iter() {
        let (adj_row, adj_col) = dir.move_tile(row, column);
        if adj_col == 2 && adj_row == 2 {
            result += count_edge(grid, depth + 1, dir.flip());
        } else {
            let tile = if adj_col < 0 || adj_col > 4 || adj_row < 0 || adj_row > 4 {
                let (outer_row, outer_col) = dir.move_tile(2, 2);
                (depth - 1, outer_row, outer_col)
            } else {
                (depth, adj_row, adj_col)
            };

            if grid.contains(&tile) {
                result += 1;
            }
        }
    }
    result
}

#[cfg(test)]
mod test {
    use super::part1;
    use super::part2;

    #[test]
    fn it_should_return_the_biodiversity_rating_for_the_first_layout_that_appears_twice() {
        assert_eq!(
            part1(&String::from(
                "....#
#..#.
#..##
..#..
#...."
            )),
            2129920
        );
    }

    #[test]
    fn it_should_return_how_many_bugs_are_present_after_ten_minutes() {
        assert_eq!(
            part2(
                &String::from(
                    "....#
#..#.
#..##
..#..
#...."
                ),
                &10
            ),
            99
        );
    }
}
