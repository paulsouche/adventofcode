// See https://aka.ms/new-console-template for more information
using Namespace;

var fileStream = File.ReadAllText(Path.Combine("Program", "input.txt"));

var part1 = new Part1(fileStream);
var part2 = new Part2(fileStream);

Console.WriteLine(part1.Solve()); // 546312
Console.WriteLine(part2.Solve()); // 87449461
