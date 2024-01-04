using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyBanHang.Class;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace QuanLyBanHang
{
    public partial class frmHoaDonBan : Form
    {
        DataTable  CTHDB;
        public frmHoaDonBan()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void frmHoaDonBan_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnInHoaDon.Enabled = false;
            btnLuu.Enabled = false;
            btnInHoaDon.Enabled = false;
            txtMaHoaDonBan.ReadOnly = true;
            txtTenNhanVienHD.ReadOnly = true;
            txtTenKhach.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtDienThoai.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGiaBan.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
            Functions.FillCombo("SELECT MaKhach, TenKhach FROM Khach", cboMaKhach, "MaKhach", "MaKhach");
            cboMaKhach.SelectedIndex = -1;
            Functions.FillCombo("SELECT MaNhanVien, TenNhanVien FROM  NhanVien", cboMaNhanVienHD, "MaNhanVien", "TenKhach");
            cboMaNhanVienHD.SelectedIndex = -1;
            Functions.FillCombo("SELECT MaHang, TenHang FROM  Hang", cboMaHang, "MaHang", "MaHang");
            cboMaHang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHoaDonBan.Text != "")
            {
                LoadInfoHoaDon();
                btnLuu.Enabled = true;
                btnInHoaDon.Enabled = true;
            }
            LoadDataGridView();
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGiaBan, a.GiamGia,a.ThanhTien FROM  ChiTietHoaDonBan AS a,  Hang AS b WHERE a.MaHDBan = N'" + txtMaHoaDonBan.Text + "' AND a.MaHang=b.MaHang";
            //sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGiaBan, a.GiamGia,a.ThanhTien \r\nFROM  ChiTietHoaDonBan AS a,  Hang AS b \r\nWHERE a.MaHang=b.MaHang";
            CTHDB = Functions.GetDataToTable(sql);
            dgvHDBanHang.DataSource =  CTHDB;
            dgvHDBanHang.Columns[0].HeaderText = "Mã hàng";
            dgvHDBanHang.Columns[1].HeaderText = "Tên hàng";
            dgvHDBanHang.Columns[2].HeaderText = "Số lượng";
            dgvHDBanHang.Columns[3].HeaderText = "Đơn giá";
            dgvHDBanHang.Columns[4].HeaderText = "Giảm giá %";
            dgvHDBanHang.Columns[5].HeaderText = "Thành tiền";
            dgvHDBanHang.Columns[0].Width = 80;
            dgvHDBanHang.Columns[1].Width = 130;
            dgvHDBanHang.Columns[2].Width = 80;
            dgvHDBanHang.Columns[3].Width = 90;
            dgvHDBanHang.Columns[4].Width = 90;
            dgvHDBanHang.Columns[5].Width = 90;
            dgvHDBanHang.AllowUserToAddRows = false;
            dgvHDBanHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayBan FROM  HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
            txtNgayBan.Text = Functions.ConvertDateTime(Functions.GetFieldValues(str));
            str = "SELECT MaNhanVien FROM  HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
            cboMaNhanVienHD.Text = Functions.GetFieldValues(str);
            str = "SELECT MaKhach FROM  HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
            cboMaKhach.Text = Functions.GetFieldValues(str);
            str = "SELECT TongTien FROM  HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
            txtThanhTien.Text = Functions.GetFieldValues(str);
            //lblBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChu(txtThanhTien.Text);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnInHoaDon.Enabled = false;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHoaDonBan.Text = Functions.CreateKey("HDB");
            LoadDataGridView();
        }

        private void ResetValues()
        {
            txtMaHoaDonBan.Text = "";
            txtNgayBan.Text = DateTime.Now.ToShortDateString();
            cboMaNhanVien.Text = "";
            cboMaKhach.Text = "";
            txtThanhTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }
        private void ResetValuesHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void dgvHoaDonBanHang_DoubleClick(object sender, EventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if ( CTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvHDBanHang.CurrentRow.Cells["MaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE  ChiTietHoaDonBan WHERE MaHDBan=N'" + txtMaHoaDonBan.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM  Hang WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE  Hang SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM  HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE  HoaDonBan SET TongTien =" + tongmoi + " WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
                Functions.RunSQL(sql);
                txtThanhTien.Text = tongmoi.ToString();
                //lblBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChu(tongmoi.ToString());
                LoadDataGridView();
            }
        }

        private void cboMaKhach_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKhach.Text == "")
            {
                txtTenKhach.Text = "";
                txtDiaChi.Text = "";
                txtDienThoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TenKhach from Khach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtTenKhach.Text = Functions.GetFieldValues(str);
            str = "Select DiaChi from Khach where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtDiaChi.Text = Functions.GetFieldValues(str);
            str = "Select DienThoai from Khach where MaKhach= N'" + cboMaKhach.SelectedValue + "'";
            txtDienThoai.Text = Functions.GetFieldValues(str);
        }

        private void cboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHang.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGiaBan.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM  Hang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtTenHang.Text = Functions.GetFieldValues(str);
            str = "SELECT DonGiaBan FROM  Hang WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtDonGiaBan.Text = Functions.GetFieldValues(str);
        }

        private void cboMaNhanVienHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVienHD.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select TenNhanVien from  NhanVien where MaNhanVien =N'" + cboMaNhanVienHD.SelectedValue + "'";
            txtTenNhanVienHD.Text = Functions.GetFieldValues(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaBan.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaBan.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaBan.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaBan.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboMaHDBan.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHDBan.Focus();
                return;
            }
            txtMaHoaDonBan.Text = cboMaHDBan.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            btnInHoaDon.Enabled = true;
            cboMaHDBan.SelectedIndex = -1;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM ChiTietHoaDonBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
                DataTable tblHang = Functions.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM Hang WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE Hang SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    Functions.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE ChiTietHoaDonBan WHERE MaHDBan=N'" + txtMaHoaDonBan.Text + "'";
                Functions.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE HoaDonBan WHERE MaHDBan=N'" + txtMaHoaDonBan.Text + "'";
                Functions.RunSqlDel(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
                btnInHoaDon.Enabled = false;
            }
        }

        private void cboMaHDBan_DropDown(object sender, EventArgs e)
        {
            Functions.FillCombo("SELECT MaHDBan FROM HoaDonBan", cboMaHDBan, "MaHDBan", "MaHDBan");
            cboMaHDBan.SelectedIndex = -1;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHDBan FROM HoaDonBan WHERE MaHDBan=N'" + txtMaHoaDonBan.Text + "'";
            if (!Functions.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                if (txtNgayBan.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgayBan.Focus();
                    return;
                }
                if (cboMaNhanVienHD.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhach.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhach.Focus();
                    return;
                }
                sql = "INSERT INTO HoaDonBan(MaHDBan, NgayBan, MaNhanVien, MaKhach, TongTien) VALUES (N'" + txtMaHoaDonBan.Text.Trim() + "','" +
                        Functions.ConvertDateTime(txtNgayBan.Text.Trim()) + "',N'" + cboMaNhanVienHD.SelectedValue + "',N'" +
                        cboMaKhach.SelectedValue + "'," + txtThanhTien.Text + ")";
                Functions.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHang.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaHang FROM ChiTietHoaDonBan WHERE MaHang=N'" + cboMaHang.SelectedValue + "' AND MaHDBan = N'" + txtMaHDBan.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaHang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM Hang WHERE MaHang = N'" + cboMaHang.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO ChiTietHoaDonBan(MaHDBan,MaHang,SoLuong,DonGia, GiamGia,ThanhTien) VALUES(N'" + txtMaHoaDonBan.Text.Trim() + "',N'" + cboMaHang.SelectedValue + "'," + txtSoLuong.Text + "," + txtDonGiaBan.Text + "," + txtGiamGia.Text + "," + txtThanhTien.Text + ")";
            Functions.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE Hang SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHang.SelectedValue + "'";
            Functions.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM HoaDonBan WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE HoaDonBan SET TongTien =" + Tongmoi + " WHERE MaHDBan = N'" + txtMaHoaDonBan.Text + "'";
            Functions.RunSQL(sql);
            txtThanhTien.Text = Tongmoi.ToString();
            //lblBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChu(Tongmoi.ToString());
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnInHoaDon.Enabled = true;
        }
    }
}
