use std::cmp::Ordering;
use std::collections::{BTreeSet, BinaryHeap, HashMap, HashSet, VecDeque};
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day18-many-worlds-interpretation/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input));

    Ok(())
}

fn part1(input: &str) -> usize {
    let grid = parse_grid(&input);
    let graph = graph(&grid);

    search(graph, '@')
}

fn part2(input: &str) -> usize {
    let mut grid = parse_grid(&input);
    four_robots(&mut grid);
    let graph = graph(&grid);

    search_four(graph)
}

#[derive(Clone, Copy, PartialEq, Eq, Hash)]
struct Coordinate(i32, i32);

impl Coordinate {
    fn neighbours(&self) -> [Coordinate; 4] {
        [
            Coordinate(self.0 - 1, self.1),
            Coordinate(self.0 + 1, self.1),
            Coordinate(self.0, self.1 - 1),
            Coordinate(self.0, self.1 + 1),
        ]
    }
}

#[derive(PartialEq, Eq)]
struct DijkstraState {
    cost: usize,
    current: char,
}

impl Ord for DijkstraState {
    fn cmp(&self, other: &Self) -> Ordering {
        other
            .cost
            .cmp(&self.cost)
            .then_with(|| self.current.cmp(&other.current))
    }
}

impl PartialOrd for DijkstraState {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

#[derive(PartialEq, Eq)]
struct FourState {
    steps: usize,
    robots: [char; 4],
    keys: BTreeSet<char>,
}

impl Ord for FourState {
    fn cmp(&self, other: &Self) -> Ordering {
        other
            .steps
            .cmp(&self.steps)
            .then(self.keys.len().cmp(&other.keys.len()))
    }
}

impl PartialOrd for FourState {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

#[derive(PartialEq, Eq)]
struct State {
    steps: usize,
    node: char,
    keys: BTreeSet<char>,
}

impl Ord for State {
    fn cmp(&self, other: &Self) -> Ordering {
        other
            .steps
            .cmp(&self.steps)
            .then(self.keys.len().cmp(&other.keys.len()))
    }
}

impl PartialOrd for State {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

#[derive(Debug, PartialEq, Eq, Clone, Copy)]
enum Tile {
    Wall,
    Empty,
    Node(char),
}

fn parse_grid(input: &str) -> HashMap<Coordinate, Tile> {
    let mut grid = HashMap::new();
    let mut height = 0;
    for line in input.trim().lines() {
        let mut width = 0;
        for c in line.chars() {
            let tile = match c {
                '#' => Tile::Wall,
                '.' => Tile::Empty,
                _ => Tile::Node(c),
            };
            grid.insert(Coordinate(width, height), tile);
            width += 1;
        }
        height += 1;
    }
    grid
}

fn graph(grid: &HashMap<Coordinate, Tile>) -> HashMap<char, HashMap<char, usize>> {
    let mut graph = HashMap::new();
    for (coord, tile) in grid.iter() {
        if let Tile::Node(c) = tile {
            let pos_edges = reachable_from(grid, *coord);
            graph.insert(*c, pos_edges);
        }
    }

    graph
}

fn reachable_from(grid: &HashMap<Coordinate, Tile>, coord: Coordinate) -> HashMap<char, usize> {
    let mut visited = HashSet::new();
    let mut result = HashMap::new();

    let mut queue = VecDeque::new();
    queue.push_back((coord, 0));

    visited.insert(coord);
    while let Some((current, steps)) = queue.pop_front() {
        for neighbour in &current.neighbours() {
            if let Some(tile) = grid.get(neighbour) {
                if !visited.contains(neighbour) {
                    visited.insert(*neighbour);
                    match tile {
                        Tile::Empty => {
                            queue.push_back((*neighbour, steps + 1));
                        }
                        Tile::Node(c) => {
                            result.insert(*c, steps + 1);
                        }
                        Tile::Wall => {}
                    }
                }
            }
        }
    }
    result
}

fn search(graph: HashMap<char, HashMap<char, usize>>, start: char) -> usize {
    let mut priority_queue = BinaryHeap::new();
    let key_count = graph.iter().filter(|(k, _)| k.is_lowercase()).count();

    let mut distances: HashMap<(char, BTreeSet<char>), usize> = HashMap::new();
    distances.insert((start, BTreeSet::new()), 0);

    let start = State {
        steps: 0,
        node: start,
        keys: BTreeSet::new(),
    };

    priority_queue.push(start);

    let mut cache: HashMap<(char, BTreeSet<char>), Vec<(char, usize)>> = HashMap::new();

    while let Some(current) = priority_queue.pop() {
        if current.keys.len() == key_count {
            return current.steps;
        }

        if let Some(&best_steps) = distances.get(&(current.node, current.keys.clone())) {
            if current.steps > best_steps {
                continue;
            }
        }

        let cache_key = (current.node, current.keys.clone());

        let cached_entry = cache
            .entry(cache_key)
            .or_insert_with(|| search_keys(&graph, &current.keys, current.node));

        for &(next_node, cost) in cached_entry.iter() {
            let mut next_keys = current.keys.clone();
            next_keys.insert(next_node);
            let next_steps = current.steps + cost;

            let distances_entry = distances
                .entry((next_node, next_keys.clone()))
                .or_insert(usize::max_value());

            if next_steps < *distances_entry {
                *distances_entry = next_steps;

                let next_state = State {
                    steps: current.steps + cost,
                    node: next_node,
                    keys: next_keys,
                };

                priority_queue.push(next_state);
            }
        }
    }
    usize::max_value()
}

fn search_keys(
    graph: &HashMap<char, HashMap<char, usize>>,
    keys: &BTreeSet<char>,
    start: char,
) -> Vec<(char, usize)> {
    let mut dist = HashMap::new();
    for &key in graph.keys() {
        dist.insert(key, usize::max_value());
    }

    let mut heap = BinaryHeap::new();

    *dist.get_mut(&start).unwrap() = 0;
    heap.push(DijkstraState {
        cost: 0,
        current: start,
    });
    let mut reach = HashSet::new();

    while let Some(DijkstraState { cost, current }) = heap.pop() {
        if current.is_lowercase() && !keys.contains(&current) {
            reach.insert(current);
            continue;
        }

        if cost > dist[&current] {
            continue;
        }

        for (&next_node, &next_cost) in graph.get(&current).unwrap().iter() {
            if next_node.is_uppercase() && !keys.contains(&next_node.to_ascii_lowercase()) {
                continue;
            }

            let next = DijkstraState {
                cost: cost + next_cost,
                current: next_node,
            };

            if next.cost < dist[&next_node] {
                dist.insert(next_node, next.cost);
                heap.push(next);
            }
        }
    }
    reach.into_iter().map(|node| (node, dist[&node])).collect()
}

fn four_robots(grid: &mut HashMap<Coordinate, Tile>) {
    let robot_coord = grid
        .iter()
        .find(|(_, &v)| v == Tile::Node('@'))
        .map(|(k, _)| k.clone())
        .unwrap();

    grid.insert(robot_coord, Tile::Wall);
    for &neighbour in &robot_coord.neighbours() {
        grid.insert(neighbour, Tile::Wall);
    }
    grid.insert(
        Coordinate(robot_coord.0 - 1, robot_coord.1 - 1),
        Tile::Node('@'),
    );
    grid.insert(
        Coordinate(robot_coord.0 - 1, robot_coord.1 + 1),
        Tile::Node('='),
    );

    grid.insert(
        Coordinate(robot_coord.0 + 1, robot_coord.1 + 1),
        Tile::Node('%'),
    );
    grid.insert(
        Coordinate(robot_coord.0 + 1, robot_coord.1 - 1),
        Tile::Node('$'),
    );
}

fn search_four(graph: HashMap<char, HashMap<char, usize>>) -> usize {
    let mut priority_queue = BinaryHeap::new();
    let key_count = graph.iter().filter(|(k, _)| k.is_lowercase()).count();

    let mut distances: HashMap<([char; 4], BTreeSet<char>), usize> = HashMap::new();
    let robots = ['@', '=', '%', '$'];

    distances.insert((robots.clone(), BTreeSet::new()), 0);

    let start = FourState {
        steps: 0,
        robots: robots,
        keys: BTreeSet::new(),
    };

    priority_queue.push(start);

    let mut cache: HashMap<(char, BTreeSet<char>), Vec<(char, usize)>> = HashMap::new();
    while let Some(current) = priority_queue.pop() {
        if current.keys.len() == key_count {
            return current.steps;
        }

        if let Some(&best_steps) = distances.get(&(current.robots, current.keys.clone())) {
            if current.steps > best_steps {
                continue;
            }
        }

        for (robot_number, &robot_location) in current.robots.iter().enumerate() {
            let cache_key = (robot_location, current.keys.clone());

            let cached_entry = cache
                .entry(cache_key)
                .or_insert_with(|| search_keys(&graph, &current.keys, robot_location));

            for &(next_node, cost) in cached_entry.iter() {
                let mut next_keys = current.keys.clone();
                next_keys.insert(next_node);

                let mut next_robots = current.robots.clone();
                next_robots[robot_number] = next_node;

                let next_steps = current.steps + cost;

                let distances_entry = distances
                    .entry((next_robots.clone(), next_keys.clone()))
                    .or_insert(usize::max_value());

                if next_steps < *distances_entry {
                    *distances_entry = next_steps;
                    let next_state = FourState {
                        steps: next_steps,
                        robots: next_robots,
                        keys: next_keys,
                    };

                    priority_queue.push(next_state);
                }
            }
        }
    }
    usize::max_value()
}

#[cfg(test)]
mod tests {
    use super::part1;
    use super::part2;

