using ChartJS.Helpers.MVC;
using Highsoft.Web.Mvc.Charts;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfFilesTest.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        public ActionResult AreaMissing()
        {
            List<double?> johnValues = new List<double?> { 0, 1, 4, 4, 5, 2, 3, 7 };
            List<double?> janeValues = new List<double?> { 1, 0, 3, 10, 3, 1, 2, 1 };

            List<AreaSeriesData> johnData = new List<AreaSeriesData>();
            List<AreaSeriesData> janeData = new List<AreaSeriesData>();

            johnValues.ForEach(p => johnData.Add(new AreaSeriesData { Y = p }));
            janeValues.ForEach(p => janeData.Add(new AreaSeriesData { Y = p }));

            ViewData["johnData"] = johnData;
            ViewData["janeData"] = janeData;


            return View("pdf");
        }

        string _setFooter = string.Format("--page-offset 0 " +
                "--footer-center [page]/[toPage]" +
                " --footer-left [webpage]" +
                " --footer-right [date]@[time]" +
                " --footer-spacing -0 --footer-font-size 8");

        public ActionResult PreviewPdf()
        {
            return new ActionAsPdf(nameof(AreaMissing))
            {
                FileName = "MyPdfFile", //Remove this line to directly preview in the browser
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(0, 0, 8, 0),
                CustomSwitches = _setFooter,
            };
        }

        public ActionResult SendEmail()
        {
            var rotativaView = new ActionAsPdf(nameof(AreaMissing))
            {
                FileName = "MyPdfFile",
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(0, 0, 8, 0),
                CustomSwitches = _setFooter,
            };

            var _byteData = rotativaView.BuildFile(this.ControllerContext);

            //Attach _byteData in the attachment 
            //email.Send();
            return Content("Email successfully sent.");
        }


        public ActionResult Pdf()
        {
            

            var rotativaView= new ActionAsPdf(nameof(AreaMissing))
            {
                FileName="MyPdfFile",
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(0, 0, 8, 0),
                CustomSwitches = _setFooter,
            };

            var _byteData = rotativaView.BuildFile(this.ControllerContext);

            System.IO.File.WriteAllBytes(Server.MapPath("~/Rotativa//test_file.pdf"), _byteData);

            return File(_byteData, "application/pdf");
        }


        private MvcHtmlString BasicLineChart()
        {
            ChartTypeLine chart = new ChartTypeLine();
            ChartTypeLine LineChart = new ChartTypeLine()
            {
                Data = new LineData()
                {
                    Labels = new string[] { "January", "February", "March", "April", "May", "June", "July" },
                    Datasets = new LineDataSets[]
                    {
                        new LineDataSets()
                        {
                            Label = "My First dataset",
                            BorderColor = "green",
                            BorderWidth = 2,
                            LinearData = new int[]{ -63, -64, 34, 43, -56, 12, 70 }
                        },
                        new LineDataSets()
                        {
                            Label = "My Second dataset",
                            BorderColor = "blue",
                            BorderWidth = 2,
                            LinearData = new int[]{ 15, -54, 45, 24, -50, 43, 36 }
                        }
                    }
                },
                Options = new LineOptions()
                {
                    Scales = new ChartOptionsScales()
                    {
                        XAxes = new ChartOptionsScalesAxes[]
                        {
                            new ChartOptionsScalesAxes()
                            {
                                Display = true,
                                ScaleLabel = new ChartScaleLabel()
                                {
                                    Display = true,
                                    LabelString = "Month"
                                }
                            }
                        },
                        YAxes = new ChartOptionsScalesAxes[]
                        {
                            new ChartOptionsScalesAxes()
                            {
                                Display = true,
                                ScaleLabel = new ChartScaleLabel()
                                {
                                    Display = true,
                                    LabelString = "Value"
                                }
                            }
                        }
                    },
                    Title = new ChartOptionsTitle()
                    {
                        Display = true,
                        Text = new string[] { "Chart.js Line Chart" }
                    },
                    Legend = new ChartOptionsLegend()
                    {
                        Position = ConstantPosition.TOP,
                    },
                    Tooltips = new ChartOptionsTooltip()
                    {
                        Mode = ConstantMode.INDEX,
                        Intersect = false
                    },
                    Hover = new ChartOptionsHover()
                    {
                        Mode = ConstantMode.NEAREST,
                        Intersect = true
                    }
                }
            };
            return new MvcHtmlString(chart.Draw("myLineChart"));
        }
    }
}
