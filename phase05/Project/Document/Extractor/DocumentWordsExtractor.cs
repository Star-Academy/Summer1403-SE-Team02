﻿namespace phase02.Document.Extractor;

public class DocumentWordsExtractor : IExtractor
{
    public IEnumerable<string> GetKey(ISearchable data)
    {
        return data.GetWords();
    }
}