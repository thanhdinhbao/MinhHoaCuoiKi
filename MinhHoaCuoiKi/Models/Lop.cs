using System;
using System.Collections.Generic;

namespace MinhHoaCuoiKi.Models;

public partial class Lop
{
    public string MaLop { get; set; } = null!;

    public string TenLop { get; set; } = null!;

    public virtual ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();
}
