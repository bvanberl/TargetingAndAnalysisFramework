using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ScenarioSim.Core.Entities;
using ScenarioSim.Core.Interfaces;
using ScenarioSim.Infrastructure.JsonNetSerializer;
using System.Net.Http;
using System.Threading;

namespace FittsLawAnalysisAddIn
{
    public delegate void SetPerformersDelegate(List<Performer> perflist);

    public partial class Ribbon1
    {
        Excel.Application xlApp;
        Excel.Workbook xlWb;
        Excel.Worksheet xlWs;
        IEnumerable<Performer> performers;

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            
        }

        private void get_performances_button_Click(object sender, RibbonControlEventArgs e)
        {
            PerformerSelector form = new PerformerSelector();
            form.OnSendMessage += import_performer_data;
            form.Show();
        }

        public void import_performer_data(List<Performer> perflist)
        {
            performers = perflist;
            xlApp = (Excel.Application)Marshal.GetActiveObject("Excel.Application");
            xlWb = xlApp.ActiveWorkbook;
            xlWs = xlWb.ActiveSheet;
            int wbNo = 1;

            foreach (Performer pfr in performers)
            {
                if(wbNo > 1)
                {
                    xlWb = xlApp.Workbooks.Add();
                    xlWs = xlWb.ActiveSheet;
                }
                ++wbNo;

                List<DateTime> trialTimes = new List<DateTime>(); // times of separate trials
                List<double> indeces = new List<double>(); // indeces of performance

                // Get all performances done by a performer
                IEnumerable<ScenarioPerformance> performances;
                using (HttpClient client = new HttpClient())
                {
                    var task = client.GetAsync($"http://scenariosim.azurewebsites.net/api/ScenarioPerformances?performerId=" 
                        + pfr.Id.ToString());
                    string result = task.Result.Content.ReadAsStringAsync().Result;
                    performances = JsonNetSerializer.DeserializeObject<IEnumerable<ScenarioPerformance>>(result);
                }
                List<ScenarioPerformance> performanceList = performances.ToList<ScenarioPerformance>();

                // Create a new worksheet for each performance
                foreach(ScenarioPerformance pfe in performanceList)
                {
                    Excel.Worksheet ws = xlWb.Worksheets.Add();
                    ws.Name = String.Format("{0:d-M-yyyy HH-mm-ss}", pfe.StartTime.LocalDateTime);

                    // Set column headers
                    ws.Cells[1, 1].value2 = "Start Time";
                    ws.Cells[1, 2].value2 = "End Time";
                    ws.Cells[1, 3].value2 = "Task Duration";
                    ws.Cells[1, 4].value2 = "Index of Difficulty";
                    ws.Cells[1, 5].value2 = "X-Value";
                    ws.Cells[1, 6].value2 = "Y-Value";
                    ws.Cells[1, 7].value2 = "Z-Value";
                    ws.Cells[1, 8].value2 = "W-Value";

                    // Populate the worksheet with trial data
                    for(int i = 2; i <= pfe.Events.Count + 1; i++)
                    {
                        double seconds = (double)pfe.Events[i - 2].Parameters["timeToTarget"].Value;
                        DateTime dt2 = ws.Cells[i, 2].value2 = pfe.Events[i - 2].Timestamp.LocalDateTime;
                        DateTime dt1 = dt2.AddSeconds(-seconds);
                        ws.Cells[i, 1].value2 = dt1.ToLongDateString() + " " + dt1.ToLongTimeString() + "." + dt1.Millisecond;
                        ws.Cells[i, 2].value2 = dt2.ToLongDateString() + " " + dt2.ToLongTimeString() + "." + dt2.Millisecond;
                        ws.Cells[i, 3].value2 = seconds;
                        ws.Cells[i, 4].value2 = pfe.Events[i - 2].Parameters["indexOfDifficulty"].Value;
                        ws.Cells[i, 5].value2 = ((Quaternion)pfe.Events[i - 2].Parameters["headRotation"].Value).X;
                        ws.Cells[i, 6].value2 = ((Quaternion)pfe.Events[i - 2].Parameters["headRotation"].Value).Y;
                        ws.Cells[i, 7].value2 = ((Quaternion)pfe.Events[i - 2].Parameters["headRotation"].Value).Z;
                        ws.Cells[i, 8].value2 = ((Quaternion)pfe.Events[i - 2].Parameters["headRotation"].Value).W;

                    }

                    // Create a chart to show relation between trial number and time elapsed
                    Excel.ChartObjects xlCharts = (Excel.ChartObjects)ws.ChartObjects(Type.Missing);
                    Excel.ChartObject timeChart = xlCharts.Add(425, 50, 400, 300);
                    Excel.Chart timeChartPage = timeChart.Chart;
                    timeChart.Select();

                    timeChartPage.ChartType = Excel.XlChartType.xlXYScatter;
                    Excel.SeriesCollection timeSeriesCollection = timeChartPage.SeriesCollection();

                    Excel.Series timeSeries1 = timeSeriesCollection.NewSeries();
                    timeSeries1.XValues = ws.get_Range("D2", "D" + (pfe.Events.Count + 1));
                    timeSeries1.Values = ws.get_Range("C2", "C" + (pfe.Events.Count + 1));
                    timeSeries1.Name = "Fitts' Law - Time to Target vs. Index of Difficulty";

                    // Set the titles for the X and Y axes
                    Excel.Axis yAxis = (Excel.Axis)timeChartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                    yAxis.HasTitle = true;
                    yAxis.AxisTitle.Text = "Time to target (s)";
                    yAxis.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;

                    Excel.Axis xAxis = (Excel.Axis)timeChartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                    xAxis.HasTitle = true;
                    xAxis.AxisTitle.Text = "Index of Difficulty (bits)";
                    xAxis.AxisTitle.Orientation = Excel.XlOrientation.xlHorizontal;

                    // Calculate and display trendline on this chart.
                    timeChartPage.SeriesCollection(1).Trendlines.Add(Excel.XlTrendlineType.xlLinear, Type.Missing, Type.Missing, 4,
                        4, Type.Missing, true, true, "Linear Trendline");

                    // Add time of trial to the list of trial times
                    trialTimes.Add(pfe.StartTime.LocalDateTime);

                    // Manually calculate the index of performance
                    double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
                    int n = pfe.Events.Count;
                    for(int i = 2; i <= pfe.Events.Count + 1; i++)
                    {
                        double x = Convert.ToDouble(ws.Cells[i, 4].value2);
                        double y = Convert.ToDouble(ws.Cells[i, 3].value2);
                        sumX += x;
                        sumY += y;
                        sumXY += x * y;
                        sumX2 += x * x;
                    }
                    double indexOfPerformance = (n * sumX2 - sumX * sumX) / (n * sumXY - sumX * sumY);
                    indeces.Add(indexOfPerformance);

                }

                // Set title of separate worksheet
                xlWs = xlWb.Worksheets[xlWb.Worksheets.Count];
                xlWs.Name = pfr.Name + " Performance";

                // Add trial indeces of performance over time
                xlWs.Cells[1, 1].value2 = "Trial Time";
                xlWs.Cells[1, 2].value2 = "Index of Performance";
                for (int i = 0; i < indeces.Count; i++)
                {
                    xlWs.Cells[i + 2, 1].value2 = String.Format("{0:d/M/yyyy HH:mm:ss}", trialTimes[i]);
                    xlWs.Cells[i + 2, 2].value2 = indeces[i];
                }

                // Create a chart to show relation between experiment and index of performance
                Excel.ChartObjects xlCharts2 = (Excel.ChartObjects)xlWs.ChartObjects(Type.Missing);
                Excel.ChartObject timeChart2 = xlCharts2.Add(425, 50, 400, 300);
                Excel.Chart timeChartPage2 = timeChart2.Chart;
                timeChart2.Select();

                timeChartPage2.ChartType = Excel.XlChartType.xlXYScatter;
                Excel.SeriesCollection timeSeriesCollection2 = timeChartPage2.SeriesCollection();

                Excel.Series timeSeries2 = timeSeriesCollection2.NewSeries();
                timeSeries2.XValues = xlWs.get_Range("A2", "A" + (indeces.Count + 1));
                timeSeries2.Values = xlWs.get_Range("B2", "B" + (indeces.Count + 1));
                timeSeries2.Name = "Index of Performance vs. Time for " + pfr.Name;

                // Set the titles for the X and Y axes
                Excel.Axis yAxis2 = (Excel.Axis)timeChartPage2.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
                yAxis2.HasTitle = true;
                yAxis2.AxisTitle.Text = "Index of Performance";
                yAxis2.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;

                Excel.Axis xAxis2 = (Excel.Axis)timeChartPage2.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                xAxis2.HasTitle = true;
                xAxis2.AxisTitle.Text = "Time";
                xAxis2.AxisTitle.Orientation = Excel.XlOrientation.xlHorizontal;

                // Calculate and display trendline on this chart.
                timeChartPage2.SeriesCollection(1).Trendlines.Add(Excel.XlTrendlineType.xlLinear, Type.Missing, Type.Missing, 4,
                    4, Type.Missing, true, true, "Linear Trendline");
            }
        }
    }
}
