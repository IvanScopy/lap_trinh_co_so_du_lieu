using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace lap_trinh_co_so_du_lieu
{
    public partial class frmMatHang : Form
    {
        // Khởi tạo đối tượng dtbase từ lớp DataBaseProcess để xử lý kết nối và truy vấn cơ sở dữ liệu
        Classes.DataBaseProcess dtbase = new Classes.DataBaseProcess();

        public frmMatHang()
        {
            InitializeComponent();
        }
        private void HienChiTiet(bool hien)
        {
            txtMaSP.Enabled = hien;
            txtTenSP.Enabled = hien;
            dtpNgayHH.Enabled = hien;
            dtpNgaySX.Enabled = hien;
            txtDonVi.Enabled = hien;
            txtDonGia.Enabled = hien;
            txtGhiChu.Enabled = hien;

            // Ẩn hiện 2 nút Lưu và Hủy
            btnLuu.Enabled = hien;
            btnHuy.Enabled = hien;
        }
        // Sự kiện load của Form frmMatHang
        private void frmMatHang_Load(object sender, EventArgs e)
        {
            // Load dữ liệu lên DataGridView
            dgvMatHang.DataSource = dtbase.DataReader("SELECT * FROM tblMatHang");

            // Ẩn các nút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Ẩn groupBox Chi tiết
            HienChiTiet(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận trước khi thoát
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Nếu người dùng chọn Yes, đóng form
                this.Close();
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Hiển thị nút Sửa và Xóa
            btnSua.Enabled = true;
            btnXoa.Enabled = true;

            // Ẩn nút Thêm
            btnThem.Enabled = false;

            // Bắt lỗi khi người dùng click linh tinh
            try
            {
                // Lấy thông tin từ các ô trong DataGridView và hiển thị lên các TextBox và các điều khiển khác
                txtMaSP.Text = dgvMatHang.CurrentRow.Cells[0].Value.ToString();
                txtTenSP.Text = dgvMatHang.CurrentRow.Cells[1].Value.ToString();
                dtpNgaySX.Value = (DateTime)dgvMatHang.CurrentRow.Cells[2].Value;
                dtpNgayHH.Value = (DateTime)dgvMatHang.CurrentRow.Cells[3].Value;
                txtDonVi.Text = dgvMatHang.CurrentRow.Cells[4].Value.ToString();
                txtDonGia.Text = dgvMatHang.CurrentRow.Cells[5].Value.ToString();
                txtGhiChu.Text = dgvMatHang.CurrentRow.Cells[6].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lấy dữ liệu từ bảng: " + ex.Message);
            }
        }

        private void XoaTrangChiTiet()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            dtpNgaySX.Value = DateTime.Today;
            dtpNgayHH.Value = DateTime.Today;
            txtDonVi.Text = "";
            txtDonGia.Text = "";
            txtGhiChu.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Cập nhật tiêu đề của form hoặc groupBox để thể hiện trạng thái thêm mới
            lblTieuDe.Text = "THÊM MẶT HÀNG";

            // Xóa sạch các thông tin chi tiết trong groupBox Chi tiết
            XoaTrangChiTiet();

            // Vô hiệu hóa các nút Sửa và Xóa (chỉ cho phép thêm mới)
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Hiển thị groupBox Chi tiết để nhập thông tin mới
            HienChiTiet(true);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Cập nhật tiêu đề cho nhãn
            lblTieuDe.Text = "TÌM KIẾM MẶT HÀNG";

            // Cấm nút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Viết câu lệnh SQL để tìm kiếm
            string sql = "SELECT * FROM tblMatHang WHERE MaSP IS NOT NULL";

            // Nếu người dùng nhập Mã sản phẩm để tìm kiếm
            if (txtTKMaSP.Text.Trim() != "")
            {
                sql += " AND UPPER(MaSP) LIKE UPPER('%" + txtTKMaSP.Text.Trim() + "%')";
            }

            // Nếu người dùng nhập Tên sản phẩm để tìm kiếm
            if (txtTKTenSP.Text.Trim() != "")
            {
                sql += " AND UPPER(TenSP) LIKE UPPER('%" + txtTKTenSP.Text.Trim() + "%')";
            }

            // Load dữ liệu tìm được lên DataGridView
            dgvMatHang.DataSource = dtbase.DataReader(sql);
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Cập nhật tiêu đề để thể hiện rằng người dùng đang ở chế độ sửa
            lblTieuDe.Text = "CẬP NHẬT MẶT HÀNG";

            // Vô hiệu hóa nút Thêm và Xóa
            btnThem.Enabled = false;
            btnXoa.Enabled = false;

            // Hiển thị groupBox Chi tiết để cho phép người dùng chỉnh sửa thông tin
            HienChiTiet(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";

            // Kiểm tra tên sản phẩm có bị để trống không
            if (txtTenSP.Text.Trim() == "")
            {
                errChiTiet.SetError(txtTenSP, "Bạn không để trống tên sản phẩm!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            // Kiểm tra ngày sản xuất, lỗi nếu ngày sản xuất lớn hơn ngày hiện tại
            if (dtpNgaySX.Value > DateTime.Now)
            {
                errChiTiet.SetError(dtpNgaySX, "Ngày sản xuất không hợp lệ!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            // Kiểm tra ngày hết hạn xem có lớn hơn ngày sản xuất không
            if (dtpNgayHH.Value < dtpNgaySX.Value)
            {
                errChiTiet.SetError(dtpNgayHH, "Ngày hết hạn nhỏ hơn ngày sản xuất!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            // Kiểm tra đơn vị có bị để trống không
            if (txtDonVi.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonVi, "Bạn không để trống đơn vị!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            // Kiểm tra đơn giá có bị để trống không
            if (txtDonGia.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonGia, "Bạn không để trống đơn giá!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            // Nếu nút Thêm được bật, thực hiện thêm mới
            if (btnThem.Enabled == true)
            {
                // Kiểm tra xem ô nhập Mã sản phẩm có bị trống không
                if (txtMaSP.Text.Trim() == "")
                {
                    errChiTiet.SetError(txtMaSP, "Bạn không để trống mã sản phẩm!");
                    return;
                }
                else
                {
                    // Kiểm tra xem mã sản phẩm đã tồn tại chưa để tránh việc thêm mới bị lỗi
                    sql = $"SELECT * FROM tblMatHang WHERE MaSP = '{txtMaSP.Text}'";
                    DataTable dtSP = dtbase.DataReader(sql);
                    if (dtSP.Rows.Count > 0)
                    {
                        errChiTiet.SetError(txtMaSP, "Mã sản phẩm đã tồn tại trong cơ sở dữ liệu");
                        return;
                    }
                    errChiTiet.Clear();
                }

                // Chèn dữ liệu mới vào cơ sở dữ liệu Oracle
                sql = "INSERT INTO tblMatHang(MaSP, TenSP, NgaySX, NgayHH, DonVi, DonGia, GhiChu) VALUES (";
                sql += $"'{txtMaSP.Text}', '{txtTenSP.Text}', TO_DATE('{dtpNgaySX.Value:yyyy-MM-dd}', 'YYYY-MM-DD'), ";
                sql += $"TO_DATE('{dtpNgayHH.Value:yyyy-MM-dd}', 'YYYY-MM-DD'), '{txtDonVi.Text}', {txtDonGia.Text}, '{txtGhiChu.Text}')";
            }

            // Nếu nút Sửa được bật, thực hiện cập nhật dữ liệu
            if (btnSua.Enabled == true)
            {
                sql = "UPDATE tblMatHang SET ";
                sql += $"TenSP = '{txtTenSP.Text}', ";
                sql += $"NgaySX = TO_DATE('{dtpNgaySX.Value:yyyy-MM-dd}', 'YYYY-MM-DD'), ";
                sql += $"NgayHH = TO_DATE('{dtpNgayHH.Value:yyyy-MM-dd}', 'YYYY-MM-DD'), ";
                sql += $"DonVi = '{txtDonVi.Text}', ";
                sql += $"DonGia = {txtDonGia.Text}, ";
                sql += $"GhiChu = '{txtGhiChu.Text}' ";
                sql += $"WHERE MaSP = '{txtMaSP.Text}'";
            }

            // Nếu nút Xóa được bật, thực hiện xóa dữ liệu
            if (btnXoa.Enabled == true)
            {
                sql = $"DELETE FROM tblMatHang WHERE MaSP = '{txtMaSP.Text}'";
            }

            // Thực thi câu lệnh SQL
            dtbase.DataChange(sql);

            // Cập nhật lại DataGridView
            sql = "SELECT * FROM tblMatHang";
            dgvMatHang.DataSource = dtbase.DataReader(sql);

            // Ẩn các nút phù hợp chức năng và ẩn groupBox Chi tiết
            HienChiTiet(false);
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Thiết lập lại trạng thái các nút
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;

            // Xóa trắng các trường chi tiết
            XoaTrangChiTiet();

            // Vô hiệu hóa groupBox chi tiết
            HienChiTiet(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn  có  chắc  chắn  xóa  mã  mặt  hàng  " + txtMaSP.Text + " không ? Nếu  có  ấn  nút  Lưu, không  thì  ấn  nút  Hủy", "Xóa  sản  phẩm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lblTieuDe.Text = "XÓA MẶT HÀNG";
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                //Hiện gropbox chi tiết
                HienChiTiet(true);
            }
        }
    }
}
