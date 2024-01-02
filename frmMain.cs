using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Class.Functions.Connect(); //Mở kết nối
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Class.Functions.Disconnect(); //Đóng kết nối
            Application.Exit(); //Thoát
        }

        private void mnuDanhMuc_Click(object sender, EventArgs e)
        {

        }

        private void mnuChatLieu_Click(object sender, EventArgs e)
        {
            frmDMChatLieu frmChatLieu = new frmDMChatLieu();
            frmChatLieu.ShowDialog(); 
        }

        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            frmDMNhanVien frmNhanVien = new frmDMNhanVien();
            frmNhanVien.ShowDialog();
        }

        private void mnuKhachHang_Click(object sender, EventArgs e)
        {
            frmDMKhachHang frmKhachHang = new frmDMKhachHang();
            frmKhachHang.ShowDialog();
        }

        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            frmDMHangHoa frmHangHoa = new frmDMHangHoa();
            frmHangHoa.ShowDialog();
        }
    }
}
