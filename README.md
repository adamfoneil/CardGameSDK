I started this because I was trying to learn a game, [Fox in the Forest](https://ultraboardgames.com/the-fox-in-the-forest/game-rules.php), and I wasn't quite getting it. I was reading the directions, and some of the language was not precise enough for my programmer brain. I got frustrated, and wondered if there could be a better way to design and test card games -- to iterate rapidly and get feedback. A natural next step would be to play card games online with friends. Numerous sites exist for that today, but are there any that let you develop your own games? I've not heard of that. How would one "develop" card games?

That's when I thought -- let's build a small SDK for card games that can be tested in some kind of host app sandbox environment -- and online with friends!

I figured I should try to model a game I already know -- to see what features an SDK should support. I chose Hearts.

I broke it down like this:
- first I have the notion of a [PlayingCard](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/PlayingCard.cs) which has suits, ranks, and let's say a [StandardDeck](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/PlayingCard.cs#L43). Fox in the Forest uses its own suits and ranks, so I knew this would need to be very flexible. But I also wanted to work with something very recognizable, hence the `StandardDeck` and familiar suits (clubs, diamonds, hearts, spades).
- then I have the notion of a [GameFactory](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/GameFactory.cs) -- something responsible for shuffling cards, launching new games, and enforcing some top level rules (like number of players).
- entwined with the GameFactory is the idea of a [GameState](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/GameState.cs) object. If I were to pause and resume a game later, how could I ensure that I picked up where I left off and that the play history was preserved? All of that would need to be in the `GameState`. This also in turn defines the actual rules of the game.

Hearts components:
- [HeartsGameFactory](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/Games/Hearts/HeartsGameFactory.cs)
- [HeartsGameState](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/Games/Hearts/HeartsGameState.cs)

I'm still a long ways from having something playable in a web UI, but I needed some feedback on whether what I've done so far is valid -- as far as my implementation of Hearts. So, I have a few superficial [tests](https://github.com/adamfoneil/CardGame/blob/master/Testing/ShuffleAndDeal.cs). These don't make many useful assertions, but at this point I'm just seeing if the very basics work.
