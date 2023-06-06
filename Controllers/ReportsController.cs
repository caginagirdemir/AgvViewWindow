using AgvViewWindow.Db;
using AgvViewWindow.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ClosedXML.Excel;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using NuGet.Protocol;
using Station = AgvViewWindow.Db.Station;
using Irony;
using System.Linq;

namespace AgvViewWindow.Controllers
{


    public class ReportsController : Controller
    {
        public BoluRequestContext _db;
        public ReportsController(BoluRequestContext db)
        {
            _db = db;
        }
        public class Foo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public FileResult Export(List<string> theDay)
        {

            DateTime dt;

            MainModel datab = new MainModel(_db);
            datab.deliveries = datab.GetDeliveries();
            if (theDay == null)
                dt = DateTime.Today.AddDays(0);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "AgvReport.xlsx";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Deliveries");

                worksheet.ColumnWidth = 12;

                var dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(string));
                dataTable.Columns.Add("MachineId", typeof(string));
                dataTable.Columns.Add("SuperMarketId", typeof(string));
                dataTable.Columns.Add("TargetId", typeof(string));
                dataTable.Columns.Add("TakePointId", typeof(string));

                worksheet.Column(6).Width = 18;
                worksheet.Column(7).Width = 18;
                worksheet.Column(8).Width = 18;
                worksheet.Column(9).Width = 18;
                worksheet.Column(10).Width = 18;
                worksheet.Column(11).Width = 18;
                worksheet.Column(12).Width = 18;
                worksheet.Column(13).Width = 18;

                dataTable.Columns.Add("RequestTime", typeof(string));
                dataTable.Columns.Add("ResponseTime", typeof(string));
                dataTable.Columns.Add("PreReleaseTime", typeof(string));
                dataTable.Columns.Add("DropTime", typeof(string));
                dataTable.Columns.Add("ToMarketTime", typeof(string));
                dataTable.Columns.Add("AtSuperMarketTime", typeof(string));
                dataTable.Columns.Add("AtPoleTime", typeof(string));
                dataTable.Columns.Add("AverageCycleTime", typeof(string));

                foreach (string itemDate in theDay)
                {
                    dt = Convert.ToDateTime(itemDate);
                    List<Delivery> model = datab.deliveries.Where(k => k.RequestTime.Day == dt.Day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year).ToList();
                    foreach (Delivery item in model)
                    {
                        dataTable.Rows.Add(item.DeliveryId, item.MachineId, item.SupermarketId, item.DeliveredTargetId, item.TakePointId, item.RequestTime, item.ResponseTime, item.PrereleasePointTime, item.DropTime, item.ToMarketTime, item.AtSupermarketTime, item.AtPoleTime, item.AvgCycleSpeed);
                    }
                }
                
