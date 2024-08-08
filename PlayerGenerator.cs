using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Custom_Program
{
    class PlayersGenerator
    {


        // Generates 2 to 4 players and returns them in a list
        public List<String> Execute() // Can custom the amount 
        {
            List<String> players = new();
            int count = GetPlayerCount();
            for (int i = 0; i < count; i++)
            {
                players.Add(GetPlayer(i + 1));
            }
            return players;
        }

        // Asks user input for number of players (validates for 2-4 players)
        private int GetPlayerCount()
        {
            int i;
            do
            {
                Console.Clear();
                Console.Write("Enter number of players (2-4): ");
                int.TryParse(Console.ReadLine().Trim(), out i);
            } while (i is > 4 or < 2);
            return i;
        }

        // request for each player's name from the user (validates for non-empty strings)
        private String GetPlayer(int playerCount)
        {
            string name;
            do
            {
                Console.Clear();
                Console.Write($"Enter Player {playerCount}'s Name: ");
                name = Console.ReadLine().Trim();
            } while (name == "");
            return name;
        }
    }
}