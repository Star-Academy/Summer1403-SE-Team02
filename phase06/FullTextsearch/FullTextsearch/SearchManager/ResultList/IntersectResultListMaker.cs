using FullTextsearch.Document;
using FullTextsearch.Document.Abstraction;
using FullTextsearch.InvertedIndex;
using FullTextsearch.InvertedIndex.Abstraction;
using FullTextsearch.SearchManager.ResultList.Abstraction;

namespace FullTextsearch.SearchManager.ResultList;

public class IntersectResultListMaker : IResultListMaker
{
    public ResultListMakerType Type { get; init; } = ResultListMakerType.Intersect;

    public HashSet<ISearchable> MakeResultList(HashSet<string> keyList, IInvertedIndex myInvertedIndex)
    {
        HashSet<ISearchable> resultList = new HashSet<ISearchable>();

        if (keyList.Count < 1) return resultList;

        resultList = myInvertedIndex.GetValue(keyList.First());

        foreach (var word in keyList)
        {
            //resultList.IntersectWith(myInvertedIndex.GetValue(word));
            resultList = resultList.Join(myInvertedIndex.GetValue(word),
                l1 => l1.GetValue(),
                l2 => l2.GetValue(),
                (l1, l2) => l1).ToHashSet();
        }

        return resultList;
    }
}