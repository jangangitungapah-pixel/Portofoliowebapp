using Portofoliowebapp.Models;

namespace Portofoliowebapp.Data;

public static class PortfolioSeedData
{
    public static List<PortfolioDemoItem> CreateDemos()
    {
        return
        [
            new(
                "01",
                "Booking Studio Musik",
                "Studio App",
                "🎧",
                "Management",
                "studio-booking.demo",
                "Ready for MVP",
                "Demo sistem booking studio musik untuk mengatur jadwal, pelanggan, paket sewa, pembayaran, dan invoice.",
                "24",
                "Booking/bulan",
                "8",
                "Paket studio",
                "96%",
                "Slot rapi",
                [
                    "Kalender booking studio harian dan mingguan",
                    "Status pembayaran: pending, DP, lunas",
                    "Data customer dan riwayat transaksi",
                    "Invoice otomatis untuk penyewaan studio"
                ]
            ),
            new(
                "02",
                "Invoice Generator",
                "Finance Tool",
                "🧾",
                "Automation",
                "invoice-ai.demo",
                "Auto layout",
                "Demo pembuat invoice digital modern untuk jasa, rental, studio, toko, dan UMKM.",
                "1 klik",
                "Generate",
                "PDF",
                "Export",
                "Clean",
                "Design",
                [
                    "Input nama client, layanan, harga, dan tanggal",
                    "Template invoice modern dan elegan",
                    "Hitung subtotal, diskon, pajak, dan total",
                    "Siap dikembangkan untuk export PDF"
                ]
            ),
            new(
                "03",
                "Dashboard UMKM",
                "Business Panel",
                "📊",
                "Dashboard",
                "umkm-control.demo",
                "Smart view",
                "Demo dashboard untuk memantau omzet, order, customer, layanan populer, dan performa bisnis.",
                "12.8jt",
                "Omzet",
                "42",
                "Order",
                "18%",
                "Growth",
                [
                    "Ringkasan omzet dan transaksi",
                    "Grafik performa mingguan",
                    "List order terbaru",
                    "Insight sederhana untuk owner bisnis"
                ]
            )
        ];
    }
}
