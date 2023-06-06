using Microsoft.AspNetCore.Mvc;
using AgvViewWindow.Db;
using AgvViewWindow.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection.PortableExecutable;

namespace AgvViewWindow.Controllers
{
    public class SettingsController : Controller
    {
        public BoluRequestContext _db;
        public SettingsController(BoluRequestContext db)
        {
            _db = db;
        }

        public IActionResult agvGroupSettings(Agvgroup formData)
        {
            MainModel returnModel = new MainModel(_db);
            if(formData.AgvGroupName != null)
            {
                Agvgroup agvgroup = _db.Agvgroups.Where(k => k.AgvGroupId == formData.AgvGroupId).FirstOrDefault();
                if(agvgroup != null)
                {
                    if(formData.AgvGroupName == "delete")
                    {
                        _db.Agvgroups.Remove(agvgroup);
                    } else
                    {
                        agvgroup.AgvGroupName = formData.AgvGroupName;
                    }
                } else
                {
                    Agvgroup newAgvGroup = new Agvgroup { AgvGroupName = formData.AgvGroupName };
                    _db.Agvgroups.Add(newAgvGroup);
                }
                _db.SaveChanges();
            }
            returnModel.agvgroups = returnModel.GetAgvGroups();
            return View(returnModel);
        }
        
        public IActionResult agvSettings(Agvinfo formData)
        {
            MainModel returnModel = new MainModel(_db);

            if(formData.AgvName != null)
            {
                Agvinfo agvinfo = _db.Agvinfos.Where(k => k.AgvId == formData.AgvId).FirstOrDefault();
                if(agvinfo != null)
                {
                    if(formData.AgvName == "delete")
                    {
                        _db.Agvinfos.Remove(agvinfo);
                    } else
                    {
                        agvinfo.AgvName = formData.AgvName;
                        agvinfo.AgvGroupName = formData.AgvGroupName;
                    }
                } else
                {
                    Agvinfo newAgv = new Agvinfo { AgvId = formData.AgvId, AgvName = formData.AgvName, AgvGroupName = formData.AgvGroupName };
                    _db.Agvinfos.Add(newAgv);
                }
                _db.SaveChanges();
            }
            returnModel.agvinfos = returnModel.GetAgvinfos();
            returnModel.agvgroups = returnModel.GetAgvGroups();
            return View(returnModel);
        }

        public IActionResult zoneSettings(Zone formData)
        {
            MainModel returnModel = new MainModel(_db);

            if (formData.ZoneName != null)
            {
                Zone zone = _db.Zones.Where(k=> k.ZoneId == formData.ZoneId).FirstOrDefault();
                if(zone != null)
                {
                    if (formData.ZoneName == "delete")
                    {
                        _db.Zones.Remove(zone);
                    } else
                    {
                        zone.ZoneName = formData.ZoneName;
                        zone.ZoneXZero = formData.ZoneXZero;
                        zone.ZoneYZero = formData.ZoneYZero;
                        zone.ZoneXOne = formData.ZoneXOne;
                        zone.ZoneYOne = formData.ZoneYOne;
                    }
                }
                else
                {
                    Zone newZone = new Zone { ZoneId = formData.ZoneId, ZoneName = formData.ZoneName, ZoneXZero = formData.ZoneXZero, ZoneYZero = formData.ZoneYZero, ZoneXOne = formData.ZoneXOne, ZoneYOne = formData.ZoneYOne };
                    _db.Zones.Add(newZone);

                }
                _db.SaveChanges();
            } 
            returnModel.zones = returnModel.GetZones();
            return View(returnModel);
        }


