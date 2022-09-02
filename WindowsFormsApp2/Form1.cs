using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public class Bilet
        {
            public ClassFeatures[] features { get; set; }
        }
        public class ClassProperties
        {
            public ClassAttributes Attributes { get; set; }
        }
        public class ClassAttributes
        {
            public string AdmArea { get; set; }
            public string District { get; set; }
            public string Location { get; set; }
            public string ActualLocaton { get; set; }
            public string OnTerritoryOfMoscow { get; set; }
            public string WorkingHours { get; set; }
            public string PointType { get; set; }
            public string BoothNumber { get; set; }
            public string PaymentType { get; set; }

        }
        public class ClassСoordinates
        {
            public float[] coordinates { get; set; }
        }
        public class ClassFeatures
        {
            public ClassСoordinates geometry { get; set; }
            public ClassProperties properties { get; set; }
        }
        public Form1()
        {
            InitializeComponent(); 
        }
        GMapOverlay ListOfMarkers1 = new GMapOverlay("markers");
        GMapOverlay ListOfMarkers2 = new GMapOverlay("markers");
        public void map_Load(object sender, EventArgs e)
        {
            // Перетаскивание карты левой кнопкой мыши
            map.DragButton = MouseButtons.Left;
            // Чья карта используется
            map.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            // Загрузка этой точки на карте
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //стартовая точка
            map.Position = new GMap.NET.PointLatLng(55.751694, 37.617218);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string url = "https://apidata.mos.ru/v1/datasets/673/features?api_key=111c24c258eb074aae14175f1c004270";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamreader.ReadToEnd();
            }
            Bilet bilet = JsonConvert.DeserializeObject<Bilet>(response);
            map.Overlays.Add(ListOfMarkers1);
            if (checkBox1.Checked)
            {
                for (int i = 0; i < bilet.features.Length; i++)
                {
                    if (bilet.features[i].properties.Attributes.PointType == "билетный киоск")
                    {
                        GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(bilet.features[i].geometry.coordinates[1], bilet.features[i].geometry.coordinates[0]), new Bitmap(@"Icon/i1.png"));
                        ListOfMarkers1.Markers.Add(marker);
                    }
                }
                map.Overlays.Add(ListOfMarkers1);
            }
            else ListOfMarkers1.Clear();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string url = "https://apidata.mos.ru/v1/datasets/673/features?api_key=111c24c258eb074aae14175f1c004270";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamreader.ReadToEnd();
            }
            Bilet bilet = JsonConvert.DeserializeObject<Bilet>(response);
            map.Overlays.Add(ListOfMarkers2);
            if (checkBox2.Checked)
            {
                for (int i = 0; i < bilet.features.Length; i++)
                {
                    if (bilet.features[i].properties.Attributes.PointType == "автомат по продаже билетов")
                    {
                        GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(bilet.features[i].geometry.coordinates[1], bilet.features[i].geometry.coordinates[0]), new Bitmap(@"Icon/i2.png"));
                        ListOfMarkers2.Markers.Add(marker);
                    }
                }
                map.Overlays.Add(ListOfMarkers2);
            }
            else ListOfMarkers2.Clear();
        }
        private void map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            string url = "https://apidata.mos.ru/v1/datasets/673/features?api_key=111c24c258eb074aae14175f1c004270";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamreader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamreader.ReadToEnd();
            }
            Bilet bilet = JsonConvert.DeserializeObject<Bilet>(response);

            for (int i = 0; i < bilet.features.Length; i++)
            {
                if ((item.Position.Lat == bilet.features[i].geometry.coordinates[1])&& (item.Position.Lng == bilet.features[i].geometry.coordinates[0]))
                {

                    labelInfo.Text = "КООРДИНАТЫ:\n" + Convert.ToString(bilet.features[i].geometry.coordinates[1]) + "; " + Convert.ToString(bilet.features[i].geometry.coordinates[0])+"\n\n"+"АДМИНИСТРАТИВНЫЙ ОКРУГ:\n"+ Convert.ToString(bilet.features[i].properties.Attributes.AdmArea) + "\n\n" + "РАЙОН:\n" + Convert.ToString(bilet.features[i].properties.Attributes.District) + "\n\n" + "АДРЕСНЫЙ ОРИЕНТИР:\n" + Convert.ToString(bilet.features[i].properties.Attributes.Location) + "\n\n" + "ФАКТИЧЕСКРЕ МЕСТОПОЛОЖЕНИЕ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.ActualLocaton) + "\n\n" + "НА ТЕРРИТОРИИ МОСКВЫ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.OnTerritoryOfMoscow) + "\n\n" + "ГРАФИК РАБОТЫ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.WorkingHours) + "\n\n" + "ТИП ТОЧКИ ПРОДАЖИ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.PointType) + "\n\n" + "НОМЕР ТОЧКИ ПРОДАЖИ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.BoothNumber) + "\n\n" + "ВИД ОПЛАТЫ:\n" + Convert.ToString(bilet.features[i].properties.Attributes.PaymentType);

                }
            }
        }
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //перемещение окна
        Point lastPoint;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
    }
}
