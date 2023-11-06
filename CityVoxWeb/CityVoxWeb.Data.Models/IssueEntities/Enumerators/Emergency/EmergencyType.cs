using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Data.Models.IssueEntities.Enumerators.Emergency
{
    public enum EmergencyType
    {
        Fire = 0,
        Medical = 1,
        Police = 2,
        TrafficAccident = 3,
        PublicDisturbance = 4,
        HazardousMaterials = 5,
        Other = 6,
    }
}
