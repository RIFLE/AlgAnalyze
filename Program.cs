using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LAB3
{
    public enum SetComamndStruct
    {
        command = 0b_00,
        flags = 0b_01,
        filenameS = 0b_10,
        flags_and_filenameS = 0b_11
    }   
    class Program
    {
        static void Main(string[] args)
        {
            try
            {   
                int argsL = args.Length;
                if(argsL == 0) throw new Exception("Nothing entered\n");
                string comand_in_line = args[0], flags = "";
                
                string[] allFNamesArray = {""};
                
                List<string> _strings = new List<string>();
                SetComamndStruct cmdConf = SetComamndStruct.command;

                if(argsL == 2 && args[1][0] == '-' && args[1].Length > 1) 
                //something -flg
                {
                    flags = args[1];
                    cmdConf = cmdConf | SetComamndStruct.flags;
                }
                else if(argsL > 1 && args[1][0] != '-') 
                //something <fname1> ...
                {
                    for(int i=1; i<argsL; i++)
                        _strings.Add(new string(args[i]));
                    allFNamesArray = _strings.ToArray();
                    cmdConf = cmdConf | SetComamndStruct.filenameS;
                }
                else if(argsL > 2 && args[1][0] == '-' && args[1].Length > 1) 
                //something -flg <fname1> ...
                {
                    flags = args[1];
                    for(int i=2; i<argsL; i++) _strings.Add(new string(args[i]));
                    allFNamesArray = _strings.ToArray();
                    cmdConf = cmdConf | SetComamndStruct.flags_and_filenameS;
                }  

                switch(comand_in_line)
                {
                    //cat -nT fileS or -
                    case "cat":
                    {
                        if((cmdConf & SetComamndStruct.filenameS) == SetComamndStruct.filenameS)
                        {
                            int counter = 0;
                            foreach(string fileName in allFNamesArray)
                            {                                     
                                if(!(Directory.Exists(fileName) || File.Exists(fileName)))
                                {
                                    Console.WriteLine("{0}: {1}: No such file or directory", comand_in_line, fileName);
                                    continue;
                                }
                                
                                FileAttributes fileAtr = File.GetAttributes(@fileName);
                                if(fileAtr.HasFlag(FileAttributes.Directory))
                                {
                                    Console.WriteLine("{0}: {1}: Is a directory", comand_in_line, fileName);
                                    continue;
                                }
                                else
                                {
                                    string[] fileData = File.ReadAllLines(@fileName);
                                    for(int i=0; i<fileData.Count(); i++, counter++)
                                    {
                                        
                                        if((cmdConf & SetComamndStruct.flags) == SetComamndStruct.flags)
                                        {
                                            if(flags.Contains('n')) 
                                            {
                                                Console.Write("     ");
                                                Console.Write((counter+1).ToString() + "  ");
                                            }    
                                            if(flags.Contains('T')) fileData[i] = fileData[i].Replace("\t","^I");
                                            if(!flags.Contains('T') && !flags.Contains('n')) 
                                            {
                                                Console.WriteLine("{0}: invalid option -- '{1}'", comand_in_line, flags[1]);
                                                return;
                                            }
                                            
                                        }
                                        Console.WriteLine(fileData[i]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string strbuf = "";
                            int counter = 0;    
                            do
                            {
                                strbuf = Console.ReadLine();
                                if((cmdConf & SetComamndStruct.flags) == SetComamndStruct.flags)
                                {
                                    if(flags.Contains('n')) 
                                    {
                                        Console.Write((counter+1).ToString() + "  ");
                                    }    
                                    if(flags.Contains('T')) strbuf = strbuf.Replace("\t","^I");
                                    if(!flags.Contains('T') && !flags.Contains('n')) 
                                    {
                                        Console.WriteLine("{0}: invalid option -- '{1}'", comand_in_line, flags[1]);
                                        return;
                                    }
                                }
                                Console.WriteLine(strbuf);
                                counter++;
                            }
                            while(strbuf != "");

                        }
                        break;
                    }
                    
                    //sort fileS
                    case "sort":
                    {
                        foreach(string fileName in allFNamesArray)
                        {                                     
                            if(Directory.Exists(fileName))
                            {
                                Console.WriteLine(comand_in_line + ": read failed: {0} : Is a directory", fileName);
                                continue;
                            }
                            else if(!File.Exists(fileName))
                            {
                                Console.WriteLine(comand_in_line + ": cannot read: {0} : No such file or directory", fileName);
                                continue;
                            }
                            else
                            {
                                string[] lines = File.ReadAllLines(@fileName);
                            
                                StringComparer currCmp = StringComparer.CurrentCulture;
                                Array.Sort(lines, currCmp);    
                                foreach(string line in lines) Console.WriteLine(line);
                            }
                        }
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("First arg is not a command: args[0] =  {0}", args[0]);
                        break;
                    }
                         
                }               
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}