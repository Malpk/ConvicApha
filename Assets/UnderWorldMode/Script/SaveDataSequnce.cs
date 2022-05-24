using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Underworld.Editors
{
    public class SaveDataSequnce 
    {
        private const string fileContainePath = "NameContaine.json";

        private static string path = "\\Resources\\";
        private static List<string> fileContain = new List<string>();

        public static string Save(string json,string nameFile)
        {
            if (fileContain.Contains(nameFile))
                nameFile = RenameFile(nameFile);
            var filePath = Application.dataPath + path + nameFile;
            File.WriteAllText(filePath, json);
            return filePath;
        }
        public static void ReSave(string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }
        public static string Load(string filePath)
        {
            Debug.Log(filePath);
            return File.ReadAllText(filePath);
        }
        private static string RenameFile(string name)
        {
            int index = 1;
            name += "-copy";
            while (fileContain.Contains(name + $"({index})"))
            {
                index++;
            }
            return name + $"({index})";
        }
    }
}