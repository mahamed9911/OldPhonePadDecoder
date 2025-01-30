using System;
using System.Collections.Generic;
using System.Text;

public class OldPhonePadDecoder
{
    public static string OldPhonePad(string input)
    {
        if (string.IsNullOrEmpty(input) || !input.EndsWith("#"))
            throw new ArgumentException("Invalid input: Must end with '#' and not be empty.");
        
        Dictionary<char, string> keypad = new Dictionary<char, string>
        {
            {'2', "ABC"}, {'3', "DEF"}, {'4', "GHI"}, {'5', "JKL"}, {'6', "MNO"},
            {'7', "PQRS"}, {'8', "TUV"}, {'9', "WXYZ"}, {'0', " "}
        };
        
        StringBuilder output = new StringBuilder();
        char lastChar = '\0';
        int count = 0;
        
        foreach (char c in input)
        {
            if (c == '#') break;
            if (c == '*') // Handle backspace
            {
                if (count > 0)
                {
                    count = 0; // Remove last typed sequence 
                }
                else if (output.Length > 0)
                {
                    output.Length--; // Remove last  character Since * is like the back button on a keyboard
                }
                lastChar = '\0';
                continue;
            }
            if (c == ' ') // Space means you can commited the last sequence
            {
                if (lastChar != '\0' && keypad.ContainsKey(lastChar)) // check current character is a part of the Dictionary
                {
                    string letters = keypad[lastChar];
                    int index = (count - 1);
                    if (index < 0) index += letters.Length; // Handles negative values
                        output.Append(letters[index % letters.Length]);          
                    
                }
                lastChar = '\0';
                count = 0;
                continue;
            }
            
            if (c == lastChar)
                count++;
            else
            {
                if (lastChar != '\0' && keypad.ContainsKey(lastChar))
                {
                    string letters = keypad[lastChar];
                    output.Append(letters[(count - 1) % letters.Length]);
                }
                lastChar = c;
                count = 1;
            }
        }
        
        // Append the last recorded character sequence (if not backspaced away)
        if (lastChar != '\0' && keypad.ContainsKey(lastChar) && count > 0)
        {
            string letters = keypad[lastChar];
            output.Append(letters[(count - 1) % letters.Length]);
        }
        
        return output.ToString();
    }
    
    public static void Main()
    {
        Console.WriteLine(OldPhonePad("33#"));               // Output: E
        Console.WriteLine(OldPhonePad("227*#"));            // Output: B
        Console.WriteLine(OldPhonePad("4433555 555666#"));  // Output: HELLO
        Console.WriteLine(OldPhonePad("8 88777444666*664#")); // Output: TURING
    }
}