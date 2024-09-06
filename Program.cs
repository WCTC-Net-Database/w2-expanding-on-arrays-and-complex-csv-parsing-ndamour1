using System;
using System.Collections.Generic;
using System.IO;

namespace W2_assignment_template
{
    public class Program
    {
        static string filePath = "input.csv";

        static void Main(string[] args)
        {
            // Main loop
            while (true)
            {
                // Main menu
                Console.Clear();
                Console.Write("Select what you want to do.\n1. Display Characters\n2. Add Character\n3. Level Up Character\n4. Exit\n");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Write("Invalid input. Please enter a number.\n");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DisplayCharacters();
                        break;
                    case 2:
                        AddCharacter();
                        break;
                    case 3:
                        LevelUpCharacter();
                        break;
                    case 4:
                        Console.Write("See you again!");
                        return;
                    default:
                        break;

                }

                // Pause to allow the user to see the result before the menu is shown again
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }

        // Method to read characters from file
        static List<Character> ReadCharactersFromFile()
        {
            var characters = new List<Character>();

            if (!File.Exists(filePath))
                return characters;

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) // Skip header
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    characters.Add(new Character
                    {
                        name = parts[0],
                        characterClass = parts[1],
                        level = int.Parse(parts[2]),
                        hitPoints = int.Parse(parts[3]),
                        equipment = parts[4].Split('|')
                    });
                }
            }
            return characters;
        }

        // Method to write characters to file
        static void WriteCharactersToFile(List<Character> characters)
        {
            var lines = new List<string> { "Name, Class, Level, HP, Equipment" };
            lines.AddRange(characters.Select(c => $"{c.name}, {c.characterClass}, {c.level}, {c.hitPoints}, {c.equipment}"));
            File.WriteAllLines(filePath, lines);
        }

        // Method to display characters
        static void DisplayCharacters()
        {
            var characters = ReadCharactersFromFile();
            if (characters.Count == 0)
            {
                Console.WriteLine("No characters found.");
            }
            else
            {
                foreach (var character in characters)
                {
                    Console.WriteLine(character);
                }
            }
        }

        // Method to add characters
        static void AddCharacter()
        {
            // name variable
            string name;
            
            // variable to break while loop below
            bool notZero = false;
            
            // Input for character's name
            Console.Write("Enter your character's first name: ");
            string firstName = Console.ReadLine();
            if (firstName.StartsWith("\"") && firstName.EndsWith("\""))
            {
                firstName = firstName.Substring(1, firstName.Length - 1);
            }
            else if (firstName.StartsWith("\"") && !firstName.EndsWith("\""))
            {
                firstName = firstName.Substring(1, firstName.Length);
            }
            else if (!firstName.StartsWith("\"") && firstName.EndsWith("\""))
            {
                firstName = firstName.Substring(0, firstName.Length - 1);
            }
            
            Console.Write("Enter your character's last name (if applicable): ");
            string lastName = Console.ReadLine();
            if (lastName.StartsWith("\"") && lastName.EndsWith("\""))
            {
                lastName = lastName.Substring(1, lastName.Length - 1);
            }
            else if (lastName.StartsWith("\"") && !lastName.EndsWith("\""))
            {
                lastName = lastName.Substring(1, lastName.Length);
            }
            else if (!lastName.StartsWith("\"") && lastName.EndsWith("\""))
            {
                lastName = lastName.Substring(0, lastName.Length - 1);
            }
            
            if (lastName == null)
            {
                name = firstName;
            } else
            {
                name = $"{firstName} {lastName}"; 
            }

            // Input for character's class
            Console.Write("Enter your character's class: ");
            string characterClass = Console.ReadLine();

            // While loop to make sure level is greater than 0
            int level = 0;
            while (!notZero)
            {
                Console.Write("Enter your character's level. It must be 1 or higher. ");
                level = int.Parse(Console.ReadLine());

                if (level <= 0)
                {
                    Console.Write("The number you entered is less than 1. Try again. ");
                    level = int.Parse(Console.ReadLine());
                }
                else
                {
                    notZero = true;
                }
            }

            // Calculation for hit points
            int hitPoints = level * 6;

            // Input for character's equipment
            Console.Write("Enter your character's equipment (separate items with a '|'): ");
            string[] equipment = Console.ReadLine().Split('|');

            // Displays the user's input for the character
            var characters = ReadCharactersFromFile();
            characters.Add(new Character { name = name, characterClass = characterClass, level = level, hitPoints = hitPoints, equipment = equipment });
            WriteCharactersToFile(characters);
            Console.WriteLine($"Welcome, {name} the {characterClass}! You are level {level} with {hitPoints} HP and your equipment includes: {string.Join(", ", equipment)}.");
        }

        // Method for leveling up character
        public static void LevelUpCharacter()
        {
            Console.Write("Enter the number indexed to the character you want to level up.");

            var characters = ReadCharactersFromFile();
            Console.Write("\n");
            int listNumber = int.Parse(Console.ReadLine()) - 1;
            Character chosen = characters[listNumber];
            int newLevel = 0;

            // Loop to make sure user inputs a number greater than chosen character's current level
            while (newLevel <= chosen.level)
            {
                Console.Write("Enter your character's new level. It must be higher than their current level. ");
                newLevel = int.Parse(Console.ReadLine());

                if (newLevel > chosen.level)
                {
                    WriteCharactersToFile(characters);
                    Console.Write($"{chosen.name} is now level {newLevel} with {newLevel * 6} HP.");
                    characters[listNumber].level = newLevel;
                    characters[listNumber].hitPoints = newLevel * 6;
                }
                else if (newLevel < chosen.level)
                {
                    Console.Write($"The number you typed is less than {chosen.level}. Try again. ");
                    newLevel = int.Parse(Console.ReadLine());
                }
                else if (newLevel == chosen.level)
                {
                    Console.Write($"{newLevel} is {chosen.name}'s current level. Try again. ");
                    newLevel = int.Parse(Console.ReadLine());
                }
            }
        }
    }
}
