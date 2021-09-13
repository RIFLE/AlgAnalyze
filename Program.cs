using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace AlgAnalyze
{
    public static class Extensions
    {
        public static string Filter(this string str, char charToRemove)
        {
            return String.Concat(str.Split(charToRemove));
        }
        public static string Filter(this string str, List<char> charsToRemove)
        {
            return String.Concat(str.Split(charsToRemove.ToArray()));
        }
        
    }
    class Program
    {
        static string GetLine(string fileName, int line)
        {
            using (var sr = new StreamReader(fileName))
            {
                sr.ReadLine();  //To ignore the table title
                for (int i=1; i<line; i++)   
                {
                    if(sr.EndOfStream)
                        { return "DigitIndex is out of a dataspace"; }
                    else
                        sr.ReadLine();
                }
                if(sr.EndOfStream)
                    { return "DigitIndex is out of a dataspace"; }
                else
                    return sr.ReadLine();
            }
        }
        static string GetLinesWithPattern(string fileName, string pattern)
        {
            using (var sr = new StreamReader(fileName))
            {
                string _containerStr = "", _temp = "";
                sr.ReadLine();  //To ignore the table title
                while(!sr.EndOfStream)
                {
                    if((_temp = sr.ReadLine()).Contains(pattern))
                    {
                        _containerStr += _temp + "\n";
                    }
                }
                return _containerStr;
            }
        }
        static void GetWordsList(ref string buffer, ref List<string> wordsList, bool state = false)
        {
            string _temp = "";
            if(state)
            {
                for(int i=0; i<buffer.Length; i++)
                {
                    if(buffer[i] == ' ') break;
                    else _temp += buffer[i];
                }
                wordsList.Add(new string(_temp));
                _temp = "";
                for(int i=0; i<buffer.Length; i++)
                {
                    if(buffer[i] == '\'')
                    {
                        i++;
                        for(; i<=buffer.Length; i++)
                        {
                            if(buffer[i] == '\'') {wordsList.Add(new string(_temp)); break;}
                            else _temp += buffer[i];
                        }
                        _temp = "";
                    }
                }
            }
            else
            {
                for(int i=0; i<=buffer.Length; i++)
                {
                    if(i != buffer.Length && buffer[i] != ' ')
                    {
                        _temp += buffer[i];
                    }
                    else
                    {
                        wordsList.Add(new string(_temp));
                        _temp = "";
                    }
                }
            }
            return;
        }
        

        static void Main(string[] args)
        {
            string fileName = @"./Data_Base";
            if(!File.Exists(fileName))
            {
                Console.WriteLine("No file existed");    
                File.WriteAllText(fileName, "Anime Title\t\tDate of release\t\tNum of episodes\t\tGenre\n");
                Console.WriteLine("Table created!");
            }

            string commandStr = "";
            List<string> commandList = new List<string>();
            string _charArr = "[]{}|/*+^;%$#@!()\\";
            bool _charCheck;
            int argsLength = 0;
            List<char> charsToRemove = new List<char>() {'{', '}'};
            while(true)
            {
                commandList.Clear();
                _charCheck = false;
                do
                {
                    Console.Write(">> ");
                    commandStr = Console.ReadLine();
                    foreach(char _char in _charArr)
                    {
                        if(commandStr.Contains(_char))
                        {
                            Console.WriteLine("Forbidden character [{0}]", _char);
                            break;
                        }
                        else
                        {
                            if(_char == '\\')
                            {
                                _charCheck = true;
                                //Console.WriteLine("No forbidden characters found.");
                            }
                            else continue;
                        }
                    }
                }
                while(!_charCheck);
                
                string _temp = "";
                /*for(int i=0; i<=commandStr.Length; i++)
                {
                    if(i != commandStr.Length && commandStr[i] != ' ')
                    {
                        _temp += commandStr[i];
                    }
                    else
                    {
                        commandList.Add(new string(_temp));
                        _temp = "";
                    }
                }
                string[] commandInArgs = commandList.ToArray();*/
                GetWordsList(ref commandStr, ref commandList);
                string[] commandInArgs = commandList.ToArray();
                argsLength = commandInArgs.Length;

                switch(commandInArgs[0])
                {
                    case "menu":
                    {
                        Console.WriteLine(  "Operations with database:\n"
                                          + "- showalldata\n" 
                                          + "- showspecdata <int>\n"
                                          + "- adddata <nameOfAnime> ReleaseDate<**.**.****> NumOfEpisodes<int> <Genre>\n"
                                          + "- changespecdata <nameOfAnime> ?<name:someName> ?<release:date> ?<numofep:int> ?<genre:str>\n"
                                          + "- findspecdata <field:data> ?<...> ?<...> ?<...>\n"
                                          + "- deldata <int>\n"
                                          + "System commands:\n- help\n- clear\n- quit");
                        break;
                    }
                    
                    case "showalldata": //Show Database

                        string[] fileData = File.ReadAllLines(@fileName);
                        foreach(string _data in fileData)
                        {
                            Console.WriteLine(_data.Filter(charsToRemove));
                        }
                        break;

                    case "showspecdata": //Show specified data (with number?)

                        if(argsLength > 2)
                        {
                            Console.WriteLine("Too many args.\nType: help showspecdata");
                            break;
                        }
                        else if(argsLength < 2)
                        {
                            Console.WriteLine("No args for command.\nType: 'help showspecdata' for additional info.");
                            break;
                        }
                        Regex commandTemplate = new Regex(@"showspecdata [1-9][0-9]?[0-9]?");
                        
                        if(commandTemplate.IsMatch(commandStr))
                        {
                            Console.WriteLine(GetLine(fileName,Int32.Parse(commandInArgs[1])).Filter(charsToRemove));
                        }
                        else
                        {
                            _temp = "";
                            if((_temp = GetLinesWithPattern(fileName,commandInArgs[1])) == "")
                            {
                                Console.WriteLine("No such pattern found.");
                            }
                            else
                            {
                                Console.Write(_temp.Filter(charsToRemove));
                            }
                        }
                        break;

                    case "adddata":         //Add a line to a database
                                            //(format: adddata <field1:>)
                        
                            //adddata <nameOfAnime> ReleaseDate<**.**.****> NumOfEpisodes<int> <Genre>\n"      
                        
                        Regex dataTemplate_1 = new Regex(@"adddata \'..*\' \'(0[1-9]|[1-2][0-9]|3[0-1])\.(0[1-9]|1[0-2])\.(19[8-9][0-9]|20[0-1][0-9]|202[0-2])\' \'[1-9]{1,4}?\' \'..*\'"); 
                        Regex dataTemplate_2 = new Regex(@"adddata ..* (0[1-9]|[1-2][0-9]|3[0-1])\.(0[1-9]|1[0-2])\.(19[8-9][0-9]|20[0-1][0-9]|202[0-2]) [1-9]{1,4}? ..*"); 
                        bool _state = false;
                        if(dataTemplate_1.IsMatch(commandStr)) _state = true;
                        if(_state || dataTemplate_2.IsMatch(commandStr))
                        {
                            //commandStr = commandStr.Filter('\'');
                            
                            /*for(int i=1; i<5; i++)
                                commandInArgs[i] = commandInArgs[i].Filter('\'');*/

                            commandList.Clear();
                            GetWordsList(ref commandStr, ref commandList, _state);
                            commandInArgs = commandList.ToArray();
                            
                            argsLength = commandInArgs.Length;
                            if(argsLength != 5) {Console.WriteLine("Incorrect arguments quantity: " + (argsLength-1) + " instead of 4\nType: 'help adddata' for additional info."); break;};
                            
                            _temp =   "{name:"    + commandInArgs[1] + "} "
                                   + "{release:" + commandInArgs[2] + "} "
                                   + "{numofep:" + commandInArgs[3] + "} "
                                   + "{genre:"   + commandInArgs[4] + "}";

                            //argsLength = commandList.ToArray();
                            //Console.WriteLine(commandStr);
                            //_temp = 
                            using (StreamWriter sw = File.AppendText(fileName)) sw.WriteLine(_temp);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect template.\nType: 'help adddata' for additional info.");
                            break;
                        }
                        
                        break;

                    case "changespecdata":  //Change specified field in the database
                                            //()
                        break;

                    case "findspecdata":    //Find via specified field (regex usage)
                                            //()
                        break;

                    case "deldata":         //Delete a line in a database (use an integer)
                        
                        break;
                    
                    case "clear":
                        Console.Clear();
                        break;
                    
                    case "quit":
                        return;

                    default:
                        Console.WriteLine("Incorrect command: " + commandInArgs[0]);
                        break;
                }
            }
        }
    }
}
