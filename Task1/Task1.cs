// Колода
using NUnit.Framework.Internal;
using System.Runtime.ExceptionServices;
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;




// Масть
internal enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
};

// Значение
internal enum Rank
{
    Six,
    Seven,
    Eight,
    Nine,
    Ten ,
    Jack,
    Queen,
    King,
    Ace
};

// Карта
record Card(Suit suit, Rank rank);

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    First,
    Second
};

namespace Task1
{
    public class Task1
    {

        /*
        * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
        * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
        * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
        * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
        * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
        *
        * Реализация должна сопровождаться тестами.
        */

        internal static string writeDeck(Deck deck)
        {
            string s = "|";
            foreach (var card in deck) s += $"{card.suit}{card.rank}|";
            return s;
        }

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            Rank rank1 = card1.rank;
            Rank rank2 = card2.rank;
            if (card1.rank == card2.rank)
                return null;
            if (rank1 > rank2) return Player.First;
            return Player.Second;
        }

        // Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            Deck deck = new Deck();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 9; j++)
                    deck.Add(new Card((Suit)i, (Rank)j));
            return deck;
        }

        // Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            var random = new Random();
            int a;
            Hand hand1 = new Hand();
            Hand hand2 = new Hand();
            while (deck.Count > 0)
            {
                a = random.Next(0, deck.Count - 1);
                if (deck.Count % 2 == 1) hand1.Add(deck[a]);
                else hand2.Add(deck[a]);
                deck.RemoveAt(a);
            }
            Dictionary<Player, Hand> result = new Dictionary<Player, Hand>()
            {
                { Player.First, hand1 },
                { Player.Second, hand2},
            };
            return result;
            
        }

        // Один раунд игры (в том числе спор при равных картах).
        // Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player, Table> Round(Dictionary<Player, Hand> hands)
        {
            Table table = new Table();
            while (true)
            {
                foreach (var player in hands)
                {
                    if (player.Value.Count > 0)
                    {
                        table.Add(player.Value[^1]);
                        player.Value.RemoveAt(player.Value.Count - 1);
                    }
                    else return new Tuple<Player, Hand>((Player)(((int)player.Key + 1) % 2), table);
                }
                Player? roundWinner = RoundWinner(table[^2], table[^1]);
                if (roundWinner != null) return new Tuple<Player, Hand>(roundWinner.Value, table);
            }
        }

        // Полный цикл игры (возвращается победивший игрок)
        // в процессе игры печатаются ходы
        internal static Player Game(Dictionary<Player, Hand> hands)
        {
            while (hands[Player.First].Count > 0 && hands[Player.Second].Count > 0)
            {
                Tuple<Player, Table> tup = Round(hands);

                foreach (var card in tup.Item2) 
                    hands[tup.Item1!].Add(card);
                Console.WriteLine(writeDeck(tup.Item2));
            }
            if (hands[Player.First].Count != 0)
                return Player.First;
            return Player.Second;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("First - Second - ...");
            Deck a = FullDeck();
            Dictionary<Player, Hand> k = Deal(a);
            var winner = Game(k);
            Console.WriteLine($"Победитель: {winner}");
        }
    }
}