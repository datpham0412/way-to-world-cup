using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Tax is a cell which forces player to pay rents
    /// </summary>
    public class Tax : Cell
    {
        public Tax(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) { }
        public override string OnCellFunction(Player player)
        {
            int tax = player.TotalMoney * 10 / 100;
            if (player.Money >= tax)
            {
                // player pays the tax
                player.Pay(null, tax);
                player.EndTurn();
                return player.Name + " has paid " + tax + "$";
            }
            // player's money is less than tax so they have to sell property to pay the loan
            player.SellPlan(null, tax);
            return "You have to sell to pay the loan\nPay Total: " + tax + "$\nSell Total: " + player.SellTotal + "$";
        }
        public override string Description
        {
            get
            {
                return "Player has to pay 10% of total money";
            }
        }
    }
}
