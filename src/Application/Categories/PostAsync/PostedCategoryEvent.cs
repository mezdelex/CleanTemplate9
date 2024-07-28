namespace Application.Categories.PostAsync;

public record PostedCategoryEvent(Guid Id, string Name, string Description);
