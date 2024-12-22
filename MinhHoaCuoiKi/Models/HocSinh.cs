using System;
using System.Collections.Generic;

namespace MinhHoaCuoiKi.Models;

public partial class HocSinh
{
    public string MaHs { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateTime NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public bool ConTbls { get; set; }

    public string? MaLop { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }
}
