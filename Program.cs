using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace AlgAnalyze
{
    class Program
    {
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
                for(int i=0; i<=commandStr.Length; i++)
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
                string[] commandInArgs = commandList.ToArray();

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
                        
                        break;
                    case "showspecdata": //Show specified data (with number?)

                        //Regex commandTemplate = new Regex(@"");

                        break;

                    case "adddata":         //Add a line to a database
                                            //(format: adddata <field1:>)
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
