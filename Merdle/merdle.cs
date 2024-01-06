using System;
using System.IO;
using System.Reflection.Emit;
using Microsoft.VisualBasic;

string solutionsPath = @".\solutions.txt";//list of possible solutions
string dictionaryPath = @".\dictionary.txt";//list of valid words
string solution;
int defaultWordLength = 5;
Card[] guessCards, solutionCards;
bool correctAnswer = false;
Random random = new Random();
int numberOfGuesses = 6;
int guessCounter;


List<string> generateWordList(int wordLength, string filePath)
{
    string[] fileAsStringArray;
    List<string> listOfWords = new List<string>();
    if (!File.Exists(filePath))
    {
        throw new ArgumentException("FILE READ ERROR- could not open: " + filePath);
    }
    else
    {
        fileAsStringArray = File.ReadAllLines(filePath);
    }
    foreach (string word in fileAsStringArray)
    {
        if (word.Length == wordLength)
        {
            listOfWords.Add(word);
        }
    }
    return listOfWords;
}

string takeGuess(int wordLength, List<string> dictionary)
{
    bool validGuess = false;
    string guess;
    do
    {
        Console.Write("Guess[" + guessCounter + "/" + numberOfGuesses + "]: ");
        guess = Console.ReadLine()!;
        if (guess.Length == wordLength && dictionary.Contains(guess))
        {
            validGuess = true;
        }
        else
        {
            Console.WriteLine("invalid guess");
        }
    }while (!validGuess);
    return guess;
}

void checkCards(Card[] solutionCard, Card[] cardToCheck)
{
    foreach (Card card in cardToCheck)
    {
        foreach (Card solCard in solutionCard)
        {
            if (card.Character == solCard.Character)
            {
                card.cardColor = ConsoleColor.DarkYellow;//if cards are matching
                if (Array.IndexOf(cardToCheck, card) == Array.IndexOf(solutionCard, solCard))// if the characters are matching and in same place
                {
                    card.isInRightPlace = true;
                }
            }
        }
        if (card.isInRightPlace == true)
        {
            card.cardColor = ConsoleColor.DarkGreen;
        }
    }
}

Card[] stringToCards(string String, int wordLength)
{
    Card[] CardArray = new Card[wordLength];//initialise array
    for (int i = 0; i < wordLength; i++)//initialise array elements
    {
        CardArray[i] = new Card();
    }
    for (int i = 0; i < wordLength; i++)
    {
        CardArray[i].Character = String[i];
    }
    return CardArray;
};

void printCards(Card[] cards)
{
    foreach (Card card in cards)
    {
        Console.BackgroundColor = card.cardColor;
        Console.Write(card.Character);
    }
    Console.Write("\n");
    Console.BackgroundColor = ConsoleColor.Black;
}

int getWordLength()
{
    string lengthInput;
    bool isValid = false;
    Console.Write("word length [1-8]: ");
    lengthInput = Console.ReadLine()!;
    isValid = int.TryParse(lengthInput, out int value);
    if (0 < value && value < 9)
    {
        return value;
    }
    else
        Console.WriteLine("invalid length, defaulting to 5");
    return defaultWordLength;

}


main:
guessCounter = 1;
int wordLength = getWordLength();
List<string> solutionsList = generateWordList(wordLength, solutionsPath);// possible answers
if (solutionsList.Count == 0)
{
    Console.WriteLine("no words of given length exist in solution.txt");
    goto main;
}
List<string> Dictionary = generateWordList(wordLength, dictionaryPath);//valid words
if (Dictionary.Count == 0)
{
    Console.WriteLine("no words of given length exist in dictionary.txt");
    goto main;
}
//TODO - condense the two above checks into the generateWordList function
solution = solutionsList[random.Next(0, solutionsList.Count)];

do
{
    correctAnswer = true;
    string guess = takeGuess(wordLength, Dictionary);//checks for valid word
    guessCards = stringToCards(guess, wordLength);
    solutionCards = stringToCards(solution, wordLength);
    checkCards(solutionCards, guessCards);
    printCards(guessCards);

    foreach (Card card in guessCards)
    {
        if (!card.isInRightPlace)
            correctAnswer = false;
    }
    guessCounter++;
} while (!correctAnswer && guessCounter < 7);

if (correctAnswer)
{
    Console.WriteLine("YOU WIN!");
}
else
{
    Console.Write("Unlucky! the answer was: ");
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.Write(solution);
    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine();
}

Console.Write("play again [Y/n]");
char key = Console.ReadKey().KeyChar;
Console.WriteLine();
if (key == 'n' || key == 'N')
{
    return;
}
else
{
    goto main;
}


public class Card
{
    public char Character;
    public ConsoleColor cardColor = ConsoleColor.Black;
    public bool isInRightPlace = false;
}