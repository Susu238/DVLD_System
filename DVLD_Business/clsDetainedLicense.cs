using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
namespace DVLD_Business
{
    public class clsDetainedLicense
    {
       public enum enMode { AddNew = 0 , Update = 1};
        enMode _Mode = enMode.AddNew;
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }

        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserIDInfo { get; set; }
        public bool IsReleased{ get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public clsUser ReleasedByUseInfo { get; set; }
        public int ReleaseApplicationID { get; set; }
        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = -1;
            
            this.ReleaseApplicationID = - 1;
            _Mode = enMode.AddNew;
        }
        public clsDetainedLicense(int detainID ,int licenseID ,DateTime detainDate,float FineFees,int createdByUserID,
            bool isReleased,DateTime releaseDate,int releasedByUserID,int releaseApplicationID)
        {
            this.DetainID = detainID;
            this.LicenseID = licenseID;
            this.DetainDate = detainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserIDInfo = clsUser.FindByUserID(this.CreatedByUserID);
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleasedByUseInfo = clsUser.FindByUserID(this.ReleasedByUserID);
            this.ReleaseApplicationID = releaseApplicationID;
            _Mode = enMode.Update;
        }
        public static bool GetDetainedLicenseBy(int detainID,int licenseID,DateTime detainDate,float fineFees,
            int createdByUserID,bool isReleased,DateTime releaseDate,int releasedByUserID,int releaseApplicationID
            )
        {
            return clsDetainedClassData.GetDetainedLicensefoByID(detainID,ref licenseID,ref detainDate,ref fineFees,
               ref  createdByUserID,ref isReleased,ref releaseDate,ref releasedByUserID,ref releaseApplicationID);
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedClassData.GetAllDetainedLicenses();
        }
        private  bool _AddNewLicense() {
             this.DetainID       = clsDetainedClassData.AddDetainLicense(this.LicenseID,this.DetainDate ,this.FineFees ,this.CreatedByUserID );

            return (DetainID != -1);
        }



        public clsDetainedLicense FindByDetainID(int DetainID)
        {
            int LicenseID = -1, createdByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
            float fineFees = 0;
            bool IsReleased = false;
            if(clsDetainedClassData.GetDetainedLicensefoByID(DetainID,ref LicenseID,ref DetainDate,
                ref fineFees,ref createdByUserID,ref IsReleased,ref ReleaseDate,ref ReleasedByUserID,ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, fineFees, createdByUserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);

            }
            else
                return null;
        }
        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int DetainID = -1, createdByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
            float fineFees = 0;
            bool IsReleased = false;
            if (clsDetainedClassData.GetDetainedLicensefoByLicenseID(LicenseID, ref DetainID, ref DetainDate,
                ref fineFees, ref createdByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, fineFees, createdByUserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);

            }
            else
                return null;
        }

        private  bool _UpdateDetainedLicense()
        {
            return clsDetainedClassData.UpdateDetainLicense(this.DetainID,this.LicenseID ,this.DetainDate,this.FineFees ,this.CreatedByUserID );
        }
        public bool ReleasedLicense(int releasedByUserID,int ReleaseApplicationID)
        {
            return clsDetainedClassData.ReleaseDetainedLicense(this.DetainID, releasedByUserID, ReleaseApplicationID);
        }
        public  bool IsDetainedLicense(int licenseID)
        {
            return clsDetainedClassData.IsDetainedLicense(licenseID);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }
    }
}
