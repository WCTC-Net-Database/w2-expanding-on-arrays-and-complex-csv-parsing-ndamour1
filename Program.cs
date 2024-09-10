using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;

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
                    Console.Write("Invalid input. Please enter a number.");
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
                        Console.WriteLine("Invalid choice. Please try again.");
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
                var parts = ParseCsvLine(line);
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
    
        // Method to parse CSV lines
        static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var currentPart = new System.Text.StringBuilder();
    
            foreach (char c in line)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes; // Toggle whether we are inside quotes
                }
                else if (c == ',' && !inQuotes)
                {
                    // If we hit a comma outside of quotes, it's a delimiter
                    result.Add(currentPart.ToString().Trim());
                    currentPart.Clear();
                }
                else
                {
                    // Append to the current field (inside quotes or not)
                    currentPart.Append(c);
                }
            }
    
            // Add the final part
            result.Add(currentPart.ToString().Trim());
            return result.ToArray();
        }
    
    
        // Method to write characters to file
        static void WriteCharactersToFile(List<Character> characters)
        {
            var lines = new List<string> { "Name,Class,Level,HP,Equipment" }; // Fix header spacing
            lines.AddRange(characters.Select(c =>
            {
                // Handle names with commas by re-quoting them
                string formattedName = c.name.Contains(",") ? $"\"{c.name}\"" : c.name;
    
                // Join equipment with '|'
                string formattedEquipment = string.Join("|", c.equipment);
    
                // Return properly formatted line
                return $"{formattedName},{c.characterClass},{c.level},{c.hitPoints},{formattedEquipment}";
            }));
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
            // variable to break while loop below
            bool notZero = false;
    
            // Input for character's name
            Console.Write("Enter your character's first name: ");
            string name = Console.ReadLine();
            if (name.StartsWith("\"") && name.EndsWith("\""))
            {
                name = name.Substring(1, name.Length - 1);
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
            Console.Write("Enter the number indexed to the character you want to level up.\n");
    
            var characters = ReadCharactersFromFile();
            for (int i = 0; i < characters.Count; i++) {
                Console.WriteLine($"{i + 1}. {characters[i].name} the {characters[i].characterClass}, Level {characters[i].level}");
            }
            int listNumber = int.Parse(Console.ReadLine()) - 1;
            Character chosen = characters[listNumber];
            int currLevel = chosen.level;
            int newLevel = 0;
    
            // Loop to make sure user inputs a number greater than chosen character's current level
            while (newLevel <= currLevel)
            {
                Console.Write($"You have chosen {chosen.name}.\nEnter your character's new level. It must be higher than their current level. ");
                newLevel = int.Parse(Console.ReadLine());
    
                if (newLevel > currLevel)
                {
                    Console.Write($"{chosen.name} is now level {newLevel} with {newLevel * 6} HP.");
                    characters[listNumber].level = newLevel;
                    characters[listNumber].hitPoints = newLevel * 6;
                    WriteCharactersToFile(characters);
                }
                else if (newLevel < chosen.level)
                {
                    Console.Write($"The number you typed is less than {currLevel}. Try again. ");
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
