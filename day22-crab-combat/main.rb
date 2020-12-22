require 'set'
require 'test/unit/assertions'
include Test::Unit::Assertions

test_arr = [
  "Player 1:\n9\n2\n6\n3\n1",
  "Player 2:\n5\n8\n4\n7\n10",
]

test_arr_2 = [
  "Player 1:\n43\n19",
  "Player 2:\n2\n29\n14",
]

input_arr = File.open(File.join(File.dirname(__FILE__), 'input.txt')).read().split("\n\n")

class CombatGame
  def initialize(input)
    @deck_1, @deck_2 = input.map { |raw_deck| raw_deck.split("\n")[1..-1].map(&:to_i) }
  end

  def play(deck_1, deck_2)
    while deck_1.length > 0 && deck_2.length > 0
      card1 = deck_1.shift()
      card2 = deck_2.shift()
      if card1 > card2
        deck_1.push(card1)
        deck_1.push(card2)
      else
        deck_2.push(card2)
        deck_2.push(card1)
      end
    end

    return deck_1.length > 0
  end

  def score
    winning_deck = play(@deck_1, @deck_2) ? @deck_1 : @deck_2
    len = winning_deck.length
    winning_deck.each.with_index.reduce(0) { |acc, (card, ind)| acc += card * (len - ind) }
  end
end

class RecursiveCombatGame < CombatGame
  def play(deck_1, deck_2)
    player1_decks = Set.new()
    player2_decks = Set.new()

    while deck_1.length > 0 && deck_2.length > 0
      player1_deck = deck_1.join(',')
      player2_deck = deck_2.join(',')

      if player1_decks.include?(player1_deck) || player2_decks.include?(player2_deck)
        return true
      end

      player1_decks.add(player1_deck)
      player2_decks.add(player2_deck)

      card1 = deck_1.shift()
      card2 = deck_2.shift()

      player_1_won = deck_1.length >= card1 && deck_2.length >= card2 ? play(deck_1[0..(card1 - 1)], deck_2[0..(card2 - 1)]) : card1 > card2

      if player_1_won
        deck_1.push(card1)
        deck_1.push(card2)
      else
        deck_2.push(card2)
        deck_2.push(card1)
      end
    end

    return deck_1.length > 0
  end
end

def part1(input)
  CombatGame.new(input).score
end

def part2(input)
  RecursiveCombatGame.new(input).score
end

assert_equal part1(test_arr), 306
puts part1(input_arr)

assert_equal part2(test_arr), 291
assert_equal part2(test_arr_2), 105
puts part2(input_arr)