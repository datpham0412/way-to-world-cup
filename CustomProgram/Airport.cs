using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Airport is a regular cell
    /// </summary>
    public class Airport : Cell
    {
        public Airport(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) { }
        public override string Description => "Airport to go through cities, no need to pay";
        public override string OnCellFunction(Player player) => base.OnCellFunction(player) + player.Name + " has arrived at the Airport";
    }
}
