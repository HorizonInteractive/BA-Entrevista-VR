using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class WriterCSV
{
    public static void WriteFile(string email, int score)
    {
        string filePath = GetPath();
        if (File.Exists(filePath))
        {
            using StreamWriter writer = new(filePath, true);
            writer.WriteLine(email + "," + score.ToString());
            writer.Flush();
        }
        else
        {
            using StreamWriter writer = new(filePath, true, Encoding.UTF8);
            writer.WriteLine("Email,Puntaje");
            writer.WriteLine(email + "," + score.ToString());
            writer.Flush();
        }
    }

    private static string GetPath()
    {
    #if UNITY_EDITOR
        return Application.dataPath + "/Data/" + "Emails.csv";
    #else
        return Application.dataPath +"/"+"Emails.csv";
    #endif
    }
}
