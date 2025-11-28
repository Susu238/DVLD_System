using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Business.clsLicenses;
namespace DVLD_Business
{
    public class clsTestAppointments
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int TestAppointmentID { get; set; }
        public clsTestTypes.enTestTypes TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int ReTakeTestApplicationID { get; set; }
        public clsApplications RetakeTestApplicationInfo { get; set; }
        public int TestID
        {
            get { return _GetTestID(); }
        }


        public clsTestAppointments()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestTypes.enTestTypes.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.ReTakeTestApplicationID = -1;
            Mode = enMode.AddNew;
        }

        public clsTestAppointments(int testAppointmentID,clsTestTypes.enTestTypes TestTypeID,int localDrivingLicenseID,
            DateTime appointmentDate,float paidFees,int createdByUserID,bool islocked,int retakeTestApplicationID)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.IsLocked = islocked;
            this.ReTakeTestApplicationID = retakeTestApplicationID;
            this.RetakeTestApplicationInfo = clsApplications.FindBaseApplication(retakeTestApplicationID);
            Mode = enMode.Update;
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointment();
        }

        private int _GetTestID()
        {
            return clsTestAppointmentsData.GetTestID(TestAppointmentID);
        }
        private bool _AddNewTestAppointment()
        {
            //call DataAccess Layer 

            this.TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(
                (int) this.TestTypeID,
                this.LocalDrivingLicenseApplicationID, this.AppointmentDate,
                this.PaidFees, this.CreatedByUserID, this.IsLocked, this.ReTakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }
        private bool _UpdateTestAppointment()
        {
            //call DataAccess Layer 

            return clsTestAppointmentsData.UpdateTestAppointment(this.TestAppointmentID,(int) this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees,
                this.CreatedByUserID, this.IsLocked, this.ReTakeTestApplicationID  );


        }
        public static clsTestAppointments Find(int TestAppointmentID)
        {
            int  TestTypeID = -1;
            int LocalDrivingLicensApplicationID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            bool isLocked = false;
            float paidFees = 0; 
            int CreatedByUserID = -1;

            bool IsFound = clsTestAppointmentsData.GetTestAppointmentLInfoByID 
                                (
                                    TestAppointmentID, ref TestTypeID,
                                    ref LocalDrivingLicensApplicationID ,ref AppointmentDate,
                                    ref paidFees, ref CreatedByUserID, ref isLocked,
                                    ref RetakeTestApplicationID 
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsTestAppointments(TestAppointmentID,(clsTestTypes.enTestTypes)TestTypeID,
                                     LocalDrivingLicensApplicationID, AppointmentDate,
                                    paidFees,CreatedByUserID,
                                    isLocked,  RetakeTestApplicationID);
            else
                return null;
        }
        public static clsTestAppointments GetLastTestAppointment(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestTypes TestTypeID)
        {
            int TestAppointmentID = -1;
            int  RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            bool isLocked = false;
            float paidFees = 0;
            int CreatedByUserID = -1;

            bool IsFound = clsTestAppointmentsData.GetTestAppointmentLInfoByID
                                (
                                 LocalDrivingLicenseApplicationID, (int) TestTypeID ,   ref TestAppointmentID, 
                                    ref AppointmentDate,
                                    ref paidFees, ref CreatedByUserID, ref isLocked,
                                    ref RetakeTestApplicationID
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsTestAppointments(TestAppointmentID, (clsTestTypes.enTestTypes)TestTypeID,
                                     LocalDrivingLicenseApplicationID, AppointmentDate,
                                    paidFees, CreatedByUserID,
                                    isLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }
        public DataTable GetApplicationTestAppointmentPerTestType(clsTestTypes.enTestTypes TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestAppointmentPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }
        public static DataTable GetApplicationTestAppointmentPerTestType(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestTypes TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestAppointmentPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }


    }
}
