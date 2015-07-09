﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


namespace WTFDashboards.Models
{
    //http://www.codeproject.com/Articles/125230/ASP-NET-MVC-Chart-Control

    public class WOChart
    {
        public Chart chart { get; set; }
        public MemoryStream chartStream { get; set; }



        public WOChart(WOChartType WOChart, WOData WOData, string Title)
        {
            chart = new Chart();
            chart.Width = 700;
            chart.Height = 300;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;

            chart.Titles.Add(CreateTitle(Title));
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.PaletteCustomColors = Settings.GetChartColorPaletteColors(chart.Palette);

            switch (WOChart)
            {
                case WOChartType.Single:
                    chart.Series.Add(CreateSeries(WOData.CollectionData, SeriesChartType.Column, chart.PaletteCustomColors[0], WOData.CollectionDataName));
                    chart.ChartAreas.Add(CreateChartArea());
                    break;
                case WOChartType.Comparison:
                    chart.Legends.Add(CreateLegend());
                    chart.Series.Add(CreateSeries(WOData.CollectionData, SeriesChartType.Column, chart.PaletteCustomColors[0], WOData.CollectionDataName));
                    chart.Series.Add(CreateSeries(WOData.CollectionComparisonData, SeriesChartType.Column, chart.PaletteCustomColors[1], WOData.ComparisonDataName));
                    chart.ChartAreas.Add(CreateChartArea());
                    break;
                case WOChartType.Performance:
                    chart.Legends.Add(CreateLegend());
                    for (int intCounter = 0; intCounter < WOData.SeriesDataSets.Count; intCounter++)
                        chart.Series.Add(CreatePerformanceSeries(WOData.SeriesDataSets[intCounter], SeriesChartType.Line, WOData.SeriesDataSets[intCounter][0].WOCategoryNonNullable));
                    chart.ChartAreas.Add(CreateChartArea());
                    break;
                default:
                    break;
            }

            chartStream = new MemoryStream();
            chart.SaveImage(chartStream);
        }

        public Title CreateTitle(string TitleText)
        {
            Title title = new Title();
            title.Text = TitleText;
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);
            return title;
        }

        public Legend CreateLegend()
        {
            Legend legend = new Legend();
            legend.Name = "Result Chart";
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Table;
            //legend.IsTextAutoFit = true;
            //legend.AutoFitMinFontSize = 10;
            return legend;
        }

        public Series CreateSeries(IList<WorkOrderMetric> results, SeriesChartType chartType, Color SeriesColor, string SeriesName)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = SeriesName;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = SeriesColor;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            DataPoint point;

            foreach (var result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.WOCategoryNonNullable;
                point.YValues = new double[] { double.Parse(string.Empty + result.AverageWODuration) };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Results";
            return seriesDetail;
        }

        public Series CreatePerformanceSeries(IList<WorkOrderMetric> results, SeriesChartType chartType, string SeriesName)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = SeriesName;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            DataPoint point;

            foreach (var result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.DateCreated.ToShortDateString();
                point.YValues = new double[] { double.Parse(string.Empty + result.AverageWODuration) };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Results";
            return seriesDetail;
        }

        public ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Results";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1; //makes the XAxis labels display...
            chartArea.AxisX.Title = "";
            chartArea.AxisY.Title = "Days";

            return chartArea;
        }
    }
}