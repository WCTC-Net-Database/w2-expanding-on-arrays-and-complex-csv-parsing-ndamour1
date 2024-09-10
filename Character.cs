using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W2_assignment_template
{
    internal class Character
    {
        public string name { get; set; }
        public string characterClass { get; set; }
        public int level { get; set; }
        public int hitPoints { get; set; }
        public string[] equipment { get; set; }
    
        public override string ToString()
        {
            if (name.Contains(", "))
            {
                int commaIndex = name.IndexOf(',');
                string firstName = name.Substring(commaIndex + 2);
                string lastName = name.Substring(0, commaIndex);
                name = $"{firstName} {lastName}";
            }
            string characterText = $"{name} the {characterClass}\nLevel: {level}\nHP: {hitPoints}\nEquipment: ";
            for (int i = 0; i < equipment.Length; i++) {
                if (i != equipment.Length - 1)
                {
                    characterText += $"{equipment[i]}, ";
                }
                else
                {
                    characterText += $"{equipment[i]}\n";
                }
            }
            return characterText;
        }
    }
}
