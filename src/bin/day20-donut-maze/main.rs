use std::cmp::Ordering;
use std::collections::{BTreeMap, BinaryHeap, HashMap, HashSet, VecDeque};
use std::usize;
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day20-donut-maze/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input));

    Ok(())
}

fn part1(input: &str) -> usize {
    let grid = parse_input(input);

    let nodes = parse_nodes(&grid);
    let graph = parse_graph(&grid, &nodes);
    if let Some(u) = shortest_path(
        graph,
        Portal::Outer(String::from("AA")),
        Portal::Outer(String::from("ZZ")),
    ) {
        u
    } else {
        usize::MAX
    }
}

fn part2(input: &str) -> usize {
    let grid = parse_input(input);

    let nodes = parse_nodes(&grid);
    let graph = parse_graph(&grid, &nodes);

    if let Some(u) = shortest_path_depth(
        graph,
        Portal::Outer(String::from("AA")),
        Portal::Outer(String::from("ZZ")),
    ) {
        u
    } else {
        usize::MAX
    }
}

#[derive(Debug, PartialEq, Eq, Hash, Clone)]
enum Portal {
    Outer(String),
    Inner(String),
}

fn parse_input(input: &str) -> BTreeMap<(i32, i32), char> {
    let mut result = BTreeMap::new();
    let mut y = 0;
    for line in input.split('\n') {
        let mut x = 0;
        for c in line.chars() {
            result.insert((y, x), c);
            x += 1;
        }
        y += 1;
    }
    result
}

fn inside_box(position: (i32, i32), top_left: (i32, i32), bottom_right: (i32, i32)) -> bool {
    position.0 >= top_left.0
        && position.1 >= top_left.1
        && position.0 <= bottom_right.0
        && position.1 <= bottom_right.1
}

fn parse_nodes(map: &BTreeMap<(i32, i32), char>) -> HashMap<(i32, i32), Portal> {
    let outer_bottom_right = *map.iter().last().unwrap().0;

    let maze_top_left = (2, 2);
    let maze_bottom_right = (outer_bottom_right.0 - 2, outer_bottom_right.1 - 2);

    let mut inner_space = map.iter().filter(|(&pos, &c)| {
        inside_box(pos, maze_top_left, maze_bottom_right) && c != '#' && c != '.'
    });
    let inner_top_left = *inner_space.next().unwrap().0;
    let inner_bottom_right = *inner_space.rev().next().unwrap().0;

    let mut result = HashMap::new();

    for (&pos, &tile) in map.iter().filter(|(&pos, _)| {
        inside_box(pos, maze_top_left, maze_bottom_right)
            && !inside_box(
                pos,
                (maze_top_left.0 + 1, maze_top_left.1 + 1),
                (maze_bottom_right.0 - 1, maze_bottom_right.1 - 1),
            )
    }) {
        if tile == '.' {
            let name = portal_name(&map, pos);
            result.insert(pos, Portal::Outer(name));
        }
    }

    for (&pos, &tile) in map.iter().filter(|(&pos, _)| {
        inside_box(
            pos,
            (inner_top_left.0 - 1, inner_top_left.1 - 1),
            (inner_bottom_right.0 + 1, inner_bottom_right.1 + 1),
        ) && !inside_box(pos, inner_top_left, inner_bottom_right)
    }) {
        if tile == '.' {
            let name = portal_name(&map, pos);
            result.insert(pos, Portal::Inner(name));
        }
    }
    result
}

fn portal_name(map: &BTreeMap<(i32, i32), char>, pos: (i32, i32)) -> String {
    let dirs: &[(i32, i32)] = &[(-1, 0), (0, -1), (1, 0), (0, 1)];
    let mut result = Vec::with_capacity(2);
    for (idx, &dir) in dirs.iter().enumerate() {
        let next = (pos.0 + dir.0, pos.1 + dir.1);
        if let Some(&c1) = map.get(&next) {
            if c1.is_ascii_alphabetic() {
                result.push(c1);
                if let Some(&c2) = map.get(&(next.0 + dir.0, next.1 + dir.1)) {
                    result.push(c2);
                }
                if idx < 2 {
                    result.reverse();
                }
                break;
            }
        }
    }

    result.into_iter().collect()
}

fn parse_graph(
    map: &BTreeMap<(i32, i32), char>,
    nodes: &HashMap<(i32, i32), Portal>,
) -> HashMap<Portal, HashMap<Portal, usize>> {
    let mut result = HashMap::with_capacity(nodes.len());
    for (_, portal) in nodes.iter() {
        result.insert(portal.clone(), HashMap::new());
    }

    let dirs = &[(0, -1), (0, 1), (-1, 0), (1, 0)];
    for (portal_pos, portal) in nodes.iter() {
        let mut visited = HashSet::new();
        let mut queue = VecDeque::new();
        queue.push_back((0usize, *portal_pos));
        visited.insert(*portal_pos);

        while let Some((steps, pos)) = queue.pop_front() {
            for dir in dirs.iter() {
                let next = (pos.0 + dir.0, pos.1 + dir.1);

                if visited.contains(&next) {
                    continue;
                }

                if let Some(connected_portal) = nodes.get(&next) {
                    result
                        .get_mut(portal)
                        .unwrap()
                        .entry(connected_portal.clone())
                        .or_insert(steps + 1);

                    continue;
                }

                if let Some(&c) = map.get(&next) {
                    if c == '.' {
                        queue.push_back((steps + 1, next));
                        visited.insert(next);
                    }
                }
            }
        }
    }

    let portals: HashSet<Portal> = result.clone().into_iter().map(|(k, _)| k).collect();

    for (portal, adj) in result.iter_mut() {
        let other = match portal {
            Portal::Inner(name) => Portal::Outer(name.clone()),
            Portal::Outer(name) => Portal::Inner(name.clone()),
        };

        if portals.contains(&other) {
            adj.insert(other, 1);
        }
    }

    result
}

