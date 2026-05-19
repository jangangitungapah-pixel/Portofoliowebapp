using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Portofoliowebapp.Data;
using Portofoliowebapp.Models;
using Portofoliowebapp.Utilities;

namespace Portofoliowebapp.Components.Pages;

public partial class StudioDemo
{
    private DateTime SelectedDate { get; } = DateTime.Today;

    private string SearchQuery { get; set; } = string.Empty;
    private string StatusFilter { get; set; } = "All";

    private string NewCustomerName { get; set; } = string.Empty;
    private string NewBookingTime { get; set; } = "19:00 - 21:00";
    private string NewNote { get; set; } = string.Empty;
    private int SelectedPackageId { get; set; } = 1;

    private bool IsToastVisible { get; set; }
    private string ToastTitle { get; set; } = string.Empty;
    private string ToastMessage { get; set; } = string.Empty;
    private string ToastType { get; set; } = "success";
    private int ToastVersion { get; set; }

    private bool IsConfirmVisible { get; set; }
    private string ConfirmEyebrow { get; set; } = "Konfirmasi aksi";
    private string ConfirmTitle { get; set; } = string.Empty;
    private string ConfirmMessage { get; set; } = string.Empty;
    private string ConfirmButtonText { get; set; } = "Lanjutkan";
    private string ConfirmIcon { get; set; } = "⚠️";
    private string ConfirmType { get; set; } = "danger";
    private Func<Task>? PendingConfirmAction { get; set; }

    private static readonly string[] StatusFilters = ["All", "Paid", "Pending", "Cancelled"];

    private List<StudioPackage> Packages { get; } = StudioSeedData.CreatePackages();
    private List<BookingItem> Bookings { get; } = StudioSeedData.CreateBookings()[..4];

    private BookingItem SelectedBooking { get; set; } = default!;

