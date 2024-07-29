namespace Application.Messages;

public static class GenericValidationMessages
{
    public static string ShouldNotBeEmpty(string property) => $"{property} should not be emtpy.";

    public static string ShouldBeAGuid(string property) => $"{property} should not be a Guid.";

    public static string ShouldNotBeLongerThan(string property, int length) =>
        $"{property} should be a maximum of {length} characters long.";
}
