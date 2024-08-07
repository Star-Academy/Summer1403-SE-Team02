using Microsoft.VisualBasic;

namespace phase02;
public class InvertedIndexController
{
    public HashSet<string> AllDocuments { get; init; }
    private static InvertedIndexController _instance;
    public InvertedeIndex MyInvertedIndex { get; init; }
    private InvertedIndexController()
    {
        AllDocuments = new HashSet<string>();
        MyInvertedIndex = new InvertedeIndex();
    }
    public static InvertedIndexController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InvertedIndexController();
            }
            return _instance;
        }
    }
    public void AddTextToMap(string name, string text)
    {
        string[] wordList = text.TextSpliter();

        foreach (var item in wordList)
        {
            if (!MyInvertedIndex.Words.ContainsKey(item))
            {
                MyInvertedIndex.Words[item] = [name];
            }
            else
            {
                MyInvertedIndex.Words[item].Add(name);
            }
        }

        AllDocuments.Add(name);
    }
}