using System;
using System.Linq;

namespace ConsoleApp1
{
    class NimGame
    {

        public NimGame() { }
        
        private  readonly Random _generateRandom = new Random();
        public struct gameBoard
        {
            public int[,] _gameBoard { get; set; } // retrieving and setting 2d array values for the rows and columns for the board
            public int _objects { get; set; } // getting and setting random values for game objects

            public int _rowSelected { get; set; } // what row the player has selected
            public int _amountOfPiecesSelected { get; set; } // how many elements in the game of nim you want to remove

            public int[] _zeroedRows { get; set; } // tracks the rows that have 0 in them so users can't access them anymore

            public bool _rowCheck { get; set; } // checks if row has no values

            public bool _gameover { get; set; } //Gameover status 
        }



        public  gameBoard GB = new gameBoard(); // declaring a new gamenboard

        public  void Begin()
        {

            /*
            Run this in Program.CS 
            eg. 
            NimGame Nim = new NimGame();

            Nim.Begin();
             
             This will start the game, the code does the rest 
             
             */


            GB._gameover = false;
            int player = 0;

            startGame(); // runs start of game procedure

            while (GB._gameover == false) //gameover flag
            {
                for (player = 1; player <= 2; player++) // iterating through players
                {
                    WriteToConsole(player);
                    Moving(); //runs when the players want to move
                }

            }

            Console.Clear();
            Console.WriteLine("Player " + (player - 1) + " WON!\n---------------------------- -\nRows | Column\n----------------------------"); //Player who has won

            for (int j = 0; j != GB._gameBoard.GetLength(0); j++)
            {
                WriteGameConsole(j);
            }

            Console.ReadKey();
        }

        private void WriteToConsole(int i) //Simple code to write to console 
        {
            Console.Clear();
            Console.WriteLine("Player " + (i) + " turn\n---------------------------- -\nRows | Column\n----------------------------"); // which player is currently playing #TODO, BE CHANGED TO COMPUTER

            for (int j = 0; j != GB._gameBoard.GetLength(0); j++)
            {
                WriteGameConsole(j);
            }
        }


        public void startGame() // starting game and creating random rows and playing objects
        {
            int run = RandomNumber(2, 5); // generating random amount of columns 
            GB._gameBoard = new int[run, 2]; // declaring amount of columns in the game
            GB._zeroedRows = new int[run]; // this will keep track of rows with no value

            for (int i = 0; i != run; i++)
            {
                GB._gameBoard[i, 0] = i + 1; // counting through cols
                GB._gameBoard[i, 1] = (2 * (RandomNumber(1, 6)) + 1); // adding random amount of game objects to game with formula of 2N + 1
            }
        }

        private void WriteGameConsole(int row)
        {
            for (int j = 0; j < GB._gameBoard.GetLength(1); j++) Console.Write(string.Format("|{0}| ", GB._gameBoard[row, j])); // printing in in matrix 
            Console.Write(Environment.NewLine);
            Console.WriteLine("----------------------------");
        }

        public void Moving() //#TODO WHEN AN ELEMNT IS 0 THE PLAYER CAN STILL SELECT ROW 
        {

            Console.Write("\nPick a row you want to make a move at.\nRow: ");

            GB._rowSelected = ParseInt(0, 1000); // values of 0, 1000 choosen as we are only testing if the next character is a int

            GB._rowSelected = NumberCheck(0, GB._gameBoard.GetLength(0) + 1, GB._rowSelected - 1); // plus one because range is min inclusive and max is the limit

            while (true)
            {
                if (CheckRow(GB._rowSelected))//checkin if row has any sticks or objects in it
                {
                    Console.Write("\nThe '{0}' row has no numbers in it. Please select a row with integers in it.\nPlease Check your entered integer and try again: ", GB._rowSelected + 1);//error meesages if row has no integers                                   
                    GB._rowSelected = (ParseInt(0, GB._gameBoard.GetLength(0)) - 1);
                }
                else break;
            }


            Console.Write("\nPick how man elements you want to pick up (remove).\nElements: ");

            GB._amountOfPiecesSelected = ParseInt(0, 1000); // values of 0, 1000 choosen as we are only testing if the next character is a int

            GB._amountOfPiecesSelected = NumberCheck(1, GB._gameBoard[GB._rowSelected, 1], GB._amountOfPiecesSelected);

            GB._gameBoard[GB._rowSelected, 1] -= GB._amountOfPiecesSelected;

            int rowEmptyCount = 0; //row empty count
            for (int i = 0; i < GB._gameBoard.GetLength(0); i++) // CHECKING IF GAME IS OVER IF ALL THE INTEGERS IN THE ARRAY ARE 0
            {
                if (GB._gameBoard[i, 1] == 0) // checking all rows that have 0 in them 
                {
                    rowEmptyCount++; // adding to a count to check against length 
                    if (rowEmptyCount == GB._gameBoard.GetLength(0)) GB._gameover = true;
                }


            }
        }

        private int NumberCheck(int min, int max, int checkableNumber)
        {

            GB._rowCheck = false;

            while (true)
            {
                if (Enumerable.Range(min, max).Contains(checkableNumber)) return checkableNumber; // checking if number is in range
                else
                {
                    Console.Write("\n'{0}' is not within the given range, min: '{1}' or max: '{2}'.\nPlease Check your entered integer and try again: ", checkableNumber + 1, min, max); //error meesages for incorrect integers 
                }


                checkableNumber = ParseInt(min, max); // iif number isn't in range it makes users input it again
            }
        }

        public int ParseInt(int min, int max) // checking if entered string is an integer
        {
            //local variables
            int checkableNumber = 0;
            string parseCheckableNumber;
            bool tryCheck = false;


            while (tryCheck == false)
            {
                parseCheckableNumber = Console.ReadLine(); //takes a string from console
                tryCheck = int.TryParse(parseCheckableNumber, out checkableNumber); //makes sure that element entered is a integer
                if (tryCheck == false)
                {
                    Console.Write("\n'{0}' is not an Integer or is greater then '{1}' or less then '{2}'.\nPlease Check your entered integer and try again: ", parseCheckableNumber, max, min); //error meesages for incorrect integers 
                }
            }
            return checkableNumber;
        }

        public bool CheckRow(int row) // checking if row has sticks or objects avaliable 
        {
            for (int i = 0; i < GB._gameBoard.GetLength(0); i++)
            {
                if (GB._gameBoard[row, 1].Equals(0)) //Lazy way of checking if row is 0zed 
                {
                    GB._zeroedRows[i] = row - 1;
                    return true;
                }
            }

            return false;
        }

        public int RandomNumber(int min, int max)
        {
            return _generateRandom.Next(min, max); //generating a random number in a range
        }
    }
}
