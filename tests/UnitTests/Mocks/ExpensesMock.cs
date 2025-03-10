namespace UnitTests.Mocks;

public static class ExpensesMock
{
    private const int _items = 5;

    public static IEnumerable<object[]> GetExpenses()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        var expenses = fixture.Build<Expense>().CreateMany(_items);

        yield return new object[] { expenses };
    }
}
