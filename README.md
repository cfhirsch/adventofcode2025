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



Part 2: Pretty straightforward after solving Part 1. My solution to part 1, which found all duplicates in each range that could be constructed by

two copies of numbers each half the length of the original number, could be generalized to any n from 2 to the length of the number. I just needed to 

be careful not to double count. For example, in the range 222220-222224, 222222 would both be found with n = 2 ("222" + "222") and n = 3 ("22" + "22" + "22").

So I added a HashSet to keep track of which invalid Ids my algorithm had already found.





