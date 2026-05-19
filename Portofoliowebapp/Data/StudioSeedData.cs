using Portofoliowebapp.Models;

namespace Portofoliowebapp.Data;

public static class StudioSeedData
{
    public static List<StudioPackage> CreatePackages()
    {
        return
        [
            new(1, "Regular Session", 150000, "Basic", "Cocok untuk latihan band reguler dan jamming santai."),
            new(2, "Recording Prep", 250000, "Popular", "Untuk latihan serius sebelum take recording atau live session."),
            new(3, "Premium Night", 350000, "Pro", "Slot malam dengan setup lebih lengkap dan durasi fleksibel.")
        ];
    }

    public static List<BookingItem> CreateBookings()
    {
        return
        [
            new(1, "Rama Band", "Regular Session", "10:00 - 12:00", 2, 150000, "Paid", "Latihan reguler, butuh 1 mic tambahan."),
            new(2, "Alya Project", "Recording Prep", "13:00 - 15:00", 2, 250000, "Pending", "Persiapan take vocal dan gitar."),
            new(3, "Northside Kids", "Premium Night", "19:00 - 22:00", 3, 350000, "Paid", "Full band, butuh setup drum lebih awal."),
            new(4, "Dika Solo", "Regular Session", "22:00 - 23:00", 1, 150000, "Pending", "Latihan gitar dan vocal monitoring."),
            new(5, "Laras Voice", "Recording Prep", "15:30 - 17:30", 2, 250000, "Cancelled", "Session vocal cancelled dari customer.")
        ];
    }

    public static List<CustomerItem> CreateCustomers()
    {
        return
        [
            new("Rama Band", "rama.band@email.com", "RB", 8, 1250000),
            new("Alya Project", "alya.project@email.com", "AP", 5, 900000),
            new("Northside Kids", "northside@email.com", "NK", 12, 2400000),
            new("Dika Solo", "dika.solo@email.com", "DS", 3, 450000),
            new("Laras Voice", "laras.voice@email.com", "LV", 4, 700000)
        ];
    }

    public static List<ChartBar> CreateWeeklyBars()
    {
        return
        [
            new("Mon", 42),
            new("Tue", 68),
            new("Wed", 54),
            new("Thu", 82),
            new("Fri", 76),
            new("Sat", 94),
            new("Sun", 64)
        ];
    }

    public static List<AdminMenuItem> CreateAdminMenuItems()
    {
        return
        [
            new("overview", "Overview", "⚡"),
            new("bookings", "Bookings", "🎧"),
            new("customers", "Customers", "👥"),
            new("finance", "Finance", "💸"),
            new("settings", "Settings", "⚙️")
        ];
    }
}
