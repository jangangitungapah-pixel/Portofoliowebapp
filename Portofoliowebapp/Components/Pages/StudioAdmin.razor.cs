using Microsoft.AspNetCore.Components;
using Portofoliowebapp.Data;
using Portofoliowebapp.Models;
using Portofoliowebapp.Utilities;

namespace Portofoliowebapp.Components.Pages;

public partial class StudioAdmin
{
    private bool IsLoggedIn { get; set; }
    private string LoginEmail { get; set; } = "admin@37musicstudio.com";
    private string LoginPassword { get; set; } = string.Empty;
    private string ActiveSection { get; set; } = "overview";
    private string BookingSearch { get; set; } = string.Empty;

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

    private List<AdminMenuItem> MenuItems { get; } = StudioSeedData.CreateAdminMenuItems();
    private List<BookingItem> Bookings { get; } = StudioSeedData.CreateBookings();
    private List<CustomerItem> Customers { get; } = StudioSeedData.CreateCustomers();
    private List<ChartBar> WeeklyBars { get; } = StudioSeedData.CreateWeeklyBars();

    private IEnumerable<BookingItem> FilteredBookings =>
        Bookings.Where(booking =>
            string.IsNullOrWhiteSpace(BookingSearch) ||
            booking.CustomerName.Contains(BookingSearch, StringComparison.OrdinalIgnoreCase) ||
            booking.PackageName.Contains(BookingSearch, StringComparison.OrdinalIgnoreCase) ||
            booking.Status.Contains(BookingSearch, StringComparison.OrdinalIgnoreCase)
        );

    private string CurrentSectionTitle => MenuItems.First(item => item.Key == ActiveSection).Label;

    private int TotalRevenue => Bookings.Where(booking => booking.Status != "Cancelled").Sum(booking => booking.Price);
    private int PaidRevenue => Bookings.Where(booking => booking.Status == "Paid").Sum(booking => booking.Price);
    private int PendingRevenue => Bookings.Where(booking => booking.Status == "Pending").Sum(booking => booking.Price);
    private int CancelledRevenue => Bookings.Where(booking => booking.Status == "Cancelled").Sum(booking => booking.Price);

    private int PaidPercentage
    {
        get
        {
            var activeCount = Bookings.Count(booking => booking.Status != "Cancelled");

            if (activeCount == 0)
            {
                return 0;
            }

            var paidCount = Bookings.Count(booking => booking.Status == "Paid");
            return (int)Math.Round((double)paidCount / activeCount * 100);
        }
    }

    private async Task Login()
    {
        IsLoggedIn = true;

        await ShowToast(
            "Login berhasil",
            "Selamat datang di dashboard admin demo.",
            "success"
        );
    }

    private async Task Logout()
    {
        IsLoggedIn = false;
        ActiveSection = "overview";

        await ShowToast(
            "Logout berhasil",
            "Kamu kembali ke halaman login demo.",
            "info"
        );
    }

    private void SetSection(string section)
    {
        ActiveSection = section;
    }

    private async Task SelectBooking(BookingItem booking)
    {
        ActiveSection = "bookings";
        BookingSearch = booking.CustomerName;

        await ShowToast(
            "Booking dipilih",
            $"{booking.CustomerName} dibuka di Booking Manager.",
            "info"
        );
    }

    private void RequestCancelBooking(BookingItem booking)
    {
        OpenConfirmModal(
            "Cancel booking ini?",
            $"{booking.CustomerName} akan ditandai sebagai Cancelled di dashboard admin.",
            "Ya, cancel",
            "⚠️",
            "warning",
            () => SetBookingStatus(booking.Id, "Cancelled")
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

    private async Task SetBookingStatus(int id, string status)
    {
        var index = Bookings.FindIndex(booking => booking.Id == id);

        if (index < 0)
        {
            return;
        }

        var customerName = Bookings[index].CustomerName;
        Bookings[index] = Bookings[index] with { Status = status };

        var toastType = status switch
        {
            "Paid" => "success",
            "Pending" => "warning",
            "Cancelled" => "danger",
            _ => "info"
        };

        await ShowToast(
            "Status booking diubah",
            $"{customerName} sekarang berstatus {status}.",
            toastType
        );
    }

    private string GetMenuClass(string key)
    {
        return ActiveSection == key ? "admin-menu-item active" : "admin-menu-item";
    }

    private static string GetAdminStatusClass(string status)
    {
        return status switch
        {
            "Paid" => "admin-status paid",
            "Pending" => "admin-status pending",
            "Cancelled" => "admin-status cancelled",
            _ => "admin-status"
        };
    }

    private static string FormatRupiah(int value)
    {
        return CurrencyFormatter.FormatRupiah(value);
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
