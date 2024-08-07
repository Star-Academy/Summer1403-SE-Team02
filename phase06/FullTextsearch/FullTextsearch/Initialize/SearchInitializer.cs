using FullTextsearch.Document;
using FullTextsearch.Document.Extractor;
using FullTextsearch.Document.Formater;
using FullTextsearch.Factory.FolderFactory;
using FullTextsearch.Factory.SearchFactory;
using FullTextsearch.InvertedIndex;
using FullTextsearch.QueryModel;
using FullTextsearch.SearchManager;

namespace FullTextsearch.Initialize;

public class SearchInitializer(
    IDataFolderReaderFactory inputDataFolderReaderFactory,
    IInvertedIndex invertedIndex,
    ISearchStrategyFactory inputSearchStrategyFactory,
    ITextEditor textEditor, IExtractor extractor)
    : ISearchInitializer
{
    public ISearchController SearchController { get; set; }
    public IDataFolderReader DataFolderReader { get; set; }
    public void Build(DataType className, string folderPath, SearchStrategyType searchType)
    {

        DataFolderReader = inputDataFolderReaderFactory.MakeDataFolderReader(className);
        var dataList = DataFolderReader.ReadDataListFromFolder(folderPath, textEditor);
        invertedIndex.AddDataListToMap(dataList, extractor);
        SearchController = inputSearchStrategyFactory.MakeSearchController(searchType);
    }
    public HashSet<ISearchable> Search(IQuery query)
    {
        return SearchController.SearchWithQuery(query, invertedIndex);
    }
}