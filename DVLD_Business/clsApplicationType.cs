using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{

  public class clsApplicationType
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }
        
        public clsApplicationType() {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = 0;
            Mode = enMode.AddNew;

            }
        private clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees = ApplicationFees;
            Mode = enMode.Update;
        }
        public static clsApplicationType FindByApplicationTypeID(int applicationTypeID)
        {
            string  applicationTypeTitle = "";
            float applicationFees = 0;
            bool IsFound = clsApplicationTypesData.GetApplicationTypeInfoByID(applicationTypeID, ref applicationTypeTitle, ref applicationFees);
            if (IsFound)
            {
                return new clsApplicationType(applicationTypeID,applicationTypeTitle,applicationFees);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypesData.AddNewApplicationType (this.ApplicationTypeTitle, this.ApplicationFees );
            return (this.ApplicationTypeID != -1);
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }


        private bool _UpdateApplicationType() {
            return clsApplicationTypesData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }
        public bool Save()
        {
            try
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewApplicationType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case enMode.Update:
                        return _UpdateApplicationType();
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Log or store error

            }
            return false;
        }

    }
}
