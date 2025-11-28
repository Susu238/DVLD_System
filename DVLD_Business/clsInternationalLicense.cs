using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
namespace DVLD_Business
{
    public class clsInternationalLicense :clsApplications
    {
        public enum enMode { AddNew = 0,Update = 1 };
        enMode Mode = enMode.AddNew;
        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID{ get; set; }
        public DateTime IssueDate { get; set; }
        public  DateTime ExpirationDate{ get; set; }
        public bool IsActive { get; set; }
        public clsDrivers DriverInfo { get; set; }



      public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
            Mode = enMode.AddNew;
        }
        public clsInternationalLicense(int applicationID, int ApplicantPersonID, DateTime ApplicationDate,int ApplicationTypeID,
           DateTime LastStatusDate, float PaidFees,  int createdByUserID ,int internationalID,int driverID,int issueUsingLocalLicenseID,
            DateTime issueDate, DateTime expirationDate,bool isActive
            
            )
        {

            //this for the base class
           base.ApplicationID = applicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;

            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;

            base.PaidFees = PaidFees;
            base.CreatedByUserID = createdByUserID;
            //this for international license
            this.InternationalLicenseID = internationalID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.IssuedUsingLocalLicenseID = issueUsingLocalLicenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;
            this.CreatedByUserID = createdByUserID;
            this.DriverInfo = clsDrivers.FindByDriverID(this.DriverID);
            Mode = enMode.Update;
        }
        public bool AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicensesData.AddNewInternationalLicense(this.ApplicationID, this.DriverID,
                this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }
        public bool UpdateInternationalLicense()
        {
            return clsInternationalLicensesData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID
                , this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }
        public static clsInternationalLicense Find(int internationalLicenseID)
        {
            int applicationID = -1, driverID = -1, issueUsingLocalLicenseID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;
            bool IsFound = clsInternationalLicensesData.GetIternationalLicenseInfoByID(internationalLicenseID, ref applicationID, ref driverID,
              ref issueUsingLocalLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID);
            if (IsFound)
            {
                //now we found the base application
              clsApplications Application =  clsApplications.FindBaseApplication(applicationID);
                return new clsInternationalLicense(Application.ApplicationID,Application.ApplicantPersonID,
               Application.ApplicationDate,Application.ApplicationTypeID,  Application.LastStatusDate
               ,
                      Application.PaidFees, Application.CreatedByUserID, internationalLicenseID,driverID, issueUsingLocalLicenseID,issueDate,
                   expirationDate, isActive
                );
            }
            else
                return null;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicensesData.GetAllInternationalLicenses();
        }
        public static int GetActiveLicenseIDByDriverID( int DriverID)
        {
            return clsInternationalLicensesData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicensesData.GetDriverInternationalLicenses(DriverID);
        }
        public bool ISExist(int InternationalLicenseID)
        {
            return clsInternationalLicensesData.IsExist(InternationalLicenseID);

            
        }
        public bool Delete(int internationalLicenseID)
        {
            return clsInternationalLicensesData.DeleteInternationalLicense(internationalLicenseID);
        }
        //Because of inheritance first we call the save method in the base class,
        //it will take care of adding all information to the application table.
        public bool Save()
        {
            base.Mode = (clsApplications.enMode)Mode;
            if (!base.Save())
            {
                return false;
            }
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return UpdateInternationalLicense();

            }

            return false;
        }
    }
}
