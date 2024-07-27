using NUnit.Framework;
using Stellar.Runtime.Helper;
using UnityEngine;

[TestFixture]
public class StringHelperTests
{
    [SetUp]
    public void SetUp()
    {
        Debug.Log($"SetUp {nameof(StringHelperTests)}");
    }

    [Test]
    public void ToColorString_ShouldReturnColoredString()
    { 
        // Arrange
        var input = "Input";
        var color = Color.red;
        var expectedOutput = "<color=#FF0000FF>Input</color>";
        
        // Act
        string colorString = StringHelper.ToColorString(input, color);

        // Assert
        Assert.AreEqual(expectedOutput, colorString);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up resources if needed
        Debug.Log($"TearDown {nameof(StringHelperTests)}");
    }
}