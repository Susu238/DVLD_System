using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DVLD_DataAccess;
using System.Threading.Tasks;
using System.Data;


namespace DVLD_Business
{
    public class clsPeopleBusiness
    {

     public   enum enMode { AddNew = 0, Update = 1 };
       public enMode Mode = enMode.AddNew;
        public int ID { set; get; }
        public string NationalNo { set; get; }

        public string FirstName { set; get; }

        public string SecondName { set; get; }
        public string ThirdName { set; get; }

        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public int NationalityCountryID { set; get; }
        public clsCountryBusiness CountryInfo;
        public DateTime DateOfBirth { set; get; }
        public short  Gendor { set; get; }

        public string ImagePath { set; get; }
        public clsPeopleBusiness()
        {
            this.ID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";

            this.ThirdName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.NationalityCountryID = -1;
            this.DateOfBirth = DateTime.Now;
            this.Gendor= -1;

            this.ImagePath = "";
            Mode = enMode.AddNew;
        }
        private clsPeopleBusiness(int ID,string NationalNo, String FirstName,string SecondName ,string ThirdName, String LastName, String Email,
    String Phone, String Address, int NationalityCountryID, DateTime DateOfBirth,short Gendor,
    String ImagePath)
        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;

            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.NationalityCountryID = NationalityCountryID;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.CountryInfo = clsCountryBusiness.Find(NationalityCountryID);
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }
        public static DataTable  GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
        public static bool IsPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }

        public static bool IsNationalNoExist(string NationalNo)
        {
            return clsPersonData.IsNationalNoExist(NationalNo);
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }
        private bool _AddNewPerson()
        {
            this.ID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName,
    this.LastName, this.Email, this.Phone,
    this.Address, this.DateOfBirth,
    this.Gendor, 
    this.NationalityCountryID,
    this.ImagePath);
            return (this.ID != -1);
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.ID,this.NationalNo, this.FirstName,this.SecondName,this.ThirdName, this.LastName, this.Email,
                this.Phone, this.Address, this.DateOfBirth, (byte)this.Gendor, this.NationalityCountryID, this.ImagePath);
        }
        public bool Save()
        {
            try
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewPerson())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case enMode.Update:
                        return _UpdatePerson();
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                // Log or store error
                return false;
            }
        }

        public static clsPeopleBusiness Find(int ID)
        {
            String NationalNo = "", FirstName = "",SecondName = "",ThirdName = "", LastName = "", Email = "", Phone = "",
                Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = -1;
            if (clsPersonData.GetPersonInfoByID(ID,ref NationalNo, ref FirstName,ref SecondName,ref ThirdName, ref LastName, ref Email,
                ref Phone, ref Address, ref NationalityCountryID, ref DateOfBirth,ref Gendor, ref ImagePath))
                return new clsPeopleBusiness(ID,NationalNo, FirstName,SecondName,ThirdName, LastName, Email, Phone, Address, NationalityCountryID,
                    DateOfBirth,Gendor, ImagePath);

            else
                return null;
        }



    
            public static clsPeopleBusiness Find(string  NationalNo)
        {
            int ID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "",
                Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = -1;
            if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref ID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref Email,
                ref Phone, ref Address, ref NationalityCountryID, ref DateOfBirth, ref Gendor, ref ImagePath))
                return new clsPeopleBusiness(ID, NationalNo, FirstName, SecondName, ThirdName, LastName, Email, Phone, Address, NationalityCountryID,
                    DateOfBirth, Gendor, ImagePath);

            else
                return null;
        }



    }

}
