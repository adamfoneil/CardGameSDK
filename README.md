I started this because I was trying to learn a game, [Fox in the Forest](https://ultraboardgames.com/the-fox-in-the-forest/game-rules.php), and I wasn't quite getting it. I was reading the directions, and some of the language was not precise enough for my programmer brain. I got frustrated, and wondered if there could be a better way to design and test card games -- to iterate rapidly and get feedback. A natural next step would be to play card games online with friends. Numerous sites exist for that today, but are there any that let you develop your own games? I've not heard of that. How would one "develop" card games?

That's when I thought -- let's build a small SDK for card games that can be tested in some kind of host app sandbox environment -- and online with friends!

I figured I should try to model a game I already know -- to see what features an SDK should support. I chose Hearts.

I broke it down like this:
- first I have the notion of a [PlayingCard](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/PlayingCard.cs) which has suits, ranks, and let's say a [ClassicDeck](https://github.com/adamfoneil/CardGameSDK/blob/master/CardGame.Abstractions/PlayingCard.cs#L22). Fox in the Forest uses its own suits and ranks, so I knew this would need to be flexible. But I also wanted to work with something recognizable, hence the `ClassicDeck` and familiar suits (clubs, diamonds, hearts, spades).
- then I have the notion of a [GameFactory](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/GameFactory.cs) -- something responsible for shuffling cards, launching new games, and enforcing some top level rules (like number of players).
- entwined with the GameFactory is the idea of a [GameState](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/GameState.cs) object. If I were to pause and resume a game later, how could I ensure that I picked up where I left off and that the play history was preserved? All of that would need to be in the `GameState`. This also in turn defines the actual rules of the game.

Hearts components:
- [HeartsGameFactory](https://github.com/adamfoneil/CardGameSDK/blob/master/Hearts/HeartsGameFactory.cs)
- [HeartsGameState](https://github.com/adamfoneil/CardGameSDK/blob/master/Hearts/HeartsGameState.cs)

I'm still a long ways from having something playable in a web UI, but I needed some feedback on whether what I've done so far is valid -- as far as my implementation of Hearts. So, I have a few superficial [tests](https://github.com/adamfoneil/CardGame/blob/master/Testing/ShuffleAndDeal.cs). These don't make many useful assertions, but at this point I'm just seeing if the very basics work. In order to test a real-ish game, I had to introduce the notion of [AutoPlay](https://github.com/adamfoneil/CardGame/blob/master/CardGame.Abstractions/Games/Hearts/HeartsGameState.cs#L128). Simulated play does not need to be good or strategic, but I needed some way to play a round from end to end to see how the scoring worked, whether hearts would "break" as expected. Since the deals are random however (by design), I get different results every time. So, there are still some difficulties with this.

# Stuff I Learned
See my detailed notes in the [Wiki](https://github.com/adamfoneil/CardGameSDK/wiki).

- When comparing suits, I need to use the `Equals` method instead of `==`. I was a little confused by this. Using `==` resulted in false negatives. I think this is a side effect of overriding the `Equals` method. I considered using `records` instead of `classes` for suits. I might have to revisit.
- Not sure all of my Hearts rules are right. Trying to progammatically articulate rules I know "intuitively" or from habit was pretty of hard -- for example when trying to simulate [automatic play](https://github.com/adamfoneil/CardGameSDK/blob/master/Hearts/HeartsGameState.cs#L146). When reading Hearts rules online, I found many of them confusing, even though I know the game.

# News
There's a Server [BlazorApp](https://github.com/adamfoneil/CardGameSDK/tree/master/BlazorApp) in progress running at [cardplace.azurewebsites.net](https://cardplace.azurewebsites.net/). Note it's on a low service tier, so it might be slow to start. You can actually register and experiment with the [Hearts](https://github.com/adamfoneil/CardGameSDK/tree/master/Hearts) game by going to the [Ready page](https://cardplace.azurewebsites.net/Ready). You'll need 4 players online simultaneously.

What the main UI looks like:

![image](https://github.com/user-attachments/assets/a2292007-ab59-4403-801d-75d3637245e1)
