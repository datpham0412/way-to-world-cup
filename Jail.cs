using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Jail is a cell that keeps player from moving for 3 turns
    /// </summary>
    public class Jail : Cell
    {
        public Jail(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) { }
        public override string OnCellFunction(Player player)
        {
            base.OnCellFunction(player); // call their parents 
            player.TurnsInJail = 2;
            player.SameDice = 0; // When a player is in jail, the same dice number is doesnt matter
            return player.Name + " has been sent to the jail.";
        }
        public override string Description => "If the player cannot roll the same number twice" +
                        "\nor pay $100 to escape, they are locked" +
                        "\nup for two turns. If player  rolls the same dice \n3 times, player will also be sent to the jail.";
    }
}
