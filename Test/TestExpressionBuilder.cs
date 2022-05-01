namespace Test;

public class TestExpressionBuilder
{
    public TestExpressionBuilder(ITestOutputHelper output) =>
        builder = new SearchBuilder<TestClass>() { Log = s => output.WriteLine(s) };

    public SearchBuilder<TestClass> builder;

    [Fact]
    public void Test1()
    {
        var search = builder.Search(t => t.Id == 1, t => t.Inner.InnerId == "inner");

        Assert.Equal("Id:(1) AND Inner/InnerId:(inner)", search);
    }

    public class TestClass
    {
        public int Id { get; set; }
        public int StringId { get; set; }
        public InnerClass Inner { get; set; } = null!;

        public class InnerClass
        {
            public string InnerId { get; set; } = null!;
        }
    }
}