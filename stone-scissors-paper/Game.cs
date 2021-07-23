using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace stone_scissors_paper
{
    public class Game
    {
        private readonly byte[] _key;
        private readonly List<string> _moves;

        public Game(List<string> moves)
        {
            _key = new byte[16];
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(_key);
            _moves = moves;
        }

        public void Run()
        {
            var computerMove = ComputerMove();
            int playerMove;
            do
            {
                ShowMenu();
                playerMove = PlayerMove();
            } while (playerMove == -1);
            if (playerMove == 0)
                return;
            ShowResult(computerMove, playerMove - 1);
        }

        private int ComputerMove()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(_moves.Count);
            var hmac256 = new HMACSHA256(_key);
            var binaryHmac = hmac256.ComputeHash(new ASCIIEncoding().GetBytes(_moves[index]));
            var stringHmac = BitConverter.ToString(binaryHmac).Replace("-", "");
            Console.WriteLine($"HMAC: {stringHmac}");
            return index;
        }

        private int PlayerMove()
        {
            Console.Write("Enter your move: ");
            if (!int.TryParse(Console.ReadLine(), out var playerMove) || playerMove > _moves.Count || playerMove < 0)
                return -1;
            return playerMove;
        }

        private void ShowMenu()
        {
            Console.WriteLine("Available moves:");
            for (var i = 0; i < _moves.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {_moves[i]}");
            }
            Console.WriteLine("0 - exit");
        }

        private void ShowResult(int computerMove, int playerMove)
        {
            Console.WriteLine($"Your move: {_moves[playerMove]}");
            Console.WriteLine($"Computer move: {_moves[computerMove]}");
            if (playerMove == computerMove)
                Console.WriteLine("Draw!");
            else
            {
                var threeNexItems = new List<string>();

                for (var i = 0; i < _moves.Count / 2; i++)
                {
                    if (++playerMove == _moves.Count)
                        playerMove = 0;

                    threeNexItems.Add(_moves[playerMove]);
                }
                Console.WriteLine(threeNexItems.FirstOrDefault(x => x == _moves[computerMove]) == null
                    ? "You win!"
                    : "You lose!");
            }
            var stringKey = BitConverter.ToString(_key).Replace("-", "");
            Console.WriteLine($"HMAC key: {stringKey}");
        }
    }
}