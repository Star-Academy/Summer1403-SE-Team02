﻿namespace phase02.QueryManager.WordFinder;

public interface IWordFinder
{
    HashSet<string> FindWords(string[] words);
}