namespace Portofoliowebapp.Utilities;

public static class CurrencyFormatter
{
    public static string FormatRupiah(int value)
    {
        return $"Rp {value:N0}".Replace(",", ".");
    }
}
