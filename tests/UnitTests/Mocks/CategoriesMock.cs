namespace UnitTests.Mocks;

public static class CategoriesMock
{
    private const int _items = 5;

    public static IEnumerable<object[]> GetCategories()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        var categories = fixture.Build<Category>().CreateMany(_items);

        yield return new object[] { categories };
    }
}
