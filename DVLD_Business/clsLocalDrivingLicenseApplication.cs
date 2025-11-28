using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Business.clsApplications;
namespace DVLD_Business
{
    public class clsLocalDrivingLicenseApplication : clsApplications
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }
        public clsLicenseClass ClassInfo;
        public clsPeopleBusiness PersonInfo { get; set; }
        public string PersonFullName
        {
            get
            {
                return base.ApplicantFullName;
            }
        }
        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            Mode = enMode.AddNew;
        }
        public clsLocalDrivingLicenseApplication(int localDrivingLicenseID, int applicationID, int ApplicantPersonID,
         DateTime applicationDate, int applicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
         float PaidFees, int CreatedByUserID, int licenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseID;
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = (int) applicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = licenseClassID;
            this.ClassInfo = clsLicenseClass.Find(LicenseClassID);
            Mode = enMode.Update;
        }
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }
        private bool _AddNewLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(


               this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }
        private bool _UpdateLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicense(this.LocalDrivingLicenseApplicationID,

               this.ApplicationID, this.LicenseClassID);

        }
        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;


            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                                (
                                  LocalDrivingLicenseApplicationID, ref ApplicationID,
                                    ref LicenseClassID
                                );

            if (IsFound)
            {
                //now we find the base application
                clsApplications Application = clsApplications.FindBaseApplication(ApplicationID);
                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID,
                Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus,
                Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }

            else
                return null;
        }
        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;


            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                                (
                                ApplicationID,ref  LocalDrivingLicenseApplicationID, 
                                    ref LicenseClassID
                                );

            if (IsFound)
            {
                //now we find the base application
                clsApplications Application = clsApplications.FindBaseApplication(ApplicationID);
                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID,
                Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID,(enApplicationStatus) Application.ApplicationStatus,
                Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }

            else
                return null;
        }



        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplications.enMode)Mode;
            if (!base.Save())
                return false;
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLocalDrivingLicenseApplication();

            }

            return false;
        }
        public bool Delete()
        {
            bool IsLocalDrivingLicenseDeleted = false;
            bool IsBaseApplicationDeleted = false;

            IsLocalDrivingLicenseDeleted = clsLocalDrivingLicenseApplicationData.DeleteLicense(this.LocalDrivingLicenseApplicationID);
            if (!IsLocalDrivingLicenseDeleted)
                return false;
            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;
        
        }
        public bool DoesPassTestType(clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }
        public bool DoesPassPreviousTes(clsTestTypes.enTestTypes CurrentTest)
        {
            switch (CurrentTest)
            {
                case clsTestTypes.enTestTypes.VisionTest:
                    // case first test no need to previous test
                    return true;
                case clsTestTypes.enTestTypes.WrittenTest:
                    return this.DoesPassTestType(clsTestTypes.enTestTypes.VisionTest);
                case clsTestTypes.enTestTypes.StreetTest:
                    return this.DoesPassTestType(clsTestTypes.enTestTypes.WrittenTest);
                default:
                    return false;
            } 
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool DoesAttendTestType(clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public byte TotalTrialPerTest(clsTestTypes.enTestTypes TestTypeID)
        {
          return  clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static byte TotalTrialPerTest(int LocalDrivingLicnseApplicationID,clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(LocalDrivingLicnseApplicationID, (int)TestTypeID);
        }
        public static bool AttendTest(int LocalDrivingLicnseApplicationID, clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(LocalDrivingLicnseApplicationID, (int)TestTypeID)>0;
        }
        public bool AttendTest( clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) ;
        }
        public bool IsThereAnActiveScheduledTest( clsTestTypes.enTestTypes TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) ;
        }
        public clsTests GetLastTestPerTestType(clsTestTypes.enTestTypes TestTypeID)
        {
            return clsTests.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTests.PassedAllTests(LocalDrivingLicenseApplicationID);
        }
        public  bool PassedAllTests()
        {
            return clsTests.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }
        public static bool IsLocalDrivingLicenseExist(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.IsLocalDrivingLicenseExist(LocalDrivingLicenseApplicationID);
        }
        public int GetActiveLicense()
        {
            return clsLicenses.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }
        public byte GetPassedTestCount()
        {
            return clsTests.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID);
        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseAppplicationID)
        {
            return clsTests.GetPassedTestsCount(LocalDrivingLicenseAppplicationID);
        }
        public int IssueLicenseForTheFirstTime(string Notes,int CreatedByUserID)
        {
            int DriverID = -1;
            clsDrivers Driver = clsDrivers.FindByPersonID(this.ApplicantPersonID);
            if(Driver == null)
            {
                Driver = new clsDrivers();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }
            //NOW WE add license 
            clsLicenses License = new clsLicenses();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.ClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.ClassInfo.ClassFees;
            License.IsActive = true;
            License.CreatedByUserID = CreatedByUserID;
            if (License.Save())
            {
                this.SetComplete();
                return License.LicenseID;
            }
            else
            {
                return -1;
            }
        }
        public bool IsLicenseIssued()
        {
            return (GetActiveLicense() != -1);
        }
    }
}