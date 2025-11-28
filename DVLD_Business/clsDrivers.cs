using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_Business
{
   public class clsDrivers
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public clsPeopleBusiness PersonInfo { get; set; }
        public DateTime CreatedDate{ get; set; }
        public clsDrivers()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;
        }
        public clsDrivers(int driverID,int personID,int createdByUserID,DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;
            this.PersonInfo = clsPeopleBusiness.Find(this.PersonID);
            Mode = enMode.Update;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriversData.GetAllDrivers();
        }
        public static DataTable GetDriverLicenses(int driverID)
        {
            return clsLicenses.GetDriverLicenses(driverID);
        }
        public static DataTable GetInternationalLicenses( int driverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(driverID);
        }
        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this.DriverID = clsDriversData.AddDriver(
                this.PersonID, this.CreatedByUserID
                 );

            return (this.DriverID != -1);
        }
        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriversData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID
               );

        }
        public static clsDrivers FindByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
          DateTime  CreatedDate = DateTime.Now;
           
            bool IsFound = clsDriversData.GetDrivernfoByPersonID
                                (
                                    PersonID, ref DriverID,
                                    ref CreatedByUserID, ref CreatedDate
                                   
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsDrivers(DriverID, PersonID,
                                    CreatedByUserID,
                                    CreatedDate
                                     );
            else
                return null;
        }
        public static clsDrivers FindByDriverID(int DriverID)
        {
            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            bool IsFound = clsDriversData.GetDrivernfoByID
                                (
                                   DriverID , ref PersonID,
                                    ref CreatedByUserID, ref CreatedDate

                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsDrivers(DriverID, PersonID,
                                    CreatedByUserID,
                                    CreatedDate
                                     );
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }
        public DataTable GetDriverLicense(int DriverID)
        {
            return clsLicensesData.GetDriverLicenses(DriverID);
        }
    }
}
