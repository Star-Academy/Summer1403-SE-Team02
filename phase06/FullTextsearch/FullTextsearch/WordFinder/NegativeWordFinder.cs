﻿using FullTextsearch.WordFinder.Abstraction;

namespace FullTextsearch.WordFinder;

public class NegativeWordFinder : IWordFinder
{
    private readonly char NegativeChar = '-';

    public WordFinderType Type { get; init; } = WordFinderType.Negative;

    public HashSet<string> FindWords(IEnumerable<string> words)
    {
        var result = words
            .Where(word => word[0] == NegativeChar)
            .Select(word => word.Substring(1))
            .ToHashSet();

        return result;
    }
}