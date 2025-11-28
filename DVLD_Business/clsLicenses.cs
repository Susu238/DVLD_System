using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Business.clsApplications;
namespace DVLD_Business
{
    public class clsLicenses
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enIssueReason {FirstTime = 1, Renew = 2,DamagedReplacement=3, LostReplacement=4 };
       public clsDrivers DriverInfo;
        public int LicenseID { set; get; }

        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public DateTime IssueDate { set; get; }
        public clsLicenseClass ClassInfo;
        public clsDetainedLicense DetainedInfo { get; set; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason{ set; get; }
        public int CreatedByUserID { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public bool IsDetained
        {
            get
            {
                return clsDetainedClassData.IsDetainedLicense(this.LicenseID);
            }
        }
       public clsLicenses() {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = 1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.IsActive = false;
            this.PaidFees = 0;
            this.IssueReason =enIssueReason.FirstTime;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
       public clsLicenses(int LicenseID,int applicationID,int driverID,int LicenseClass,
           DateTime issueDate,DateTime expirationDate,string notes,float paidFees,bool isActive,
           enIssueReason issueReason, int createdByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.IsActive = isActive;
            this.PaidFees = paidFees;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;
            this.DriverInfo = clsDrivers.FindByDriverID(this.DriverID);
            this.ClassInfo = clsLicenseClass.Find(this.LicenseClass);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            Mode = enMode.Update;
        }
        public static string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return " Renew";
                case enIssueReason.DamagedReplacement:
                    return " Replace for damage";
                case enIssueReason.LostReplacement:
                    return "Replace for Lost";
                default:
                    return " First Time";

            }
        }
        public int Detain(float FineFees,int createdByUserID)
        {
            clsDetainedLicense detainLicense = new clsDetainedLicense();
            detainLicense.LicenseID = this.LicenseID;
            detainLicense.DetainDate = DateTime.Now;
            detainLicense.FineFees = FineFees;
            detainLicense.CreatedByUserID = createdByUserID;
            if(!detainLicense .Save())
            {
                return -1;
            }
            else
            {
                return detainLicense.DetainID;
            }
        }
        public static DataTable GetAllLicenseClasses()
        {
            return clsLicensesData.GetAllLicenses();
        }
        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicensesData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        private bool _AddNewLicense()
        {
            //call DataAccess Layer 

            this.LicenseID = clsLicensesData.AddNewLicense (
                this.ApplicationID, this.DriverID,
                this.LicenseClass, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees,this.IsActive,(byte)this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }
        private bool _UpdateLicense()
        {
            //call DataAccess Layer 

            return clsLicensesData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID,
                this.LicenseClass, this.IssueDate,
                this.ExpirationDate,this.Notes, this.PaidFees,this.IsActive,(byte)this.IssueReason, this.CreatedByUserID);

        }
        public static clsLicenses FindLicense(int LicenseID)
        {
            int ApplicationID = -1,DriverID = -1,LicenseClass = 1;
            DateTime IssueDate = DateTime.Now ,ExpirationDate = DateTime.Now;
          byte IssueReason = 1;
            bool isActive = false;
           string notes = "";
            float paidFees = 0; int CreatedByUserID = -1;

            bool IsFound = clsLicensesData.GetLicenseInfoByID
                                (
                                    LicenseID, ref ApplicationID,
                                    ref DriverID, ref LicenseClass,
                                    ref IssueDate, ref ExpirationDate,ref notes,
                                    ref paidFees, ref isActive,ref IssueReason, ref CreatedByUserID
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsLicenses(LicenseID, ApplicationID,
                                     DriverID, LicenseClass,
                                    IssueDate, ExpirationDate,
                                     notes,paidFees,isActive,(enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }
        public bool Delete()
        {
            return clsLicensesData.DeleteLicense(this.LicenseID);
        }
        public static bool IsLicenseExist(int LicenseID)
        {
            return clsLicensesData.IsLicenseExist(LicenseID);
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicensesData.GetDriverLicenses(DriverID);
        }
        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }
        public bool DeactivateCurrentLicense()
        {
            return (clsLicensesData.DeActivateLicense(this.LicenseID)) ;
        }
        public clsLicenses RenewLicense(string Notes ,int CreatedByUserID)
        {
            clsApplications Application = new clsApplications();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus =clsApplications. enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;
            if (!Application.Save())
            {
                return null;
            }
            clsLicenses License = new clsLicenses();
            License.ApplicationID = Application.ApplicationID;
            License.DriverID = this.DriverInfo.DriverID;
            License.LicenseClass = this.ClassInfo.LicenseClassID;
            License.IssueDate = DateTime.Now;
            int DefaultValidity = this.ClassInfo.DefaultValidityLength;
            License.ExpirationDate = DateTime.Now.AddYears(DefaultValidity);
            License.Notes = Notes;
            License.PaidFees = this.ClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicenses.enIssueReason.Renew;
            License.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            License.CreatedByUserID = CreatedByUserID;
            if (!License.Save())
            {
                return null;
            }
            DeactivateCurrentLicense();
            return License;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID ,ref int ApplicationID)
        {
            clsApplications Application = new clsApplications();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees;
            Application.CreatedByUserID = ReleasedByUserID;
            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }
            ApplicationID = Application.ApplicationID;
            return this.DetainedInfo.ReleasedLicense(ReleasedByUserID, Application.ApplicationID);
        }
        public clsLicenses ReplaceLicense(string Notes, int CreatedByUserID)
        {
            clsApplications Application = new clsApplications();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (IssueReason == enIssueReason.LostReplacement)?
                (int)clsApplications.enApplicationType.ReplaceLostDrivingLicense
              :  (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.FindByApplicationTypeID(Application.ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;
            if (!Application.Save())
            {
                return null;
            }
            clsLicenses License = new clsLicenses();
            License.ApplicationID = Application.ApplicationID;
            License.DriverID = this.DriverInfo.DriverID;
            License.LicenseClass = this.ClassInfo.LicenseClassID;
            License.IssueDate = DateTime.Now;
            int DefaultValidity = this.ClassInfo.DefaultValidityLength;
            License.ExpirationDate = DateTime.Now.AddYears(DefaultValidity);
            License.Notes = Notes;
            License.PaidFees = 0;
            License.IsActive = true;
            License.IssueReason = IssueReason;
            License.CreatedByUserID = CreatedByUserID;
            if (!License.Save())
            {
                return null;
            }
            DeactivateCurrentLicense();
            return License;
        }

    }
}
