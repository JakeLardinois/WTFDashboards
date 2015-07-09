using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Drawing;
using System.Reflection;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;

namespace WTFDashboards.Models
{




    public static class Settings
    {
        public static Color[] GetChartColorPaletteColors(ChartColorPalette ChartColorPalette)
        {
            string[] ColorValues;


            switch (ChartColorPalette)
            {
                case ChartColorPalette.Berry:
                    ColorValues = new string[] { "#8A2BE2", "#BA55D3", "#4169E1", "#C71585", "#0000FF", "#8019E0", "#DA70D6", "#7B68EE", "#C000C0", "#0000CD", "#800080" };
                    break;
                case ChartColorPalette.Bright:
                    ColorValues = new string[] { "#008000", "#0000FF", "#800080", "#800080", "#FF00FF", "#008080", "#FFFF00", "#808080", "#00FFFF", "#000080", "#800000", "#FF3939", "#7F7F00", "#C0C0C0", "#FF6347", "#FFE4B5" };
                    break;
                case ChartColorPalette.BrightPastel:
                    ColorValues = new string[] { "#418CF0", "#FCB441", "#DF3A02", "#056492", "#BFBFBF", "#1A3B69", "#FFE382", "#129CDD", "#CA6B4B", "#005CDB", "#F3D288", "#506381", "#F1B9A8", "#E0830A", "#7893BE" };
                    break;
                case ChartColorPalette.Chocolate:
                    ColorValues = new string[] { "#A0522D", "#D2691E", "#8B0000", "#CD853F", "#A52A2A", "#F4A460", "#8B4513", "#C04000", "#B22222", "#B65C3A" };
                    break;
                case ChartColorPalette.EarthTones:
                    ColorValues = new string[] { "#33023", "#B8860B", "#C04000", "#6B8E23", "#CD853F", "#C0C000", "#228B22", "#D2691E", "#808000", "#20B2AA", "#F4A460", "#00C000", "#8FBC8B", "#B22222", "#843A05", "#C00000" };
                    break;
                case ChartColorPalette.Excel:
                    ColorValues = new string[] { "#9999FF", "#993366", "#FFFFCC", "#CCFFFF", "#660066", "#FF8080", "#0063CB", "#CCCCFF", "#000080", "#FF00FF", "#FFFF00", "#00FFFF", "#800080", "#800000", "#007F7F", "#0000FF" };
                    break;
                case ChartColorPalette.Fire:
                    ColorValues = new string[] { "#FFD700", "#FF0000", "#FF1493", "#DC143C", "#FF8C00", "#FF00FF", "#FFFF00", "#FF4500", "#C71585", "#DDE221" };
                    break;
                case ChartColorPalette.Grayscale:
                    ColorValues = new string[] { "#C8C8C8", "#BDBDBD", "#B2B2B2", "#A7A7A7", "#9C9C9C", "#919191", "#868686", "#7A7A7A", "#707070", "#656565", "#565656", "#4F4F4F", "#424242", "#393939", "#2E2E2E", "#232323" };
                    break;
                case ChartColorPalette.Light:
                    ColorValues = new string[] { "#E6E6FA", "#FFF0F5", "#FFDAB9", "#", "#FFFACD", "#", "#FFE4E1", "#F0FFF0", "#F0F8FF", "#F5F5F5", "#FAEBD7", "#E0FFFF" };
                    break;
                case ChartColorPalette.Pastel:
                    ColorValues = new string[] { "#87CEEB", "#32CD32", "#BA55D3", "#F08080", "#4682B4", "#9ACD32", "#40E0D0", "#FF69B4", "#F0E68C", "#D2B48C", "#8FBC8B", "#6495ED", "#DDA0DD", "#5F9EA0", "#FFDAB9", "#FFA07A" };
                    break;
                case ChartColorPalette.SeaGreen:
                    ColorValues = new string[] { "#2E8B57", "#66CDAA", "#4682B4", "#008B8B", "#5F9EA0", "#38B16E", "#48D1CC", "#B0C4DE", "#8FBC8B", "#87CEEB" };
                    break;
                case ChartColorPalette.SemiTransparent:
                    ColorValues = new string[] { "#FF6969", "#69FF69", "#6969FF", "#FFFF5D", "#69FFFF", "#FF69FF", "#CDB075", "#FFAFAF", "#AFFFAF", "#AFAFFF", "#FFFFAF", "#AFFFFF", "#FFAFFF", "#E4D5B5", "#A4B086", "#819EC1" };
                    break;
                default:
                    //return typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    //    .Select(c => (SystemColors)c.GetValue(null, null))
                    //    .ToList()
                    //    .ToArray();
                    return typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                        .Select(c => (Color)c.GetValue(null, null))
                        .ToList()
                        .ToArray();
            }

            var ColorList = new List<Color>();
            foreach (var ColorValue in ColorValues)
                ColorList.Add(ColorTranslator.FromHtml(ColorValue));

            return ColorList.ToArray();
        }
    }

    public class QueryDefinitions
    {
        static System.Resources.ResourceManager objResourceManager = new System.Resources.ResourceManager("WTFDashboards.Models.QueryDefs", System.Reflection.Assembly.GetExecutingAssembly());
        private StringBuilder strSQL = new StringBuilder();

        public string GetQuery(string strQueryName)
        {
            return objResourceManager.GetString(strQueryName);
        }

        public string GetQuery(string strQueryName, string[] strParams)
        {

            strSQL.Clear();
            strSQL.Append(objResourceManager.GetString(strQueryName));

            for (int intCounter = strParams.Length - 1; intCounter > -1; intCounter--)
            {
                string strTemp = "~p" + intCounter;
                strSQL.Replace(strTemp, strParams[intCounter]);
            }

            return strSQL.ToString();
        }
    }

    public static class SharedVariables
    {
        public static DateTime MINDATE = new DateTime(1900, 1, 1);
        public static DateTime MAXDATE = new DateTime(2999, 1, 1);
    }
}