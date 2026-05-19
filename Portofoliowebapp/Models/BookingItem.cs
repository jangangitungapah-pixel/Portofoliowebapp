namespace Portofoliowebapp.Models;

public sealed record BookingItem(
    int Id,
    string CustomerName,
    string PackageName,
    string Time,
    int Duration,
    int Price,
    string Status,
    string Note
);
