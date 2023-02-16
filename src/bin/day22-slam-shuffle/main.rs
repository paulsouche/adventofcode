use regex::Regex;
use std::{fs::read_to_string, io};

fn main() -> io::Result<()> {
    let input = read_to_string("src/bin/day22-slam-shuffle/input.txt")?;

    println!("{}", part1(&input));
    println!("{}", part2(&input));

    Ok(())
}

fn part1(input: &str) -> u16 {
    let actions = parse_actions(input);
    card_find(2019, 10007, &actions) as u16
}

fn part2(input: &str) -> i128 {
    let actions = parse_actions(input);
    let deck_size = 119315717514047;

    let x = 2020;
    let y = reverse_apply(x, deck_size, &actions);
    let z = reverse_apply(y, deck_size, &actions);

    let a = ((y - z) * modinv(x - y, deck_size)) % deck_size;
    let b = (y - a * x) % deck_size;

    let times = 101741582076661;

    let one = mod_power(a, times, deck_size) * x;
    let two = mod_power(a, times, deck_size) - 1;
    let three = modinv(a - 1, deck_size);

    (one + mul_mod(mul_mod(two, three, deck_size), b, deck_size)) % deck_size
}

#[derive(Debug, Eq, PartialEq)]
enum Action {
    Increment(i128),
    Cut(i128),
    Deal,
}

fn parse_actions(input: &str) -> Vec<Action> {
    let mut result = Vec::new();
    let r1 = Regex::new(r"deal with increment (\d+)").unwrap();
    let r2 = Regex::new(r"cut (-?\d+)").unwrap();
    let r3 = Regex::new("deal into new stack").unwrap();

    for line in input.lines() {
        if let Some(c) = r1.captures(line) {
            result.push(Action::Increment(c[1].parse().unwrap()));
        } else if let Some(c) = r2.captures(line) {
            result.push(Action::Cut(c[1].parse().unwrap()));
        } else if let Some(_) = r3.captures(line) {
            result.push(Action::Deal);
        }
    }

    result
}

fn card_find(mut idx: i128, size: i128, actions: &[Action]) -> i128 {
    for action in actions.iter() {
        match action {
            Action::Deal => idx = card_deal(idx, size),
            Action::Cut(cut) => idx = card_cut(idx, size, *cut),
            Action::Increment(incr) => idx = card_increment(idx, size, *incr),
        }
    }
    idx
}

fn card_increment(idx: i128, size: i128, incr: i128) -> i128 {
    (idx * incr) % size
}

fn card_cut(idx: i128, size: i128, cut: i128) -> i128 {
    (idx + size - cut) % size
}

fn card_deal(idx: i128, size: i128) -> i128 {
    size - idx - 1
}

fn rev_card_deal(res: i128, size: i128) -> i128 {
    size - res - 1
}

fn rev_card_cut(res: i128, size: i128, cut: i128) -> i128 {
    (res + cut + size) % size
}

fn egcd(a: i128, b: i128) -> (i128, i128, i128) {
    if a == 0 {
        (b, 0, 1)
    } else {
        let (g, y, x) = egcd(b % a, a);
        (g, x - (b / a) * y, y)
    }
}

fn modinv(a: i128, m: i128) -> i128 {
    let (g, x, _) = egcd(a, m);
    if g != 1 {
        panic!("Modular inverse does not exist");
    }
    x % m
}

fn mod_power(mut a: i128, mut b: i128, p: i128) -> i128 {
    let mut res = 1;

    a = a % p;
    if a == 0 {
        return 0;
    }
    while b > 0 {
        if b & 1 == 1 {
            res = (res * a) % p
        }
        b = b >> 1;
        a = (a * a) % p
    }
    res
}

fn rev_card_increment(res: i128, size: i128, incr: i128) -> i128 {
    (modinv(incr, size) * res) % size
}

fn reverse_apply(mut res: i128, size: i128, actions: &[Action]) -> i128 {
    for action in actions.iter().rev() {
        match action {
            Action::Deal => res = rev_card_deal(res, size),
            Action::Cut(cut) => res = rev_card_cut(res, size, *cut),
            Action::Increment(incr) => res = rev_card_increment(res, size, *incr),
        }
    }
    res
}

fn mul_mod(mut a: i128, mut b: i128, m: i128) -> i128 {
    if a >= m {
        a %= m;
    }
    if b >= m {
        b %= m;
    }
    let x = a;
    let c = x * b / m;
    let r = (a * b - c * m) % m;
    if r < 0 {
        r + m
    } else {
        r
    }
}
