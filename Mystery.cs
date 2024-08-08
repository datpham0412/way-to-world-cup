using System;
using System.Collections.Generic;
using System.Reflection;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Mystery represent for player picking the ( ? Card ) up
    /// </summary>
    public class Mystery : Cell
    {
        private static Dictionary<Type, Card> _cards = new(); // Dictionary for all available cards
        private Card _chooseCard; // the card that is chosen
        public static void AddCard(Card card) => _cards.Add(card.GetType(), card); // Add more cards 

        public static void AddCard<Type>() where Type : Card => AddCard(Activator.CreateInstance<Type>()); //  Add more cards based on the type

        public Mystery(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) => _chooseCard = null;
        public override string Description => "Get a mysterious card";
        public override string OnCellFunction(Player player)
        {
            string result = player.Name + " chooses a card\n"; 
            if (_cards.Count <= 0)
            {
                return result;
            }
            // get random cards 
            int coordinate = player.Coordinate; //  the position of the player at that time 
            int cardNum = new Random().Next(0, _cards.Count); // get a random card 
            List<Card> cardList = new List<Card>(_cards.Values); 
            _chooseCard = cardList[cardNum];  
            _chooseCard.Activate(player, _board); // turn on the card's effect
            result += _chooseCard.Description;
            _chooseCard.Description = "";
            _chooseCard = null;
            // if the player is not in the original position
            if (player.Coordinate != coordinate)
                result += "\n\n" + _board.FindCell(player.Coordinate).OnCellFunction(player);
            else // if the player step on the original position so they will lose their turn
                player.IsInTurn = false;
            return result;
        }
    }
}
