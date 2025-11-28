using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_Business
{
   public class clsTestTypes
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestTypes { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public clsTestTypes.enTestTypes ID { get; set; }        
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }

        public float TestTypeFees { get; set; }

        public clsTestTypes()
        {
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.ID = clsTestTypes.enTestTypes.VisionTest;
            this.TestTypeFees = 0;
            Mode = enMode.AddNew;

        }
        private clsTestTypes(clsTestTypes.enTestTypes ID, string TestTypeTitle,string TestTypeDescription, float TestTypeFees)
        {            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.ID = ID;
            this.TestTypeFees = TestTypeFees;
            Mode = enMode.Update;
        }
        public static clsTestTypes FindByTestTypeID(clsTestTypes.enTestTypes testTypeID)
        {
            string testTypeTitle = "";
            string testTypeDescription = "";

            float testTypeFees = 0;
            bool IsFound = clsTestTypesData.GetTestTypeInfoByID((int)testTypeID, ref testTypeTitle,ref testTypeDescription, ref testTypeFees);
            if (IsFound)
            {
                return new clsTestTypes (testTypeID, testTypeTitle,testTypeDescription,testTypeFees);
            
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewTestType()
        {
            this.ID = (clsTestTypes.enTestTypes)clsTestTypesData.AddNewTestType(this.TestTypeTitle,this.TestTypeDescription ,this.TestTypeFees);
            return (this.TestTypeTitle!= "");
        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }


        private bool _UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType((int)this.ID, this.TestTypeTitle ,this.TestTypeDescription, this.TestTypeFees);
        }
        public bool Save()
        {
            try
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewTestType())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case enMode.Update:
                        return _UpdateTestType();
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
