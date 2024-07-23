using System.Reflection;

namespace phase02;
public class DocumentReader : IDataReader
{
    public InvertedIndexController MyInvertedIndex {get; set;}
    public DocumentReader()
    {
        this.MyInvertedIndex = InvertedIndexController.Instance;
    }
    public void RaedFolder(string path)
    {

        try
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var data = RaedData(file);
                var name = Path.GetFileName(file);

                MyInvertedIndex.AddTextToMap(name, data);
            }
        }
        catch(FileNotFoundException)
        {
            throw new FileNotFoundException("Path not found!");
        }

    }

    public string RaedData(string path)
    {
        return File.ReadAllText(path);
    }
}