    #[test]
    fn it_should_find_the_shortest_path_1() {
        assert_eq!(
            part1(&String::from(
                "
                #########
                #b.A.@.a#
                #########"
            )),
            8
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_2() {
        assert_eq!(
            part1(&String::from(
                "
                ########################
                #f.D.E.e.C.b.A.@.a.B.c.#
                ######################.#
                #d.....................#
                ########################"
            )),
            86
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_3() {
        assert_eq!(
            part1(&String::from(
                "
                ########################
                #...............b.C.D.f#
                #.######################
                #.....@.a.B.c.d.A.e.F.g#
                ########################"
            )),
            132
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_4() {
        assert_eq!(
            part1(&String::from(
                "
                #################
                #i.G..c...e..H.p#
                ########.########
                #j.A..b...f..D.o#
                ########@########
                #k.E..a...g..B.n#
                ########.########
                #l.F..d...h..C.m#
                #################"
            )),
            136
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_5() {
        assert_eq!(
            part1(&String::from(
                "
                ########################
                #@..............ac.GI.b#
                ###d#e#f################
                ###A#B#C################
                ###g#h#i################
                ########################"
            )),
            81
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_with_four_vaults() {
        assert_eq!(
            part2(&String::from(
                "
                #######
                #a.#Cd#
                ##...##
                ##.@.##
                ##...##
                #cB#Ab#
                #######"
            )),
            8
        );
    }
}