        public IActionResult mesBridgeSettings()
        {
            return View();
        }
        public IActionResult smSettings(byte status = 0)
        {
            MainModel returnModel = new MainModel(_db);
            if(status == 1)
            {

            }
            returnModel.supermarkets = returnModel.GetSupermarkets();
            return View(returnModel);
        }
        public IActionResult symbolicHold(byte status = 0)
        {
            MainModel returnModel = new MainModel(_db);
            if (status == 1)
            {
                bool flag = true;
                HubConnection hubConnection = new HubConnectionBuilder()
                                                    .WithUrl("https://localhost:5001/mesHub")
                                                    .Build();

                hubConnection.On("response52ok", () =>
                {
                    flag = false;
                });

                hubConnection.StartAsync().Wait();
                hubConnection.InvokeAsync<int>("Msg_52", 4);
                while (flag)
                    ;
                returnModel.symbolicpointholds = returnModel.GetSymbolicpointholds();
                return View(returnModel);
            }
            else
            {
                returnModel.symbolicpointholds = returnModel.GetSymbolicpointholds();
                return View(returnModel);
            }
        }
        public IActionResult symbolicInfo(byte status = 0)
        {
            MainModel returnModel = new MainModel(_db);
            if (status == 1)
            {
                bool flag = true;
                HubConnection hubConnection = new HubConnectionBuilder()
                                                    .WithUrl("https://localhost:5001/mesHub")
                                                    .Build();
                hubConnection.On("response52ok", () =>
                {
                    flag = false;
                });

                hubConnection.StartAsync().Wait();
                hubConnection.InvokeAsync<int>("Msg_52", 5);
                while (flag)
                    ;

            }
            returnModel.symbolicpointinfos = returnModel.GetSymbolicpointinfos();
            return View(returnModel);

        }
        public IActionResult symbolicName(byte status = 0)
        {
            MainModel returnModel = new MainModel(_db);
            if (status == 1)
            {
                bool flag = true;
                HubConnection hubConnection = new HubConnectionBuilder()
                                                    .WithUrl("https://localhost:5001/mesHub")
                                                    .Build();
                hubConnection.On("response52ok", () =>
                {
                    flag = false;
                });
                hubConnection.StartAsync().Wait();
                hubConnection.InvokeAsync<int>("Msg_52", 3);
                while (flag)
                    ;

            }
            returnModel.Symbolicpointnames = returnModel.GetSymbolicpointnames();
            return View(returnModel);

        }
        public IActionResult errorSettings(Errorsetting formData)
        {
            MainModel returnModel = new MainModel(_db);

            if (formData.ErrorId > 0)
            {

                Errorsetting retrun_val = new Errorsetting();

                retrun_val = returnModel.GetErrorSettingsWithId(formData.ErrorId);

                if (retrun_val != null)
                {

                    Errorsetting error = _db.Errorsettings.Where(w => w.ErrorId == formData.ErrorId).First();

                    if (formData.ErrorName != "delete")
                    {
                        error.ErrorName = formData.ErrorName.Trim();
                        error.ErrorGroup = formData.ErrorGroup;
                        error.DashboardVisible = formData.DashboardVisible;
                        error.RecordFlag = formData.RecordFlag;
                        error.ShortList = formData.ShortList;
                        error.Duration = formData.Duration;
                    }
                    else
                        _db.Errorsettings.Remove(error);
                }
                else
                {
                    _db.Errorsettings.Add(new Errorsetting
                    {
                        ErrorId = formData.ErrorId,
                        ErrorName = formData.ErrorName.Trim(),
                        ErrorGroup = formData.ErrorGroup,
                        DashboardVisible = formData.DashboardVisible,
                        RecordFlag = formData.RecordFlag,
                        Duration = formData.Duration,
                        ShortList = formData.ShortList
                    });
                }

                _db.SaveChanges();
                }

                returnModel.errorsettings = returnModel.GetErrorSettings();
                return View(returnModel);
            }

            public Errorsetting GetSpecificError(int ErrorId)
            {
                MainModel returnModel = new MainModel(_db);
                Errorsetting retrun_val = new Errorsetting();

                retrun_val = returnModel.GetErrorSettingsWithId(ErrorId);

                return (retrun_val);
            }

            public IActionResult setSymbolicName(int SPId, string nameVal)
            {
                //MainModel returnModel = new MainModel(_db);
                Symbolicpointname? val = new Symbolicpointname();
                val = _db.Symbolicpointnames.Where(a => a.SymbolicPointId == SPId).FirstOrDefault();
                if (val != null)
                {
                    val.SymbolicPointName1 = nameVal;
                }
                _db.SaveChanges();
                return Redirect("symbolicName");
            }

            public IActionResult targetSettings(Target formData)
            {
                MainModel returnModel = new MainModel(_db);
                if(formData.BatteryLevel != 0)
                {
                    Target target = _db.Targets.First();
                    target.BatteryLevel = formData.BatteryLevel;
                    target.AgvOptimumSpeed = formData.AgvOptimumSpeed;
                    target.AgvUtilRatio = formData.AgvUtilRatio;
                    target.SmLoad = formData.SmLoad;
                    target.SmWaiting = formData.SmWaiting;
                    target.WsWaiting = formData.WsWaiting;
                    _db.SaveChanges();
                }
                returnModel.targets = returnModel.GetTargets();
                return View(returnModel);
            }
        }
    }
