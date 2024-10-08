﻿using FullTextsearch.Context;
using FullTextsearch.Document;
using FullTextsearch.Document.Abstraction;
using FullTextsearch.Document.Extractor;
using FullTextsearch.Document.Extractor.Abstraction;
using FullTextsearch.InvertedIndex.Abstraction;
using FullTextsearch.Model;

namespace FullTextsearch.InvertedIndex;

public class InvertedIndexDbController : IInvertedIndex
{
    private ApplicationDbContext _context;

    public InvertedIndexDbController(ApplicationDbContext context)
    {
        _context = context;
    }
    public void AddDataToMap(ISearchable myData, IExtractor myExtractor)
    {
        var invertedIndexMap = _context.InvertedIndexMap;
        foreach (var key in myExtractor.GetKey(myData))
        {
            var record = invertedIndexMap.FirstOrDefault(x => x.Key == key);
            if (record != null)
            {
                var tmp = record.Values.ToHashSet();
                tmp.Add(myData.GetValue());
                record.Values = tmp.ToArray();
            }
            else
            {
                var newRecord = new InvertedIndexRecord() { Key = key, Values = [myData.GetValue()] };
                _context.InvertedIndexMap.Add(newRecord);
            }
        }

        _context.SaveChanges();
    }

    public void AddDataListToMap(IEnumerable<ISearchable> dataList, IExtractor myExtractor)
    {
        foreach (var data in dataList)
        {
            AddDataToMap(data, myExtractor);
        }
    }

    public HashSet<ISearchable> GetValue(string word)
    {
        var invertedIndexMap = _context.InvertedIndexMap.ToList().Select(x => new {key = x.Key, values = x.Values.ToList().Select(x => new DbValue.DbValue(x))});
        var record = invertedIndexMap.FirstOrDefault(x => x.key == word);
        if (record != null)
        {
            return new HashSet<ISearchable>(record.values);
        }

        return new HashSet<ISearchable>();
    }

    public HashSet<ISearchable> GetAllValue()
    {
        var invertedIndexMap = _context.InvertedIndexMap.ToList();
        var allValue = invertedIndexMap.Select(x => x.Values.ToList().Select(y => new DbValue.DbValue(y)));
        var result = new HashSet<ISearchable>();
        foreach (var dbValues in allValue)
        {
            result.UnionWith(dbValues);
        }

        return result;
    }
}