#[derive(Clone, Eq, PartialEq)]
struct State {
    depth: usize,
    cost: usize,
    node: Portal,
}

impl Ord for State {
    fn cmp(&self, other: &State) -> Ordering {
        other
            .depth
            .cmp(&self.depth)
            .then_with(|| other.cost.cmp(&self.cost))
    }
}

impl PartialOrd for State {
    fn partial_cmp(&self, other: &State) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

fn shortest_path(
    graph: HashMap<Portal, HashMap<Portal, usize>>,
    start: Portal,
    goal: Portal,
) -> Option<usize> {
    let mut dist: HashMap<Portal, usize> =
        graph.iter().map(|(k, _)| (k.clone(), usize::MAX)).collect();

    let mut heap = BinaryHeap::new();

    dist.insert(start.clone(), 0);
    heap.push(State {
        depth: 0,
        cost: 0,
        node: start,
    });

    while let Some(State { cost, node, .. }) = heap.pop() {
        if node == goal {
            return Some(cost);
        }

        if cost > dist[&node] {
            continue;
        }

        for (next_node, travel_cost) in &graph[&node] {
            let next = State {
                depth: 0,
                cost: cost + travel_cost,
                node: next_node.clone(),
            };

            if next.cost < dist[&next.node] {
                dist.insert(next.node.clone(), next.cost);
                heap.push(next);
            }
        }
    }
    None
}

fn shortest_path_depth(
    graph: HashMap<Portal, HashMap<Portal, usize>>,
    start: Portal,
    goal: Portal,
) -> Option<usize> {
    let mut dist: HashMap<(usize, Portal), usize> = HashMap::new();

    let mut heap = BinaryHeap::new();

    dist.insert((0, start.clone()), 0);
    heap.push(State {
        depth: 0,
        cost: 0,
        node: start.clone(),
    });

    while let Some(State { depth, cost, node }) = heap.pop() {
        if node == goal && depth == 0 {
            return Some(cost);
        }

        let current_key = (depth, node.clone());
        if let Some(&dist_cost) = dist.get(&current_key) {
            if cost > dist_cost {
                continue;
            }
        }

        for (next_node, &travel_cost) in &graph[&node] {
            if travel_cost == 1 && depth == 0 {
                if let Portal::Outer(_) = node {
                    continue;
                }
            }

            let next_depth = if travel_cost == 1 {
                match node {
                    Portal::Outer(_) => depth - 1,
                    Portal::Inner(_) => depth + 1,
                }
            } else {
                depth
            };
            let next = State {
                depth: next_depth,
                cost: cost + travel_cost,
                node: next_node.clone(),
            };

            let dist_next = dist
                .entry((next_depth, next_node.clone()))
                .or_insert(usize::MAX);
            if next.cost < *dist_next {
                *dist_next = next.cost;
                heap.push(next);
            }
        }
    }
    None
}

#[cfg(test)]
mod test {
    use super::part1;
    use super::part2;

    #[test]
    fn it_should_find_the_shortest_path_1() {
        assert_eq!(
            part1(&String::from(
                "         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       "
            )),
            23
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_2() {
        assert_eq!(
            part1(&String::from(
                "                   A               
                   A               
  #################.#############  
  #.#...#...................#.#.#  
  #.#.#.###.###.###.#########.#.#  
  #.#.#.......#...#.....#.#.#...#  
  #.#########.###.#####.#.#.###.#  
  #.............#.#.....#.......#  
  ###.###########.###.#####.#.#.#  
  #.....#        A   C    #.#.#.#  
  #######        S   P    #####.#  
  #.#...#                 #......VT
  #.#.#.#                 #.#####  
  #...#.#               YN....#.#  
  #.###.#                 #####.#  
DI....#.#                 #.....#  
  #####.#                 #.###.#  
ZZ......#               QG....#..AS
  ###.###                 #######  
JO..#.#.#                 #.....#  
  #.#.#.#                 ###.#.#  
  #...#..DI             BU....#..LF
  #####.#                 #.#####  
YN......#               VT..#....QG
  #.###.#                 #.###.#  
  #.#...#                 #.....#  
  ###.###    J L     J    #.#.###  
  #.....#    O F     P    #.#...#  
  #.###.#####.#.#####.#####.###.#  
  #...#.#.#...#.....#.....#.#...#  
  #.#####.###.###.#.#.#########.#  
  #...#.#.....#...#.#.#.#.....#.#  
  #.###.#####.###.###.#.#.#######  
  #.#.........#...#.............#  
  #########.###.###.#############  
           B   J   C               
           U   P   P               "
            )),
            58
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_with_layers_1() {
        assert_eq!(
            part2(&String::from(
                "         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       "
            )),
            26
        );
    }

    #[test]
    fn it_should_find_the_shortest_path_with_layers_2() {
        assert_eq!(
            part2(&String::from(
                "             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     "
            )),
            396
        );
    }
}