    private IEnumerable<BookingItem> FilteredBookings
    {
        get
        {
            var query = SearchQuery.Trim();

            return Bookings
                .Where(booking => StatusFilter == "All" || booking.Status == StatusFilter)
                .Where(booking =>
                    string.IsNullOrWhiteSpace(query) ||
                    booking.CustomerName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    booking.PackageName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    booking.Status.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    private int TodayBookings => Bookings.Count;
    private int PendingBookings => Bookings.Count(booking => booking.Status == "Pending");
    private int TotalRevenue => Bookings.Where(booking => booking.Status != "Cancelled").Sum(booking => booking.Price);
    private int PaidRevenue => Bookings.Where(booking => booking.Status == "Paid").Sum(booking => booking.Price);
    private int InvoiceSubtotal => SelectedBooking.Price;

    private int InvoicePaidAmount => SelectedBooking.Status switch
    {
        "Paid" => SelectedBooking.Price,
        "Cancelled" => 0,
        _ => SelectedBooking.Price / 2
    };

    private int InvoiceRemaining => Math.Max(InvoiceSubtotal - InvoicePaidAmount, 0);

    private string InvoicePaidLabel => SelectedBooking.Status switch
    {
        "Paid" => "Paid",
        "Cancelled" => "Paid",
        _ => "Estimated DP"
    };

    protected override void OnInitialized()
    {
        SelectedBooking = Bookings.First();
    }

    private void SelectBooking(BookingItem booking)
    {
        SelectedBooking = booking;
    }

    private void SetFilter(string filter)
    {
        StatusFilter = filter;
    }

    private async Task AddBooking()
    {
        var selectedPackage = Packages.First(package => package.Id == SelectedPackageId);

        var customerName = string.IsNullOrWhiteSpace(NewCustomerName)
            ? "New Customer"
            : NewCustomerName.Trim();

        var note = string.IsNullOrWhiteSpace(NewNote)
            ? "Booking baru dari form demo."
            : NewNote.Trim();

        var booking = new BookingItem(
            Bookings.Max(booking => booking.Id) + 1,
            customerName,
            selectedPackage.Name,
            NewBookingTime,
            2,
            selectedPackage.Price,
            "Pending",
            note
        );

        Bookings.Insert(0, booking);
        SelectedBooking = booking;

        NewCustomerName = string.Empty;
        NewBookingTime = "19:00 - 21:00";
        NewNote = string.Empty;
        StatusFilter = "All";
        SearchQuery = string.Empty;

        await ShowToast(
            "Booking ditambahkan",
            $"{booking.CustomerName} masuk ke jadwal dengan status Pending.",
            "success"
        );
    }

    private async Task MarkSelectedPaid()
    {
        await UpdateSelectedStatus("Paid", "Pembayaran lunas", $"{SelectedBooking.CustomerName} ditandai sudah lunas.", "success");
    }

    private async Task MarkSelectedPending()
    {
        await UpdateSelectedStatus("Pending", "Status pending", $"{SelectedBooking.CustomerName} dikembalikan ke status pending.", "warning");
    }

    private void RequestCancelSelectedBooking()
    {
        OpenConfirmModal(
            "Batalkan booking ini?",
            $"{SelectedBooking.CustomerName} akan ditandai sebagai Cancelled. Data masih tetap ada di jadwal.",
            "Ya, cancel",
            "⚠️",
            "warning",
            CancelSelectedBooking
        );
    }

    private void RequestDeleteSelectedBooking()
    {
        OpenConfirmModal(
            "Hapus booking ini?",
            $"{SelectedBooking.CustomerName} akan dihapus dari daftar booking demo. Aksi ini tidak bisa dibatalkan.",
            "Ya, hapus",
            "🗑️",
            "danger",
            DeleteSelectedBooking
        );
    }

    private void OpenConfirmModal(
        string title,
        string message,
        string buttonText,
        string icon,
        string type,
        Func<Task> action)
    {
        ConfirmTitle = title;
        ConfirmMessage = message;
        ConfirmButtonText = buttonText;
        ConfirmIcon = icon;
        ConfirmType = type;
        PendingConfirmAction = action;
        IsConfirmVisible = true;
    }

    private void CloseConfirmModal()
    {
        IsConfirmVisible = false;
        PendingConfirmAction = null;
    }

    private async Task RunConfirmAction()
    {
        if (PendingConfirmAction is null)
        {
            CloseConfirmModal();
            return;
        }

        var action = PendingConfirmAction;
        CloseConfirmModal();

        await action.Invoke();
    }

    private async Task CancelSelectedBooking()
    {
        await UpdateSelectedStatus("Cancelled", "Booking dibatalkan", $"{SelectedBooking.CustomerName} ditandai cancelled.", "danger");
    }

    private async Task UpdateSelectedStatus(string status, string title, string message, string type)
    {
        var index = Bookings.FindIndex(booking => booking.Id == SelectedBooking.Id);

        if (index < 0)
        {
            return;
        }

        var updatedBooking = SelectedBooking with { Status = status };
        Bookings[index] = updatedBooking;
        SelectedBooking = updatedBooking;

        await ShowToast(title, message, type);
    }

    private async Task DeleteSelectedBooking()
    {
        if (Bookings.Count <= 1)
        {
            await ShowToast(
                "Tidak bisa dihapus",
                "Minimal harus ada satu booking di demo ini.",
                "warning"
            );

            return;
        }

        var deletedCustomerName = SelectedBooking.CustomerName;

        Bookings.RemoveAll(booking => booking.Id == SelectedBooking.Id);
        SelectedBooking = Bookings.First();

        await ShowToast(
            "Booking dihapus",
            $"{deletedCustomerName} sudah dihapus dari jadwal demo.",
            "danger"
        );
    }

    private async Task PrintInvoice()
    {
        await ShowToast(
            "Invoice siap dicetak",
            $"Invoice untuk {SelectedBooking.CustomerName} akan dibuka di print dialog.",
            "info"
        );

        try
        {
            await JS.InvokeVoidAsync("portfolioPrintInvoice");
        }
        catch (TaskCanceledException)
        {
            // Print dialogs can cancel JS interop in Blazor Server after the browser opens them.
        }
        catch (JSException)
        {
            await ShowToast(
                "Print gagal",
                "Browser gagal membuka print dialog. Coba klik Print Invoice lagi.",
                "warning"
            );
        }
    }

    private async Task CopyInvoiceSummary()
    {
        var summary = $"""
    37 Music Studio Invoice
    Invoice ID: #INV-37-{SelectedBooking.Id:000}
    Customer: {SelectedBooking.CustomerName}
    Package: {SelectedBooking.PackageName}
    Time: {SelectedBooking.Time}
    Duration: {SelectedBooking.Duration} jam
    Status: {SelectedBooking.Status}
    Subtotal: {FormatRupiah(InvoiceSubtotal)}
    {InvoicePaidLabel}: {FormatRupiah(InvoicePaidAmount)}
    Remaining: {FormatRupiah(InvoiceRemaining)}
    """;

        await JS.InvokeVoidAsync("navigator.clipboard.writeText", summary);

        await ShowToast(
            "Invoice summary disalin",
            $"Ringkasan invoice {SelectedBooking.CustomerName} sudah masuk clipboard.",
            "success"
        );
    }

    private string GetInvoiceStatusNote()
    {
        return SelectedBooking.Status switch
        {
            "Paid" => "Payment completed",
            "Pending" => "Waiting for payment confirmation",
            "Cancelled" => "Booking has been cancelled",
            _ => "Status generated from demo"
        };
    }

    private static string FormatRupiah(int value)
    {
        return CurrencyFormatter.FormatRupiah(value);
    }

    private string GetBookingClass(BookingItem booking)
    {
        return booking.Id == SelectedBooking.Id ? "booking-item active" : "booking-item";
    }

    private string GetFilterClass(string filter)
    {
        return StatusFilter == filter ? "studio-filter active" : "studio-filter";
    }

    private static string GetStatusClass(string status)
    {
        return status switch
        {
            "Paid" => "status-pill-demo paid",
            "Pending" => "status-pill-demo pending",
            "Cancelled" => "status-pill-demo cancelled",
            _ => "status-pill-demo"
        };
    }

    private async Task ShowToast(string title, string message, string type = "success")
    {
        var currentVersion = ++ToastVersion;

        ToastTitle = title;
        ToastMessage = message;
        ToastType = type;
        IsToastVisible = true;

        await InvokeAsync(StateHasChanged);
        await Task.Delay(3200);

        if (currentVersion == ToastVersion)
        {
            IsToastVisible = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void HideToast()
    {
        ToastVersion++;
        IsToastVisible = false;
    }
}
