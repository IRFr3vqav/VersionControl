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
        private int _million = (int)Math.Pow(10, 6);
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
            string[] headers = new string[]
            {
                    "Kód",
                    "Eladó",
                    "Oldal",
                    "Kerület",
                    "Lift",
                    "Szobák száma",
                    "Alapterület (m2)",
                    "Ár (mFt)",
                    "Négyzetméter ár (Ft/m2)"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }

            object[,] values = new object[Flats.Count, headers.Length];
            int szamlalo = 0;
            int floorColumn = 6;
            foreach (var flat in Flats)
            {
                values[szamlalo, 0] = flat.Code;
                values[szamlalo, 1] = flat.Vendor;
                values[szamlalo, 2] = flat.Side;
                values[szamlalo, 3] = flat.District;
                values[szamlalo, 4] = flat.Elevator ? "Van" : "Nincs";
                values[szamlalo, 5] = flat.NumberOfRooms;
                values[szamlalo, floorColumn] = flat.FloorArea;
                values[szamlalo, 7] = flat.Price;
                values[szamlalo, 8] = string.Format("={0}/{1}*{2}", "H" + (szamlalo+2).ToString(), GetCell(szamlalo+2, floorColumn+1), _million.ToString());
                szamlalo++;
            }

            var range = xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1)));
            range.Value2 = values;
        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

    }
}
