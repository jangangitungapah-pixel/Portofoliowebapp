namespace Portofoliowebapp.Models;

public sealed record CustomerItem(
    string Name,
    string Email,
    string Initial,
    int BookingCount,
    int TotalSpend
);
