# adventofcode2025

My solutions to 2025 Advent of Code.



Dec 1:



Part 1: Fairly straightforward modulo arithmetic. I needed to be careful about subtracting when the number of rotations was more than 100, but after

I accounted for that I got the right answer.



Part 2: Well this took a little bit of time for me to get right. First time I've had trouble with a puzzle on day one :). I had to be careful about

the edge cases (for example, if we start from 0 and move left less than 100 times, we never pass 0).



Dec 2:



Part 1: Was overthinking this one. If both numbers in a range have an odd number of digits, then there are no invalid ids between them. Otherwise,

need to figure out the min and max range of numbers with length half of the relevant endpoint, then loop through each one and check if it is in bounds.





