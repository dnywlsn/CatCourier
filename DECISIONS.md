# DECISIONS.md - Cat Courier

What I asked the AI agent, what it got wrong, what I changed and why.

## Week 10 - Three enemy types

2026-11-11  Chose 3 enemies for my rooftop game: pigeon (dives from the sky), dog (patrol + chase on a roof), crow (throws pebbles). Asked the agent (Claude) to make the art, the 3 state machines and place them in Level 1.
2026-11-11  Agent's first Dog got stuck at the edge of its roof and never turned around (Clamp kept x inside the limits, so "x > rightX" was never true). Changed the check to >= and <=.
2026-11-11  First pigeon patrolled right above the first gap and dived while the cat was jumping, so the cat always fell in the first 5 seconds. Pigeon1 leftX 9 -> 11.5.
2026-11-11  Falling between roofs sent the cat back to the very start - too harsh with enemies. Now the cat goes back to the last roof it stood on (lastGround) and blinks for 1 s.
2026-11-11  Added a "TRY N" counter so tries are easy to count in the playtest.

## Playtest (fill in)

Tester 1: ___ tries
Tester 2: ___ tries
Changed after playtest:
- ___: ___ -> ___
- ___: ___ -> ___
