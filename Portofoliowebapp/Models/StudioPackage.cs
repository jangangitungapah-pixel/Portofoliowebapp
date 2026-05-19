namespace Portofoliowebapp.Models;

public sealed record StudioPackage(
    int Id,
    string Name,
    int Price,
    string Tag,
    string Description
);
