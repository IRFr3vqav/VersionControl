using MnbCurrencyReader.Entities;
using MnbCurrencyReader.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace MnbCurrencyReader
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        string result;
        string result2;
        BindingList<string> Currencies = new BindingList<string>();
        public Form1()
        {
            InitializeComponent();

            var mnbService2 = new MNBArfolyamServiceSoapClient();
            var request2 = new GetCurrenciesRequestBody();
            var response2 = mnbService2.GetCurrencies(request2);
            result2 = response2.GetCurrenciesResult;

            var xml2 = new XmlDocument();
            xml2.LoadXml(result2);
            int szamlalo1 = 0;
            foreach  (XmlElement el in xml2.DocumentElement)
            {
                int szamlalo2 = el.InnerText.Length;

                while (szamlalo2!=0)
                {
                    string currency = el.InnerText.Substring(szamlalo1, 3);
                    Currencies.Add(currency);
                    szamlalo1 = szamlalo1 + 3;
                    szamlalo2 = szamlalo2 - 3;
                }
                    
                
                
            }

            RefreshData();
        }

        private void RefreshData()
        {
            Rates.Clear();

            WebszolgHivasa();
            dataGridView1.DataSource = Rates;
            comboBox1.DataSource = Currencies;
            XMLfeldolgozasa();
            Diagram();
        }

        private void WebszolgHivasa()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);

            result = response.GetExchangeRatesResult;
        }

        private void XMLfeldolgozasa()
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                if (childElement == null) continue;
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        private void Diagram()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
