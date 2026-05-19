namespace Portofoliowebapp.Models;

public sealed record PortfolioDemoItem(
    string Number,
    string Title,
    string Category,
    string Icon,
    string Label,
    string Url,
    string Badge,
    string Description,
    string MetricOne,
    string MetricOneLabel,
    string MetricTwo,
    string MetricTwoLabel,
    string MetricThree,
    string MetricThreeLabel,
    string[] Features
);
