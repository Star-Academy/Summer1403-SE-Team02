﻿namespace FullTextsearch.Utility.Abstractions;

public interface IFileDirectoryUtility
{
    string[] GetFiles(string path);
    string GetFileName(string path);
    string ReadAllText(string path);
}