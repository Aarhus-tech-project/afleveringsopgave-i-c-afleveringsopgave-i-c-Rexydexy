namespace Assignment
{
    public class TicTacToe
    {
        private char[] board = new char[9];
        private int[] scoreBoard = new int[2];
        private string[] playerNames = new string[2];

        public void Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool running = true;
            Console.Clear();
            PrintTitle();

            while (running)
            {
                ShowMainMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": SetupAndPlayGame(); break;
                    case "2": ShowScoreBoard(); break;
                    case "3": ShowRules(); break;
                    case "4":
                        running = false;
                        Console.WriteLine("\nTak for spillet! Farvel\n");
                        break;
                    default:
                        PrintError("Ugyldigt valg. Vælg venligst 1, 2, 3 eller 4.");
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- HOVEDMENU ---");
            Console.ResetColor();
            Console.WriteLine("  1. Spil et nyt spil");
            Console.WriteLine("  2. Se pointtavle");
            Console.WriteLine("  3. Regler");
            Console.WriteLine("  4. Afslut");
            Console.Write("\nDit valg: ");
        }

        private void SetupAndPlayGame()
        {
            Console.Clear();
            PrintTitle();

            playerNames[0] = GetPlayerName("Spiller 1 (X)");
            playerNames[1] = GetPlayerName("Spiller 2 (O)");

            bool playAgain = true;
            while (playAgain)
            {
                PlayOneGame();

                Console.Write("\nVil I spille igen? (j/n): ");
                string? again = Console.ReadLine()?.Trim().ToLower();
                while (again != "j" && again != "n")
                {
                    PrintError("Skriv 'j' for ja eller 'n' for nej.");
                    Console.Write("Vil I spille igen? (j/n): ");
                    again = Console.ReadLine()?.Trim().ToLower();
                }
                playAgain = (again == "j");
            }
        }

        private string GetPlayerName(string prompt)
        {
            string? name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write($"Indtast navn for {prompt}: ");
                name = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    PrintError("Navn må ikke være tomt. Prøv igen.");
            }
            return name;
        }

        private void PlayOneGame()
        {
            ResetBoard();
            int currentPlayer = 0;
            char[] symbols = { 'X', 'O' };
            bool gameOver = false;

            Console.Clear();
            PrintTitle();

            while (!gameOver)
            {
                DrawBoard();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{playerNames[currentPlayer]}'s tur ({symbols[currentPlayer]})");
                Console.ResetColor();

                int move = GetPlayerMove();
                board[move] = symbols[currentPlayer];

                if (CheckWin(symbols[currentPlayer]))
                {
                    DrawBoard();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\nTillykke! {playerNames[currentPlayer]} vinder!");
                    Console.ResetColor();
                    scoreBoard[currentPlayer]++;
                    gameOver = true;
                }
                else if (IsBoardFull())
                {
                    DrawBoard();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\nUafgjort! Brættet er fuldt.");
                    Console.ResetColor();
                    gameOver = true;
                }
                else
                {
                    currentPlayer = 1 - currentPlayer;
                }
            }
        }

        private int GetPlayerMove()
        {
            while (true)
            {
                Console.Write("Vælg et felt (1-9): ");
                string? input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int number))
                {
                    PrintError("Ugyldigt input. Skriv et tal mellem 1 og 9.");
                    continue;
                }

                int index = number - 1;
                if (index < 0 || index > 8)
                {
                    PrintError("Tallet skal være mellem 1 og 9.");
                    continue;
                }

                if (board[index] != ' ')
                {
                    PrintError("Det felt er allerede taget. vælg et andet felt.");
                    continue;
                }

                return index;
            }
        }

        private void ResetBoard()
        {
            for (int i = 0; i < board.Length; i++)
                board[i] = ' ';
        }

        private bool IsBoardFull()
        {
            foreach (char cell in board)
                if (cell == ' ') return false;
            return true;
        }

        private bool CheckWin(char symbol)
        {
            int[,] wins = {
                { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
                { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
                { 0, 4, 8 }, { 2, 4, 6 }
            };

            for (int i = 0; i < wins.GetLength(0); i++)
                if (board[wins[i, 0]] == symbol &&
                    board[wins[i, 1]] == symbol &&
                    board[wins[i, 2]] == symbol)
                    return true;

            return false;
        }

private void DrawBoard()
{
    Console.Clear();
    PrintTitle();
    Console.WriteLine("  Brættet:");
    Console.WriteLine();

    void PrintCell(int i)
    {
        if (board[i] == 'X')
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" X ");
            Console.ResetColor();
        }
        else if (board[i] == 'O')
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" O ");
            Console.ResetColor();
        }
        else
        {
            Console.Write($" {i + 1} ");
        }
    }

    PrintCell(0); Console.Write("|"); PrintCell(1); Console.Write("|"); PrintCell(2); Console.WriteLine();
    Console.WriteLine("---+---+---");
    PrintCell(3); Console.Write("|"); PrintCell(4); Console.Write("|"); PrintCell(5); Console.WriteLine();
    Console.WriteLine("---+---+---");
    PrintCell(6); Console.Write("|"); PrintCell(7); Console.Write("|"); PrintCell(8); Console.WriteLine();
    Console.WriteLine();
}

        private void ShowScoreBoard()
        {
            Console.Clear();
            PrintTitle();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- POINTTAVLE ---");
            Console.ResetColor();
            Console.WriteLine();

            if (string.IsNullOrEmpty(playerNames[0]))
            {
                Console.WriteLine("  Ingen spil er spillet endnu.");
            }
            else
            {
                Console.WriteLine($"  {playerNames[0]} (X):  {scoreBoard[0]} sejre");
                Console.WriteLine($"  {playerNames[1]} (O):  {scoreBoard[1]} sejre");

                if (scoreBoard[0] > scoreBoard[1])
                    Console.WriteLine($"\n  {playerNames[0]} leder!");
                else if (scoreBoard[1] > scoreBoard[0])
                    Console.WriteLine($"\n  {playerNames[1]} leder!");
                else
                    Console.WriteLine("\n  Uafgjort!");
            }

            Console.WriteLine("\nTryk på en tast for at vende tilbage...");
            Console.ReadKey();
        }

        private void ShowRules()
        {
            Console.Clear();
            PrintTitle();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- REGLER ---");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("  Tic Tac Toe spilles på et 3x3 bræt.");
            Console.WriteLine("  To spillere skiftes til at sætte X eller O.");
            Console.WriteLine("  Den foerste med 3 i traek vinder.");
            Console.WriteLine("  Er brættet fuldt uden vinder, er det uafgjort.");
            Console.WriteLine();
            Console.WriteLine("  Feltnumre:");
            Console.WriteLine("  1 | 2 | 3");
            Console.WriteLine("  --+---+--");
            Console.WriteLine("  4 | 5 | 6");
            Console.WriteLine("  --+---+--");
            Console.WriteLine("  7 | 8 | 9");
            Console.WriteLine("\nTryk på en tast for at vende tilbage...");
            Console.ReadKey();
        }

        private void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================");
            Console.WriteLine("        TIC TAC TOE           ");
            Console.WriteLine("==============================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ! {message}");
            Console.ResetColor();
        }
    }
}
