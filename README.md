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



Dec 5:



Part 1: Easy, although I had to update to use longs instead of ints when I looked at actual puzzle input.



Part 2: Ugh, interval merging problem. I tried using an algorithm that I found online, and it worked for the test input, but not for my puzzle input.

I have no idea why. I ended up cribbing from https://aoc.csokavar.hu/2025/5/.



Dec 6:



Part 1: Straightforward - parse the puzzle input, follow the rules.



Part 2: Also straightforward, although a little more involved to follow the rules.



Dec 7:



Part 1: Not too difficult, but I had a late night last night and brain is not working at 100% :). You simply need to count the number of splitters that get 

hit by a beam.



Part 2: Needed to be more rested before I could figure this one out. This time, as I'm walking through the possible beam paths, I build up a tree. Then

I use a recursive algorithm with memoization to quickly add up all the possible paths from the root to a leaf node.



Dec 8:



Part 1: Not that difficult. I generated a dictionary where the keys were tuples of 3D points, and the values were the distance between them, then sorted by distance.

I also maintained a list of circuits, where initially each box is in its own dedicated circuit. I loop through the first n keys in the dictionary, and find the circuit

that currently contains source and target boxes. If they are not the same circuit, I append the second to the first and remove the second from the list of circuits.



Part 2: Also straightforward. I just needed to iterate in my loop until the number of circuits equaled one. I did have to be careful when calculating the product, as

the result is a long and I was multiplying two ints.



Dec 9:



Part 1: Easy. Wrote a method to calculate rectangle area (being sure to handle the corner cases where both corners are on the same row or same column), then loop

through all possible pairs of distinct tiles to find the max.

Part 2: Welp, geometry is my kryptonite. I ended up asking CoPilot to write an algorithm that, given a integer valued list of segment endpoints for a polygon, and the corners of a rectangle, determines whether rectangle is contained in the polygon (where
it's OK for the border of the rectangle to overlap with the border of the polygon).

Dec 10:

Part 1: I started trying to parse out the input using regular expressions and ended up using a simpler string parsing approach to deserialize the machines. Then I did breadth-first search to find the mininal number of button presses for each machine. My
solution took almost 10 seconds so there must be a faster approach than the one I used. I did get the right answer, though.

UPDATE: In attempting to solve part 2, I changed my approach to a backwards search from the target state. Starting at target,
how many states are one button press away, then use those states to find the states that are two button presses away, etc.
until I find the start state. Sped up part 1, still waaayyyyy too slow for part 2. :).

Part 2: First time I've ever added a NuGet package dependency to solve an Advent Of Code problem lol. I finally started looking
at this as an integer-valued linear programming problem. I asked CoPilot and it gave me a C# implementation to solve this problem type and the solution it gave me uses Google.OrTools. I then transformed each machine into an instance of this problem
and got the right answer in 2.5 seconds. Not super great but not super bad either.

Dec 11:

Part 1: Easy. I parsed the puzzle input into a dictionary and used breadth first search to find all the paths.

Part 2: Not so easy :). One of those classic Advent of Code problems where the naive approach I took to Part 1 just did not scale. There were waaayyyyyy too many paths between devices. I ended up constructing a function that calculates the number
of paths from start to end that not pass between any device in except:

C(s, t, except) = 1

C(out, t, except) = 1 if t = "out", 0 otherwise

C(s, t, except) = Sum(o in outputs(s), C(o, t, except))

I added in memoization - not sure if needed but it was really fast. The answer is then:

C(svr, dac, [fft])*C(dac, fft, [svr])*C(fft, out, [svr, dac]) + C(svc, fft, [dac])*C(fft, dac, [svr]) * C(dac, out, [svr, fft])

Dec 12:

Part 1: OK this was - not so satisfying. I struggled with a backtracking approach and it was quick on the first two test examples
(where a solution exists), but then takes forever going through all possible ways to pack presents in the last example
(where a solution does NOT exist). Adding memoization didn't help. Then I tried writing an algorithm that tries to find the 
smallest possible packing of a set of presents, and then checking that it fits within a given bounding rectangle, but my
algorithm wasn't finding the tightest packing and it wasn't clear to me why.

Finally, on Christmas Day, I surrendered and looked at the forums. For the puzzle input, it suffices to check that the area
under the tree is at least as large as the sum of the bounding rectangles (all 3x3) of the presents. What is really frustrating
here is that THIS IS NOT TRUE FOR THE TEST INPUT! Specifically, this check fails for the first test example, yet it is possible
to find a packing solution in this case. IMHO it's perfectly fine for the real puzzle input to have examples that are not 
present in the test input - that's part of the challenge, after all, thinking through all the corner cases. It's not so fine
for the test input to have examples that are NOT present in the real puzzle input, and that therefore you don't need to think through. It's a bit - misleading.

