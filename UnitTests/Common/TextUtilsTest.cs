using Common;

namespace UnitTests.Common;

public class TextUtilsTest
{
    [Fact]
    public void TestCodify()
    {
        var text = string.Empty;
        
        Assert.Equal("", text);
        Assert.Equal("", TextUtils.Codify(text,10));

        text = "Rowan Tree";
        Assert.Equal("ROWANTREE", TextUtils.Codify(text,10));
        Assert.Equal("ROWAN", TextUtils.Codify(text,5));
        Assert.Equal("ROWANT", TextUtils.Codify(text,6));

    }
}