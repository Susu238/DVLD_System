using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
   public class clsCountryBusiness
    {
        enum enMode { AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;
        public int ID { set; get; }
        public string CountryName { set; get; }
        public clsCountryBusiness()
        {
            this.ID = -1;
            this.CountryName = "";


            this.Mode = enMode.AddNew;
        }
        private clsCountryBusiness(int ID, string CountryName)
        {
            this.ID = ID;
            this.CountryName = CountryName;
            this.Mode = enMode.Update;
        }
      
        
       
     
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
      

        public static clsCountryBusiness Find(int ID)
        {
            string CountryName = "";
          
            if (clsCountryData.GetAllCountriesByID(ID, ref CountryName))
                return new clsCountryBusiness(ID, CountryName);
            else
                return null;
        }
        public static clsCountryBusiness Find(string CountryName)
        {
            int CountryID = -1;
            if (clsCountryData.GetAllCountriesByName(ref CountryID, CountryName))
                return new clsCountryBusiness(CountryID, CountryName);
            else
                return null;
        }







    }
}
