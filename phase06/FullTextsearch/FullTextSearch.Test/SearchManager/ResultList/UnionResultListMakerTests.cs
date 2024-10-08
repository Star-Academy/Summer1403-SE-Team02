﻿using Moq;
using FullTextsearch.Document;
using FullTextsearch.Document.Abstraction;
using FullTextsearch.InvertedIndex;
using FullTextsearch.InvertedIndex.Abstraction;
using FullTextsearch.SearchManager.ResultList;

namespace FullTextSearch.Test.SearchManager.ResultList;

public class UnionResultListMakerTests
{
    private readonly UnionResultListMaker _sut;

    public UnionResultListMakerTests()
    {
        _sut = new UnionResultListMaker();
    }

    [Fact]
    public void MakeResultList_ShouldReturnsUnionResultList_WhenInputDataIsOk()
    {
        //Arrange
        var keys = new List<string> { "a" };
        var docName = "test";
        var dataMock = new Mock<ISearchable>();
        dataMock.Setup(x => x.GetWords()).Returns(keys);
        dataMock.Setup(x => x.GetValue()).Returns(docName);
        
        var keys2 = new List<string> { "b" };
        var docName2 = "test2";
        var dataMock2 = new Mock<ISearchable>();
        dataMock2.Setup(x => x.GetWords()).Returns(keys2);
        dataMock2.Setup(x => x.GetValue()).Returns(docName2);
        
        var keys3 = new List<string> { "a" };
        var docName3 = "test3";
        var dataMock3 = new Mock<ISearchable>();
        dataMock3.Setup(x => x.GetWords()).Returns(keys3);
        dataMock3.Setup(x => x.GetValue()).Returns(docName3);
        
        var dataMap = new Dictionary<string, HashSet<ISearchable>>
        {
            {"test", new HashSet<ISearchable>{dataMock.Object, dataMock2.Object}},
            {"test2", new HashSet<ISearchable>{dataMock3.Object}},
            {"test3", new HashSet<ISearchable>{}}
        };

        var expectedResult = new HashSet<ISearchable> { dataMock.Object, dataMock2.Object, dataMock3.Object };

        var invertedIndexMock = new Mock<IInvertedIndex>();
        invertedIndexMock.Setup(x => x.GetValue(It.IsAny<string>()))
            .Returns<string>(input => dataMap[input]);
        
        //Act
         var result= _sut.MakeResultList(dataMap.Keys.ToHashSet(), invertedIndexMock.Object);
        

        //Assert
        Assert.Equal(result, expectedResult);
    }
    
    [Fact]
    public void MakeResultList_ShouldReturnsEmptyResultList_WhenKeysDoesNotExistIsOk()
    {
        //Arrange
        var dataMap = new Dictionary<string, HashSet<ISearchable>>
        {
        };

        var invertedIndexMock = new Mock<IInvertedIndex>();

        //Act
        var result = _sut.MakeResultList(dataMap.Keys.ToHashSet(), invertedIndexMock.Object);

        //Assert
        Assert.Empty(result);
    }
}