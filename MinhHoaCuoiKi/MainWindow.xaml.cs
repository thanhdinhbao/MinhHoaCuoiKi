using MinhHoaCuoiKi.Models;
using System.Windows;

namespace MinhHoaCuoiKi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QlhocSinhContext _dbContext = new QlhocSinhContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbLop.Items.Clear();
            var lopList = _dbContext.Lops.Select(l => new { l.MaLop, l.TenLop }).ToList();
            cbLop.ItemsSource = lopList;
            cbLop.DisplayMemberPath = "TenLop";
            cbLop.SelectedValuePath = "MaLop";
            LoadHocSinh();
            dpNgaySinh.SelectedDate = DateTime.Now;
        }

        private void LoadHocSinh()
        {
            var hocSinhList = _dbContext.HocSinhs
                .Select(hs => new
                {
                    hs.MaHs,
                    hs.HoTen,
                    NgaySinh = hs.NgaySinh.ToString("dd-MM-yyyy"),
                    hs.GioiTinh,
                    ConThuongBinh = hs.ConTbls ? "Có" : "Không",
                    TenLop = hs.MaLopNavigation.TenLop,
                    Tuoi = DateTime.Now.Year - hs.NgaySinh.Year
                })
                .ToList();

            dgHocSinh.ItemsSource = hocSinhList;
        }
        private bool CheckValid()
        {
            if (string.IsNullOrWhiteSpace(txtMaHocSinh.Text))
            {
                MessageBox.Show("Mã học sinh không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMaHocSinh.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHoTen.Focus();
                return false;
            }

            if (!dpNgaySinh.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày sinh phải được chọn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                dpNgaySinh.Focus();
                return false;
            }

            if (cbLop.SelectedValue == null)
            {
                MessageBox.Show("Bạn phải chọn lớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                cbLop.Focus();
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValid())
            {
                return; 
            }

            if (int.TryParse((DateTime.Now.Year - dpNgaySinh.SelectedDate.Value.Year).ToString(), out int tuoi) &&
        tuoi >= 10 && tuoi <= 15)
            {
                if (_dbContext.HocSinhs.Any(hs => hs.MaHs == txtMaHocSinh.Text))
                {
                    MessageBox.Show("Mã học sinh đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var hocSinh = new HocSinh
                {
                    MaHs = txtMaHocSinh.Text,
                    HoTen = txtHoTen.Text,
                    NgaySinh = dpNgaySinh.SelectedDate.Value,
                    GioiTinh = rbNam.IsChecked == true ? "Nam" : "Nữ",
                    ConTbls = cbConThuongBinh.IsChecked == true,
                    MaLop = cbLop.SelectedValue.ToString()
                };

                _dbContext.HocSinhs.Add(hocSinh);
                _dbContext.SaveChanges();
                LoadHocSinh();
            }
            else
            {
                MessageBox.Show("Tuổi học sinh phải từ 10 đến 15.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var thongKe = _dbContext.Lops
        .Select(l => new
        {
            l.MaLop,
            l.TenLop,
            SoNu = l.HocSinhs.Count(hs => hs.GioiTinh == "Nữ")
        })
        .ToList();

            var thongKeWindow = new ThongKeWindow();
            thongKeWindow.dgThongKe.ItemsSource = thongKe;
            thongKeWindow.Show();
        }
    }
}