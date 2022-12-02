using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class OrgStructureModel
    {
        public Int32 Id { get; set; }

        public String structure { get; set; }

        public String City { get; set; }

        public String Name { get; set; }

        public String desig { get; set; }

        public String super { get; set; }

        public String phone { get; set; }

        public String email { get; set; }


        public class UnitDescription
        {
            public int ID { get; set; }

            public String Name { get; set; }

            public String Description { get; set; }

            public String UnitId { get; set; }

            public String strccount { get; set; }

            public String Unithead { get; set; }

            public Int32 UnitParentId { get; set; }

        }
         

        public List<OrgStructureModel> GetList()
        {
            List<OrgStructureModel> OrgLst = new List<OrgStructureModel>() { 
            
                new OrgStructureModel { Id = 1, structure = "A", City = "Kolkata",Name="ABC1",desig="SE",super="ABC",phone="123",email="A@gmail.com" }, 
                new OrgStructureModel { Id = 1, structure = "B", City = "Kolkata1",Name="ABC2",desig="RE",super="ABC",phone="123",email="B@gmail.com" },  
                new OrgStructureModel { Id = 1, structure = "C", City = "Kolkata2",Name="ABC3",desig="IE",super="ABC",phone="123",email="C@gmail.com" },
                new OrgStructureModel { Id = 1, structure = "D", City = "Kolkata3",Name="ABC4",desig="FE",super="ABC",phone="123",email="D@gmail.com" },
                new OrgStructureModel { Id = 1, structure = "E", City = "Kolkata4",Name="ABC5",desig="ASE",super="ABC",phone="123",email="E@gmail.com" },            
            };

            return OrgLst;
        }

        public List<UnitDescription> GetUnitList()
        {
            List<UnitDescription> UDESC = new List<UnitDescription>() { 
            
                new UnitDescription { ID = 1, Name = "A", Description = "Kolkata1",UnitId="ABC1",strccount="SE",Unithead ="ABC",UnitParentId =0, }, 
                new UnitDescription { ID = 2, Name = "B", Description = "Kolkata2",UnitId="ABC2",strccount="SE",Unithead ="ABC",UnitParentId =1, },
                new UnitDescription { ID = 3, Name = "B", Description = "Kolkata3",UnitId="ABC3",strccount="SE",Unithead ="ABC",UnitParentId =1, }, 
                new UnitDescription { ID = 4, Name = "B", Description = "Kolkata4",UnitId="ABC4",strccount="SE",Unithead ="ABC",UnitParentId =1, }, 
                new UnitDescription { ID = 5, Name = "B", Description = "Kolkata5",UnitId="ABC5",strccount="SE",Unithead ="ABC",UnitParentId =1, }, 
                new UnitDescription { ID = 6, Name = "B", Description = "Kolkata6",UnitId="ABC6",strccount="SE",Unithead ="ABC",UnitParentId =1, }, 
            };

            return UDESC;
        }
    }
}