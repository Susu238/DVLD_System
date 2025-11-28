using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_Business
{
    public class clsTests
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public clsTestAppointments TestAppointmentInfo { get; set; }
        public clsTests()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        public clsTests(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            this.TestID = testID;
            this.TestAppointmentID = testAppointmentID;
            this.TestResult = testResult;
            this.Notes = notes;
            this.TestAppointmentInfo = clsTestAppointments.Find(TestAppointmentID);
            this.CreatedByUserID = createdByUserID;
            Mode = enMode.Update;
        }
        public static DataTable GetAllTests()
        {
            return clsTests.GetAllTests();
        }
        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this.TestID = clsTestsData.AddNewTest(


              this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }
        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestsData.UpdateTest(this.TestID,

               this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

        }
        public static clsTests Find(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = "";
            bool IsFound = clsTestsData.GetTestInfoByID
                                (
                                  TestID, ref TestAppointmentID,
                                    ref TestResult, ref Notes, ref CreatedByUserID
                                );

            if (IsFound)
            {
                //we return new object of that person with the right data
                return new clsTests(TestID, TestAppointmentID,
                TestResult, Notes, CreatedByUserID
               );
            }

            else
                return null;
        }

        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }
        public bool Delete()
        {
            return clsTestsData.DeleteTest(this.TestID);
        }
        public static bool IsTestExist(int TestID)
        {
            return clsTestsData.IsTestExist(TestID);
        }
        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestsData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestsCount(LocalDrivingLicenseApplicationID) == 3;
        }
        public static clsTests FindLastTestPerPersonAndLicenseClass(int PersonID,int LicenseClassID,
            clsTestTypes.enTestTypes TestTypeID)
        {
            int testID = -1, TestAppointmentID = -1, createdByUserID = -1;
            bool testResult = false;
            string Notes = "";
            if(clsTestsData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID,LicenseClassID,(int)TestTypeID,
                   ref testID,ref TestAppointmentID,ref testResult,ref Notes,ref createdByUserID)){
                return new clsTests(testID, TestAppointmentID, testResult, Notes, createdByUserID);

            }
            else
            {
                return null;
            }
        }
     
    }

}
    
