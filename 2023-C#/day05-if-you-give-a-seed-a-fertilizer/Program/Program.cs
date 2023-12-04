// See https://aka.ms/new-console-template for more information
using Namespace;

var fileStream = File.ReadAllText(Path.Combine("Program", "input.txt"));

var part1 = new Part1(fileStream);
var part2 = new Part2(fileStream);

Console.WriteLine(part1.Solve()); // 535088217
// We could start from 0 but it's longer
long seedStart = 51000000;
Console.WriteLine(part2.Solve(seedStart)); // 51399228
