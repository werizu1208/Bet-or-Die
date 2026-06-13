# Bet or Die — Project Handoff for Claude

## Overview
Unity gambling/survival game. The player faces demons and chooses to bet or refuse each encounter, spending resources to escape.

## Core Concept
- **Title:** Bet or Die
- **Genre:** Gambling / Survival
- **Engine:** Unity
- **Status:** Pre-production (concept only, no code yet)
- **Repository:** GitHub (shared across two PCs — always pull before working)

## Resource System
Three resource pools, consumed **left to right in this priority order:**

| Priority | Resource | Notes |
|----------|----------|-------|
| 1st | 金 (Money) | Spent first |
| 2nd | HP | Spent second |
| 3rd | 四肢 (Limbs) | Spent last — losing limbs likely has permanent gameplay consequences |

The ordering is intentional: the player starts losing money, then health, then body parts — escalating stakes.

## Gameplay Loop
1. Player encounters a demon
2. Player chooses: **Bet** or **Refuse**
3. If betting: wager resources, resolve the gamble
4. Repeat until the player **escapes** or loses everything

## Key Design Questions (not yet decided)
- What determines win/loss probability per demon?
- What happens when all limbs are lost (game over? special state?)
- How many demons / stages before escape?
- Is there a roguelike structure (run-based) or a single narrative playthrough?
- Can the player refuse indefinitely, or does refusal have a cost?

## Workflow
- Unity project, version controlled via GitHub
- Two development machines share the same repo
- **Always `git pull` before starting a session**
- Prefer small, focused commits

## For Claude on a New Machine
1. Read this file first
2. Check `git log --oneline -10` to see recent changes
3. Ask the user what they want to work on today
4. Do not assume any code exists — verify with file search before referencing implementation details
