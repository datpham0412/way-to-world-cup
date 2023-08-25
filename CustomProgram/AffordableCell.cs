using System;
using System.Collections.Generic;
using System.IO;
using SplashKitSDK;

/// <summary>
/// The cell that player able to purchase
/// </summary>
/// 

namespace Custom_Program
{

    public abstract class AffordableCell : Cell
    {
        private Player _belongTo; // cell belongs to owner
        protected int[] _rentCosts; // renting cost
        protected int _cost; // original cost
        public AffordableCell(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) => _belongTo = null;
        public int Cost => _cost;
        public Player BelongTo
        {
            get => _belongTo;
            set => _belongTo = value;
        }
        public virtual int Value => _cost; // get the true value of the city
        public virtual int ActualRent => 0; // get the actual rent of the city
        public virtual void Reset() => _belongTo = null; // reset the city to unowned city
        public override string OnCellFunction(Player player) // cell's function when player step on that cell
        {
            if (BelongTo != null) // if the area does not belong to any player
            {
                if (BelongTo != player) // belong to other player, not the player that playing at that moment 
                {
                    // if the area belong to other player, the player step on that cell gotta pay to the owner
                    if (player.Money < ActualRent)
                    {
                        player.IsSelling = true;
                        // player has to sell properties to pay the rent
                        if (player.TotalMoney >= ActualRent)
                        {
                            player.SellPlan(BelongTo, ActualRent);
                            return "You have to sell to pay the rent\nPay Total: " + ActualRent + "$\nSell Total: " + player.SellTotal + "$";
                        }
                        // player has to sell all and go bankrupt
                        player.SellAll();
                        int payment = player.Money;
                        player.Pay(BelongTo, payment);
                        player.EndTurn();
                        player.IsBankrupt = true;
                        return player.Name + " has paid " + payment + "$\n" + player.Name + " is bankrupt!";
                    }
                    // player pays the rent
                    player.Pay(BelongTo, ActualRent);
                    player.EndTurn();
                    return player.Name + " has paid " + ActualRent + "$";
                }
            }
            else
            {
                if (player.Money >= _cost)
                {
                    // player able to Purchase this area
                    player.PurchasePlan(this);
                    return "Do you want to purchase this estate?\nCost: " + _cost + "$";
                }
                // cost go beyond player's affordable
                player.EndTurn();
                return player.Name + " does not have enough money to purchase this area.";
            }
            return "";
        }
        public override void Draw()
        {
            base.Draw(); // draw base on the virtual method
            DrawName();
        }
        // draw the city's name to the cell
        private void DrawName()
        {
            int nameWidth = SplashKit.TextWidth(Name, "CellFont", 13);
            SplashKit.DrawTextOnBitmap(_image, Name, Color.Black, "CellFont", 13, (_image.Width - nameWidth) / 2, 1); // draw the city's name 
        }
    }
}
