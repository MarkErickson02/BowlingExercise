using System;

namespace BowlingExercise
{
    
    class BowlingGame
    {
        private int totalScore;
        private int[] frameTotal;

        public BowlingGame()
        {
            totalScore = 0;
            frameTotal = new int[10];
        }

        public static Boolean IsSpare(char c)
        {
            if (c == '/')
            {
                return true;
            }
            return false;
        }

        public static Boolean IsStrike(char c)
        {
            if (c == 'x')
            {
                return true;
            }
            return false;
        }

        public static int CalculateFinalFrame(char[] frameResults)
        {
            int finalFrameTotal = 0;
            for (int i=19; i < 21; i++)
            {
                if (IsStrike(frameResults[i]))
                {
                    finalFrameTotal += 10;
                }
                if (Char.IsDigit(frameResults[i]))
                {
                    finalFrameTotal += (int)(frameResults[i] - '0');
                }

                if (IsSpare(frameResults[i]))
                {
                    finalFrameTotal = 10;
                }
            }
            
            return finalFrameTotal;
        }

        public static int HandleStrike(int i, char[] frameResults)
        {
            int frameScore = 10;
            int twoThrowsFound = 2;
            int counter = i + 1;
            while (twoThrowsFound > 0 && counter < frameResults.Length)
            {
                if (Char.IsDigit(frameResults[counter]) || IsSpare(frameResults[counter]) || IsStrike(frameResults[counter]))
                {
                    twoThrowsFound--;
                }
                counter++;
            }

            // There has not been two throws after a strike so the score cannot be calcualted yet.
            if (twoThrowsFound != 0)
            {
                return 0;
            }

            int j = i + 1;
            while (j < counter)
            {
                if (Char.IsDigit(frameResults[j]))
                {
                    frameScore += (int)(frameResults[j] - '0');
                }

                else if (IsSpare(frameResults[j]))
                {
                    frameScore += (10 - (frameResults[j + 1] - '0'));
                }
                else if (IsStrike(frameResults[j]))
                {
                    frameScore += 10;
                }
                j++;
            }

            return frameScore;
        }

        public static int HandleSpare(int i, char[] frameResults)
        {
            int frameScore = 10;

            // Handling case where no throw exists after spare.
            if (i + 1 > frameResults.Length - 1)
            {
                return 0;
            }

            if (i + 1 <= frameResults.Length - 1)
            {
                if (IsStrike(frameResults[i + 1]))
                {
                    frameScore += 10;
                }
                else if (Char.IsDigit(frameResults[i + 1]))
                {
                    frameScore += (int)(frameResults[i + 1] - '0');
                }
            }
            
            return frameScore;
        }

        public int CalculateScore(char[] frameResults)
        {
            int throwNumber = 0;
            int frameScore = 0;
            int limitToNineFrames = frameResults.Length-1;
            bool tenthFrameExists = false;
            if (limitToNineFrames > 18)
            {
                tenthFrameExists = true;
                limitToNineFrames = 18;
            }
            for (int i = 0 ; i <= limitToNineFrames ; i++)
            {
                if (Char.IsDigit(frameResults[i]))
                {
                    frameScore += (int) (frameResults[i] - '0');
                }

                if (IsSpare(frameResults[i]))
                {
                    frameScore = HandleSpare(i, frameResults);
                }
                                
                if (IsStrike(frameResults[i]))
                {
                    frameScore += HandleStrike(i, frameResults);
                }
                                
                if (throwNumber % 2 == 1)
                {
                   
                    frameTotal[throwNumber/2] = frameScore + totalScore;
                    totalScore += frameScore;
                    frameScore = 0;
                }
               
                throwNumber++;
                
            }
            if (tenthFrameExists)
            {
                int tenthFrameTotal = CalculateFinalFrame(frameResults);
                frameTotal[9] = tenthFrameTotal + totalScore;
                totalScore += tenthFrameTotal;
            }
            
            return totalScore;
        }

        public char[] ReadInput()
        {
            Console.Write("Please enter your frame scores using 0-9, / for spares and x for strikes, seperated by spaces: ");
            String input = Console.ReadLine().Replace(" ", "").ToLower();
            char[] splitInput = input.ToCharArray();

            if (splitInput.Length > 21)
            {
                Console.WriteLine("Input is larger than a standard 10 pin bowling game. Will calculate score until 21st character.");

            }

            for (int i=0; i<splitInput.Length; i++)
            {
                if ( (! Char.IsDigit(splitInput[i])) && (! IsSpare(splitInput[i])) && (! IsStrike(splitInput[i])) && splitInput[i] != '-')
                {
                    Console.WriteLine("The character: " + splitInput[i] + " is not an accepted character");
                    splitInput[0] = 'e';
                    return splitInput;
                }
            }
            return splitInput;

        }

        static void Main(string[] args)
        {
            //Char[] frameResults = {'8','/','5','4', '9', '0', 'x', '-', 'x', '-', '5', '/', '5', '3', '6', '3', '9', '/', '9', '/', 'x' };
            BowlingGame game = new BowlingGame();
            char[] frameResults = game.ReadInput();

            // Edge case handling for no given input
            if (frameResults.Length == 0)
            {
                Console.WriteLine("Your score is 0");
                return;
            }

            // Error handling for unknown character
            if (frameResults[0] == 'e')
            {
                return;
            }
            
            int total = game.CalculateScore(frameResults);
            Console.WriteLine("The total score was " + total);
            
            for (int i=0; i<game.frameTotal.Length; i++)
            {
                Console.WriteLine("Frame Number: " + (i+1) + " Frame Score: " + game.frameTotal[i]);
            }
            
        }
    }
}
