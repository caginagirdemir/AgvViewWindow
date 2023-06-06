using AgvViewWindow.Db;
using Microsoft.AspNetCore.Mvc;

namespace AgvViewWindow.Models
{
    public class MainModel
    {
        public BoluRequestContext db { get; set; }
        public List<Agvinfo> agvinfos { get; set; }
        public List<Delivery> deliveries { get; set; }
        public List<Errorsetting> errorsettings { get; set; }
        public List<Supermarket> supermarkets { get; set; }
        public List<Symbolicpointinfo> symbolicpointinfos { get; set; }
        public List<Symbolicpointhold> symbolicpointholds { get; set; }
        public List<Symbolicpointname> Symbolicpointnames { get; set; }
        public List<Errorstop> Errorstops { get; set; }
        public List<Zone> zones { get; set; }
        public List<Agvgroup> agvgroups { get; set; }   
        public Target targets { get; set; }   

        public MainModel(BoluRequestContext _db)
        {
            db = _db;
        }

        public List<Agvinfo> GetAgvinfos()
        {
            return db.Agvinfos.ToList();
        }
        public Target GetTargets()
        {
            return db.Targets.First();
        }
        public List<Zone> GetZones()
        {
            return db.Zones.ToList();
        }
        public List<Agvgroup> GetAgvGroups()
        {
            return db.Agvgroups.ToList();
        }
        public List<Supermarket> GetSupermarkets()
        {
            return db.Supermarkets.ToList();
        }
        public List<Symbolicpointname> GetSymbolicpointnames()
        {
            return db.Symbolicpointnames.ToList();
        }
        public List<Symbolicpointinfo> GetSymbolicpointinfos()
        {
            return db.Symbolicpointinfos.ToList();
        }
        public List<Symbolicpointhold> GetSymbolicpointholds()
        {
            return db.Symbolicpointholds.ToList();
        }

        public List<Errorsetting> GetErrorSettings()
        {
            return db.Errorsettings.ToList();
        }
        public List<Errorstop> GetErrorstops()
        {
            return db.Errorstops.ToList();
        }

        public List<Delivery> GetDeliveries()
        {
            return db.Deliveries.ToList();
        }

        public Errorsetting GetErrorSettingsWithId(int id)
        {
            return db.Errorsettings.Where(a => a.ErrorId == id).FirstOrDefault();
        }

    }
}
