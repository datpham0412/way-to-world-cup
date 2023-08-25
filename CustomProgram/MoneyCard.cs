using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    // <summary>
    // This class define a card that has financial impact( ? From Mysterious Card )
    // </summary>
    public class MoneyCard : Card
    {
        public override void Activate(Player player, Board board)
        {
            int money = new Random().Next(-300, 301); // player can lost or get money in that range
            if (money < 0)
                Description = "Pay $" + (-money);
            else
                Description = "Receive $" + money;
            player.Money += money;
        }
    }
}
