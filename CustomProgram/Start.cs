using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// The starting line of the game
    /// </summary>
    public class Start : Cell
    {
        public Start(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) { }
        public override string Description => "Starting line\nPass here to earn $500";
        public override string OnCellFunction(Player player) => base.OnCellFunction(player) + player.Name + " has reached at the Starting point";
    }
}
