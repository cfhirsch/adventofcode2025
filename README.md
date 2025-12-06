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



Dec 3:



Part 1: This one was easy. No notes required.



Part 2: Ah, the infamous "my initial solution for Part 1 did not scale." I tried going through all possible combinations of m indices out of n possible values

but then I realized how huge n choose m was in this case. Then I realized a greedy solution works here. Starting at index i, find the sequence of m numbers

that generates the largest number. If there are only m values left, just return that. Otherwise find the largest digit starting at index i, update index i, 

decrement m, rinse, lather, repeat. Code ran in 46 ms.



Dec 4:



Part 1: This one was easy. Just load the input into a two dimensional array, and check the neighbors of each square that contains a roll.



Part 2: I was dreading part 2 given how easy part 1 was, but this one was also straightforward. Just iterated on the approach I took to Part 1, store the results

of removing each round's worth of rolls in a new array, keep going until I can't remove any more.





