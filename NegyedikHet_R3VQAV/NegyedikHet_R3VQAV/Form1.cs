using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace NegyedikHet_R3VQAV
{
    public partial class Form1 : Form
    {
        List<Flat> Flats;
        RealEstateEntities context = new RealEstateEntities();

        Excel.Application xlApp; //Excel alkalmazas
        Excel.Workbook xlWB; //letrehozunk munkafuzetet
        Excel.Worksheet xlSheet; //letrehozunk munkalapot a munkafuzeten belul
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        public void LoadData()
        {
            Flats = context.Flats.ToList();
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application(); //Excel elinditasa
                xlWB = xlApp.Workbooks.Add(Missing.Value);//ures munkafuzet letrehozasa
                xlSheet = xlWB.ActiveSheet; //kivalasztjuk a rajta levo munkalap

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true; //control atadasa a felhasznalonak
            }
            catch (Exception ex)
            {
                string hiba = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(hiba, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing);//ha hiba van, nem kell menteni erre utal a false
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        public void CreateTable()
        {

        }
    }
}