                worksheet.Cell("A2").InsertTable(dataTable);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }

            }
        }

        public IActionResult Index(string theDay)
        {
            ReportModel cycleReportModel = new ReportModel();

            Target target = _db.Targets.FirstOrDefault();
            if(target != null)
                cycleReportModel.optimum_speed = target.AgvOptimumSpeed;
            else
                cycleReportModel.optimum_speed = 0;

            cycleReportModel.daily_average_min = 0;
            cycleReportModel.daily_average_speed = 0;
            List<Double> StationSumSpeed = new List<double> { 0.00, 0.00, 0.00 };
            List<int> StationSpeedTick = new List<int> { 0, 0, 0 };
            List<Delivery> deliveryData = new List<Delivery>();
            List<int> Station = new List<int>();

            List<DateTime> dtList = new List<DateTime>();
            List<int> dayList = new List<int>();
            List<int> monthList = new List<int>();
            List<int> yearList = new List<int>();

            if (theDay != null)
            {
                string[] days = theDay.Split("-");
                foreach (string day in days)
                {
                    DateTime subDt = Convert.ToDateTime(day);
                    cycleReportModel.dates.Add(subDt.Date.ToString("dd.MM.yyyy"));
                    dtList.Add(subDt);
                    if(!dayList.Contains(subDt.Day))
                        dayList.Add(subDt.Day);
                    if (!monthList.Contains(subDt.Month))
                        monthList.Add(subDt.Month);
                    if (!yearList.Contains(subDt.Year))
                        yearList.Add(subDt.Year);
                    List<Delivery> subDeliveryData = _db.Deliveries.Where(k => k.RequestTime.Day == subDt.Day && k.RequestTime.Month == subDt.Month && k.RequestTime.Year == subDt.Year).ToList();
                    deliveryData.AddRange(subDeliveryData);
                }

                if (deliveryData.Count > 0)
                    foreach (Delivery item in deliveryData)
                        if (!Station.Contains((int)item.DeliveredTargetId))
                            Station.Add((int)item.DeliveredTargetId);
            }
            
            List<int> Agv = new List<int>();


            if (deliveryData.Count  > 0)
            {
                foreach (int item in Station)
                {
                    // fill Station and StationGroup names
                    Station stationData = _db.Stations.Where(k => k.StationSymbolicPointId == item).FirstOrDefault();
                    if(stationData != null)
                    {
                        string stationStr = stationData.StationName + " (" + stationData.StationSymbolicPointId + ")";
                        cycleReportModel.stationTypeName.Add(stationStr);
                        cycleReportModel.stationGroupTypeName.Add(stationData.StationGroupName);
                    }

                    // fill Station Daily Speeds
                    List<Delivery> deliveryDataJustForOneId = _db.Deliveries.Where(k => k.DeliveredTargetId == item).ToList();
                    TimeSpan? ts;
                    foreach (Delivery itemDeliveriesForOneId in deliveryDataJustForOneId)
                    {
                        ts = itemDeliveriesForOneId.AtSupermarketTime - itemDeliveriesForOneId.RequestTime;

                        if (dayList.Contains(itemDeliveriesForOneId.RequestTime.Day))
                        {
                            StationSumSpeed[0] += ts.Value.TotalMinutes;
                            StationSpeedTick[0]++;
                        }
                        if (monthList.Contains(itemDeliveriesForOneId.RequestTime.Month))
                        {
                            StationSumSpeed[1] += ts.Value.TotalMinutes;
                            StationSpeedTick[1]++;
                        }
                        if (yearList.Contains(itemDeliveriesForOneId.RequestTime.Year))
                        {
                            StationSumSpeed[2] += ts.Value.TotalMinutes;
                            StationSpeedTick[2]++;
                        }
                    }
                    cycleReportModel.daily_average_min += Math.Round(StationSumSpeed[0] / StationSpeedTick[0], 2);
                    cycleReportModel.stationTypeSpeedDaily.Add(Math.Round(StationSumSpeed[0] / StationSpeedTick[0], 2).ToString());
                    cycleReportModel.stationTypeSpeedMonthly.Add(Math.Round(StationSumSpeed[1] / StationSpeedTick[1], 2).ToString());
                    cycleReportModel.stationTypeSpeedYearly.Add(Math.Round(StationSumSpeed[2] / StationSpeedTick[2], 2).ToString());

                    for (int i = 0; i < 3; i++)
                    {
                        StationSumSpeed[i] = 0;
                        StationSpeedTick[i] = 0;
                    }
                }
                cycleReportModel.daily_average_min = Math.Round(cycleReportModel.daily_average_min / Station.Count, 2);
            

                List<Agvgroup> agvGroupList = new List<Agvgroup>();
                agvGroupList = _db.Agvgroups.ToList();

                List<double> agvSumSpeed = new List<double> { 0.00, 0.00, 0.00 };
                List<int> agvSpeedTick = new List<int> { 0, 0, 0 };

                foreach (Agvgroup item in agvGroupList)
                {
                    cycleReportModel.agvTypeName.Add(item.AgvGroupName);

                    List<Agvinfo> agvInfos = _db.Agvinfos.Where(k => k.AgvGroupName == item.AgvGroupName).ToList();
                    foreach (Agvinfo deliveryItem in agvInfos)
                    {
                        List<Delivery> deliveryInfo = _db.Deliveries.Where(k => k.MachineId == deliveryItem.AgvId).ToList();
                        foreach (Delivery delivery in deliveryInfo)
                        {
                            if(dayList.Contains(delivery.RequestTime.Day))
                            {
                                agvSumSpeed[0] += (double)delivery.AvgCycleSpeed;
                                agvSpeedTick[0]++;
                            }

                            if (monthList.Contains(delivery.RequestTime.Month))
                            {
                                agvSumSpeed[1] += (double)delivery.AvgCycleSpeed;
                                agvSpeedTick[1]++;
                            }

                            if (yearList.Contains(delivery.RequestTime.Year))
                            {
                                agvSumSpeed[2] += (double)delivery.AvgCycleSpeed;
                                agvSpeedTick[2]++;
                            }
                        }
                    }
                    cycleReportModel.daily_average_speed += Math.Round(agvSumSpeed[0] / agvSpeedTick[0], 2);
                    cycleReportModel.agvTypeSpeedDaily.Add(Math.Round(agvSumSpeed[0] / agvSpeedTick[0], 2).ToString());
                    cycleReportModel.agvTypeSpeedMonthly.Add(Math.Round(agvSumSpeed[1] / agvSpeedTick[1], 2).ToString());
                    cycleReportModel.agvTypeSpeedYearly.Add(Math.Round(agvSumSpeed[2] / agvSpeedTick[2], 2).ToString());

                    for (int i = 0; i < 3; i++)
                    {
                        agvSumSpeed[i] = 0;
                        agvSpeedTick[i] = 0;
                    }
                }

                cycleReportModel.daily_average_speed = Math.Round(cycleReportModel.daily_average_speed / agvGroupList.Count, 2);
            }
            return View(cycleReportModel);
        }

        public IActionResult SMDashboard(string theDay)
        {
            SmWaitingModel returnModel = new SmWaitingModel();

            List<Delivery> deliveryData = new List<Delivery>();
            List<DateTime> dtList = new List<DateTime>();
            List<int> dayList = new List<int>();
            List<int> monthList = new List<int>();
            List<int> yearList = new List<int>();

            

            if (theDay != null)
            {
                string[] days = theDay.Split("-");
                foreach (string day in days)
                {
                    DateTime subDt = Convert.ToDateTime(day);
                    returnModel.dates.Add(subDt.Date.ToString("dd.MM.yyyy"));
                    dtList.Add(subDt);
                    if (!dayList.Contains(subDt.Day))
                        dayList.Add(subDt.Day);
                    if (!monthList.Contains(subDt.Month))
                        monthList.Add(subDt.Month);
                    if (!yearList.Contains(subDt.Year))
                        yearList.Add(subDt.Year);
                }
            

            deliveryData = _db.Deliveries.ToList();
            if (deliveryData.Count > 0)
            {
                List<Supermarket> superMarketList = new List<Supermarket>();
                List<int> agvList = new List<int>();
                superMarketList = _db.Supermarkets.ToList();
                foreach (Supermarket item in superMarketList)
                {

                    //load variables
                    double[] SMLoadingSum = new double[3];
                    uint[] SMLoadingTick = new uint[3];
                    double[] smLoadTimeLine = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    double[] smLoadTimeLineTicks = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    //wait variables
                    double[] SMWaitSum = new double[3];
                    uint[] SMWaitTick = new uint[3];
                    double[] smWaitTimeLine = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    double[] smWaitTimeLineTicks = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



                    returnModel.SuperMarketNames.Add(item.SupermarketName);
                    List<Delivery> deliveryWithOneSm = deliveryData.FindAll(s => s.SupermarketId == item.SupermarketSymbolicPoint).ToList();
                    var deliveryWithOneDay = deliveryWithOneSm.GroupBy(s => s.RequestTime.Day);
                    var deliveryWithOneMonth = deliveryWithOneSm.GroupBy(s => s.RequestTime.Month);
                    var deliveryWithOneYear = deliveryWithOneSm.GroupBy(s => s.RequestTime.Year);


                    foreach (var yearDelivery in deliveryWithOneYear)
                    {
                        if(yearList.Contains(yearDelivery.Key))
                        {
                            var monthDeliveries = yearDelivery.GroupBy(s => s.RequestTime.Month);
                            foreach (var monthDelivery in monthDeliveries)
                            {
                                var deliveryList = monthDelivery.GroupBy(s => s.RequestTime.Day);
                                foreach (var itemDay in deliveryList)
                                {
                                    var deliveryWithOneAgv = itemDay.GroupBy(s => s.MachineId);
                                    foreach (var item1 in deliveryWithOneAgv)
                                    {
                                        foreach (Delivery item2 in item1)
                                        {
                                            TimeSpan? ts = item2.ResponseTime - item2.RequestTime;
                                            SMLoadingSum[2] += ts.Value.TotalMinutes;
                                            SMLoadingTick[2]++;
                                        }

                                        if (item1.Count() >= 2)
                                        {
                                            List<Delivery> deliveryTemp = item1.ToList();
                                            for (int i = 0; i < deliveryTemp.Count - 1; i++)
                                            {
                                                TimeSpan? ts = deliveryTemp[i + 1].RequestTime - deliveryTemp[i].AtSupermarketTime;
                                                SMWaitSum[2] += ts.Value.TotalMinutes;
                                                SMWaitTick[2]++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    foreach (var monthDelivery in deliveryWithOneMonth)
                    {
                        if (monthList.Contains(monthDelivery.Key))
                        {
                            var deliveryList = monthDelivery.GroupBy(s => s.RequestTime.Day);
                            foreach (var itemDay in deliveryList)
                            {
                                var deliveryWithOneAgv = itemDay.GroupBy(s => s.MachineId);
                                foreach (var item1 in deliveryWithOneAgv)
                                {
                                    foreach (Delivery item2 in item1)
                                    {
                                        TimeSpan? ts = item2.ResponseTime - item2.RequestTime;
                                        SMLoadingSum[1] += ts.Value.TotalMinutes;
                                        SMLoadingTick[1]++;
                                    }

                                    if (item1.Count() >= 2)
                                    {
                                        List<Delivery> deliveryTemp = item1.ToList();
                                        for (int i = 0; i < deliveryTemp.Count - 1; i++)
                                        {
                                            TimeSpan? ts = deliveryTemp[i + 1].RequestTime - deliveryTemp[i].AtSupermarketTime;
                                            SMWaitSum[1] += ts.Value.TotalMinutes;
                                            SMWaitTick[1]++;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    foreach (var dayDelivery in deliveryWithOneDay)
                    {
                        if (dayList.Contains(dayDelivery.Key))
                        {
                            var deliveryWithOneAgv = dayDelivery.GroupBy(s => s.MachineId);
                            foreach (var deliveryWithAgvItem in deliveryWithOneAgv)
                            {
                                foreach (Delivery deliveryWithOneAgvItem in deliveryWithAgvItem)
                                {
                                    TimeSpan? ts = deliveryWithOneAgvItem.ResponseTime - deliveryWithOneAgvItem.RequestTime;
                                    SMLoadingSum[0] += ts.Value.TotalMinutes;
                                    SMLoadingTick[0]++;
                                    double tempVal = (double)deliveryWithOneAgvItem.RequestTime.Hour / 12;
                                    tempVal = (double)tempVal * 6;
                                    smLoadTimeLine[(int)tempVal] += SMLoadingSum[0];
                                    smLoadTimeLineTicks[(int)tempVal]++;
                                }

                                if (deliveryWithAgvItem.Count() >= 2)
                                {
                                    List<Delivery> deliveryTemp = deliveryWithAgvItem.ToList();
                                    for (int i = 0; i < deliveryTemp.Count - 1; i++)
                                    {
                                        TimeSpan? ts = deliveryTemp[i + 1].RequestTime - deliveryTemp[i].AtSupermarketTime;
                                        SMWaitSum[0] += ts.Value.TotalMinutes;
                                        SMWaitTick[0]++;
                                        double tempVal = (double)deliveryTemp[i + 1].RequestTime.Hour / 12;
                                        tempVal = (double)tempVal * 6;
                                        smWaitTimeLine[(int)tempVal] += SMLoadingSum[0];
                                        smWaitTimeLineTicks[(int)tempVal]++;
                                    }
                                }
                            }
                        }


                    }
                    returnModel.LoadTimeLine.Add(smLoadTimeLine);
                    returnModel.LoadTimeLineCount.Add(smLoadTimeLineTicks);
                    returnModel.LoadSMValueDaily.Add(Math.Round(SMLoadingSum[0] / SMLoadingTick[0], 2).ToString());
                    returnModel.LoadSMValueMonthly.Add(Math.Round(SMLoadingSum[1] / SMLoadingTick[1], 2).ToString());
                    returnModel.LoadSMValueYearly.Add(Math.Round(SMLoadingSum[2] / SMLoadingTick[2], 2).ToString());

                    returnModel.WaitingTimeLine.Add(smWaitTimeLine);
                    returnModel.WaitingTimeLineCount.Add(smWaitTimeLineTicks);
                    returnModel.WaitingSMValueDaily.Add(Math.Round(SMWaitSum[0] / SMWaitTick[0], 2).ToString());
                    returnModel.WaitingSMValueMonthly.Add(Math.Round(SMWaitSum[1] / SMWaitTick[1], 2).ToString());
                    returnModel.WaitingSMValueYearly.Add(Math.Round(SMWaitSum[2] / SMWaitTick[2], 2).ToString());

                }

            }


                //foreach (Supermarket item in superMarketList)
                //{
                //    returnModel.SuperMarketNames.Add(item.SupermarketName);

                //    List<Delivery> deliveryWithOneSm = deliveryData.FindAll(s=>s.SupermarketId == item.SupermarketSymbolicPoint).ToList();

                //    foreach (DateTime dtItem in dtList)
                //    {
                //        List<Delivery> deliveryWithOneDay = deliveryWithOneSm.FindAll(s => s.RequestTime.Day == dtItem.Day && s.RequestTime.Month == dtItem.Month && s.RequestTime.Year == dtItem.Year).ToList();


                //    }


                //List<Delivery> deliveryDataJustForOneSM = _db.Deliveries.Where(k => k.SupermarketId == item.SupermarketSymbolicPoint && k.RequestTime.Year == dt.Year).ToList();

                //foreach (Delivery itemDeliveriesForOneSM in deliveryDataJustForOneSM)
                //{
                //    TimeSpan? ts = itemDeliveriesForOneSM.ResponseTime - itemDeliveriesForOneSM.RequestTime;
                //    if (itemDeliveriesForOneSM.RequestTime.Day == dt.Day)
                //    {
                //        SMLoadingSum[0] += ts.Value.TotalMinutes;
                //        SMLoadingSumTick[0]++;
                //        tempVal = (double)itemDeliveriesForOneSM.RequestTime.Hour / 12;
                //        tempVal = (double)tempVal * 6;
                //        smLoadTimeLine[(int)tempVal] += SMLoadingSum[0];
                //        smLoadTimeLineTicks[(int)tempVal]++;
                //    }
                //    if (itemDeliveriesForOneSM.RequestTime.Month == dt.Month)
                //    {
                //        SMLoadingSum[1] += ts.Value.TotalMinutes;
                //        SMLoadingSumTick[1]++;
                //    }
                //    if (itemDeliveriesForOneSM.RequestTime.Year == dt.Year)
                //    {
                //        SMLoadingSum[2] += ts.Value.TotalMinutes;
                //        SMLoadingSumTick[2]++;
                //    }
                //}
                //returnModel.LoadTimeLine.Add(smLoadTimeLine);
                //returnModel.LoadTimeLineCount.Add(smLoadTimeLineTicks);
                //returnModel.LoadSMValueDaily.Add(Math.Round(SMLoadingSum[0] / SMLoadingSumTick[0], 2).ToString());
                //returnModel.LoadSMValueMonthly.Add(Math.Round(SMLoadingSum[1] / SMLoadingSumTick[1], 2).ToString());
                //returnModel.LoadSMValueYearly.Add(Math.Round(SMLoadingSum[2] / SMLoadingSumTick[2], 2).ToString());

                //for (int i = 0; i < 3; i++)
                //{
                //    SMLoadingSum[i] = 0;
                //    SMLoadingSumTick[i] = 0;
                //}

                //foreach (Delivery itemDeliveriesForOneSM in deliveryDataJustForOneSM)
                //{
                //    if(!agvList.Contains((int)itemDeliveriesForOneSM.MachineId))
                //    {
                //        agvList.Add((int)itemDeliveriesForOneSM.MachineId);
                //        List<Delivery> deliveryWait = _db.Deliveries.Where(k => k.RequestTime.Year == dt.Year && k.MachineId == itemDeliveriesForOneSM.MachineId).ToList();

                //        foreach (Delivery deliveryItem in deliveryWait)
                //        {

                //        }
                //    }
                //}
                //}




                //foreach (Supermarket item in superMarketList)
                //{
                //    List<Delivery> subDelivery = datab.deliveries.Where(k => k.RequestTime.Day == dt.Day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId).ToList();
                //    List<int> agvList = new List<int>();
                //    double sumDaily = 0.00;
                //    UInt16 sumTick = 0;
                //    double tempVal = 0.00;
                //    double[] smWaitTimeLine = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                //    double[] smWaitTimeLineTicks = new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //    foreach (Delivery subItem in subDelivery)
                //    {
                //        if (!agvList.Contains((int)subItem.MachineId))
                //        {
                //            agvList.Add((int)subItem.MachineId);
                //            List<Delivery> subDeliveryWithMachine = datab.deliveries.Where(k => k.RequestTime.Day == dt.Day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId && k.MachineId == subItem.MachineId).ToList();
                //            if (subDeliveryWithMachine.Count > 1)
                //            {
                //                for (int i = 0; i < (subDeliveryWithMachine.Count - 1); i++)
                //                {
                //                    if (subDeliveryWithMachine[i].AtSupermarketTime != null)
                //                    {
                //                        TimeSpan? ts = subDeliveryWithMachine[i + 1].RequestTime - subDeliveryWithMachine[i].AtSupermarketTime;
                //                        sumDaily += ts.Value.TotalMinutes;
                //                        sumTick++;



                //                        tempVal = (double)subDeliveryWithMachine[i].AtSupermarketTime.Value.Hour / 12;
                //                        tempVal = (double)tempVal * 6;
                //                        smWaitTimeLine[(int)tempVal] += sumDaily;
                //                        smWaitTimeLineTicks[(int)tempVal]++;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    for (int i = 0; i < 12; i++)
                //    {
                //        if (smWaitTimeLineTicks[i] > 0)
                //            smWaitTimeLine[i] = Math.Round(smWaitTimeLine[i] / smWaitTimeLineTicks[i], 2);
                //    }
                //    returnModel.WaitingTimeLine.Add(smWaitTimeLine);
                //    returnModel.WaitingTimeLineCount.Add(smWaitTimeLineTicks);
                //    if (Math.Round(sumDaily / sumTick, 2) > 0)
                //    {
                //        returnModel.WaitingSMValueDaily.Add(Math.Round(sumDaily / sumTick, 2).ToString());
                //    }
                //    else
                //    {
                //        return View(returnModel);
                //    }

                //    int days = DateTime.DaysInMonth(dt.Year, dt.Month);
                //    double montlySum = 0;
                //    int montlyTickSum = 0;
                //    for (int day = 1; day <= days; day++)
                //    {
                //        subDelivery = datab.deliveries.Where(k => k.RequestTime.Day == day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId).ToList();
                //        agvList.Clear();
                //        sumDaily = 0.00;
                //        sumTick = 0;
                //        if(subDelivery.Count > 0)
                //        { 
                //            foreach (Delivery subItem in subDelivery)
                //            {
                //                if (!agvList.Contains((int)subItem.MachineId))
                //                {
                //                    agvList.Add((int)subItem.MachineId);
                //                    List<Delivery> subDeliveryWithMachine = datab.deliveries.Where(k => k.RequestTime.Day == day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId && k.MachineId == subItem.MachineId).ToList();
                //                    if (subDeliveryWithMachine.Count > 1)
                //                    {
                //                        for (int i = 0; i < (subDeliveryWithMachine.Count - 1); i++)
                //                        {
                //                            TimeSpan? ts = subDeliveryWithMachine[i + 1].RequestTime - subDeliveryWithMachine[i].AtSupermarketTime;
                //                            sumDaily += ts.Value.TotalMinutes;
                //                            sumTick++;
                //                        }
                //                        if (Math.Round(sumDaily / sumTick, 2) > 0)
                //                        {
                //                            montlyTickSum++;
                //                            montlySum += Math.Round(sumDaily / sumTick, 2);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    if (montlyTickSum > 0)
                //    {
                //        returnModel.WaitingSMValueMonthly.Add(Math.Round(montlySum / montlyTickSum, 2).ToString());
                //    }



                //    double yearlySum = 0;
                //    int yearlyTickSum = 0;
                //    for (int i = 1; i <= 12; i++)
                //    {
                //        days = DateTime.DaysInMonth(dt.Year, i);
                //        montlySum = 0;
                //        montlyTickSum = 0;

                //        for (int day = 1; day <= days; day++)
                //        {
                //            subDelivery = datab.deliveries.Where(k => k.RequestTime.Day == day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId).ToList();
                //            agvList.Clear();
                //            sumDaily = 0.00;
                //            sumTick = 0;
                //            if (subDelivery.Count > 0)
                //            {
                //                foreach (Delivery subItem in subDelivery)
                //                {
                //                    if (!agvList.Contains((int)subItem.MachineId))
                //                    {
                //                        agvList.Add((int)subItem.MachineId);
                //                        List<Delivery> subDeliveryWithMachine = datab.deliveries.Where(k => k.RequestTime.Day == day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year && k.SupermarketId == item.SupermarketId && k.MachineId == subItem.MachineId).ToList();
                //                        if (subDeliveryWithMachine.Count > 1)
                //                        {
                //                            for (int j = 0; j < (subDeliveryWithMachine.Count - 1); j++)
                //                            {
                //                                TimeSpan? ts = subDeliveryWithMachine[j + 1].RequestTime - subDeliveryWithMachine[j].AtSupermarketTime;
                //                                sumDaily += ts.Value.TotalMinutes;
                //                                sumTick++;
                //                            }
                //                            if (Math.Round(sumDaily / sumTick, 2) > 0)
                //                            {
                //                                montlyTickSum++;
                //                                montlySum += Math.Round(sumDaily / sumTick, 2);
                //                            }
                //                        }
                //                    }
                //                }
                //            }

                //        }
                //        if (Math.Round(montlySum / montlyTickSum, 2) > 0)
                //        {
                //            yearlyTickSum++;
                //            yearlySum += Math.Round(montlySum / montlyTickSum, 2);
                //        }
                //    }
                //    if (yearlySum > 0)
                //    {
                //        returnModel.WaitingSMValueYearly.Add(Math.Round(yearlySum / yearlyTickSum, 2).ToString());
                //    }
                //}
            }
            return View(returnModel);
        }
        public IActionResult Utilization(string theDay)
        {
            UtilizationModel returnModel = new UtilizationModel();

            DateTime dt = DateTime.Now;

            if (theDay != null)
            {
                dt = Convert.ToDateTime(theDay);
            }
            returnModel.date = dt.Date.ToString("dd.MM.yyyy");

            return View(returnModel);
        }
        public IActionResult WSDashboard(string theDay)
        {
            WsModel returnModel = new WsModel();
            MainModel mainModel = new MainModel(_db);
            List<int> targets = new List<int>();
            DateTime dt = DateTime.Now;

            if (theDay != null)
            {
                dt = Convert.ToDateTime(theDay);
            }
            returnModel.date = dt.Date.ToString("dd.MM.yyyy");

            List<Zone> zones = _db.Zones.ToList();
            foreach (Zone zoneItem in zones)
                returnModel.zones.Add(zoneItem.ZoneName);
            
            List<Errorsetting> errorsettings = _db.Errorsettings.ToList();
            foreach (Errorsetting errorItem in errorsettings)
                if (!returnModel.errorList.Contains(errorItem.ErrorGroup))
                    returnModel.errorList.Add(errorItem.ErrorGroup);

            List<Delivery> deliveryTable = _db.Deliveries.Where(k => k.RequestTime.Day == dt.Day && k.RequestTime.Month == dt.Month && k.RequestTime.Year == dt.Year).ToList();
            List<Symbolicpointname> spNames = mainModel.GetSymbolicpointnames();

            foreach (Delivery item in deliveryTable)
            {
                if(item.DeliveredTargetId != null)
                    if(!targets.Contains((int)item.DeliveredTargetId))
                        targets.Add((int)item.DeliveredTargetId);
            }

            foreach (int item in targets)
            {
                foreach (Symbolicpointname spName in spNames)
                {
                    if (spName.SymbolicPointId == item)
                    {
                        returnModel.workStationList.Add(spName.SymbolicPointName1);
                    }
                }
            }

            if(!(returnModel.workStationList.Count > 0))
                return View(returnModel);

            double sumtoDropForStation = 0.00;
            UInt16 sumtoDropTickForStation = 0;

            double sumtoMarketForStation = 0.00;
            UInt16 sumtoMarketTickForStation = 0;

            double sumErrorForStation = 0.00;
            UInt16 sumErrorTickForStation = 0;
            foreach (int item in targets)
            {
                sumtoDropForStation = 0.00;
                sumtoDropTickForStation = 0;
                sumtoMarketForStation = 0.00;
                sumtoMarketTickForStation = 0;
                sumErrorForStation = 0.00;
                sumErrorTickForStation = 0;
                foreach (Delivery itemDelivery in deliveryTable)
                {
                    if(itemDelivery.DeliveredTargetId == item)
                    {
                        if(itemDelivery.PrereleasePointTime != null && itemDelivery.DropTime != null)
                        {
                            TimeSpan? ts = itemDelivery.DropTime - itemDelivery.PrereleasePointTime;
                            sumtoDropForStation += ts.Value.TotalMinutes;
                            sumtoDropTickForStation++;
                        }

                        if (itemDelivery.DropTime != null && itemDelivery.ToMarketTime != null)
                        {
                            TimeSpan? ts = itemDelivery.ToMarketTime - itemDelivery.DropTime;
                            sumtoMarketForStation += ts.Value.TotalMinutes;
                            sumtoMarketTickForStation++;
                        }

                        if(itemDelivery.PrereleasePointTime != null && itemDelivery.ToMarketTime != null)
                        {
                            List<Errorstop> stops =  _db.Errorstops.Where(k => k.TrackBeginTime.Day == dt.Day && k.TrackBeginTime.Month == dt.Month && k.TrackBeginTime.Year == dt.Year && k.TrackBeginTime > itemDelivery.PrereleasePointTime && k.TrackBeginTime < itemDelivery.ToMarketTime).ToList();
                            foreach (Errorstop itemStops in stops)
                            {
                                sumErrorForStation += Convert.ToDouble(itemStops.StopDuration);
                                sumErrorTickForStation++;
                            }
                        }
                    }
                }
                returnModel.toDropTime.Add(Math.Round(sumtoDropForStation / sumtoDropTickForStation, 2).ToString());
                returnModel.toOutTime.Add(Math.Round(sumtoMarketForStation / sumtoMarketTickForStation, 2).ToString());
                returnModel.stopedTime.Add(Math.Round(sumErrorForStation / sumErrorTickForStation, 2).ToString());
            }

            return View(returnModel);
        }

        public IActionResult ControlDashboard()
        {
            return View();
        }

    }

}
