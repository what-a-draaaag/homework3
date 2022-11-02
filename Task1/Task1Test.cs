using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{
    Card card1 = new Card(Suit.Hearts, Rank.Eight);
    Card card2 = new Card(Suit.Spades, Rank.King);
    Card card3 = new Card(Suit.Spades, Rank.Ten);
    Card card4 = new Card(Suit.Clubs, Rank.Ace);
    Card card5 = new Card(Suit.Diamonds, Rank.Jack);
    Card card6 = new Card(Suit.Diamonds, Rank.Six);

    [Test]
    public void RoundWinnerTest()
    {

        That(RoundWinner(card1, card2), Is.EqualTo(Player.Second));
        That(RoundWinner(card3, card4), Is.EqualTo(Player.Second));
        That(RoundWinner(card5, card6), Is.EqualTo(Player.First));
    }

    [Test]
    public void FullDeckTest()
    {
        var deck = FullDeck();
        var deckClubs = deck.Where(x => x.suit == Suit.Clubs).ToList();

        That(deck, Has.Count.EqualTo(DeckSize));
        That(deckClubs, Has.Count.EqualTo(DeckSize / 4));
    }

    [Test]
    public void RoundTest()
    {
        Dictionary<Player, List<Card>> roundHands = new Dictionary<Player, List<Card>>()
        {
            { Player.First, new List<Card>()
            { card5 , card3, card1} },

            { Player.Second, new List<Card>()
            { card6, card4, card2} },
        };

        Player result = Round(roundHands).Item1;

        That(result, Is.EqualTo(Player.Second));
    }

    [Test]
    public void Game2CardsTest()
    {
        var seven = new List<Card>();
        var king = new List<Card>();

        for (int i = 1; i < 5; ++i)
        {
            seven.Add(new Card((Suit)i, Rank.Seven));
            king.Add(new Card((Suit)(5 - i), Rank.King));
        }

        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.First, seven },
            { Player.Second, king }
        };

        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.Second));
    }
}