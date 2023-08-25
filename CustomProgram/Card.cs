using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Cards have special effect for player ( ? Card ) - can get money for player, take player's money or control the player's movement ( < 4 steps)
    /// </summary>
    public abstract class Card
    {
        private string _description;
        public Card()
        {
            _description = "";
        }
        public string Description
        {
            get => _description;
            set => _description = value; // get description property for the card
        }
        public abstract void Activate(Player player, Board board); // Activate the card 
    }
}
