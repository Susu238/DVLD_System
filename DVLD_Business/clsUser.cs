using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess; 
namespace DVLD_Business
{

  

    public class clsUser
    {

     
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int userID { set; get; }
        
        public  int PersonID{ set; get; }
        public clsPeopleBusiness PersonInfo;
        public string userName{ set; get; }
        public string Password { set; get; }

        public bool IsActive{ set; get; }

        public clsUser()
        {
            this.userID = -1;
            this.userName = "";
            this.Password = "";
            this.IsActive = true;
            Mode = enMode.AddNew;
         
            
        }
        private clsUser(int userID, int PersonID,  string userName, string Password, bool IsActive)
        {
            this.userID = userID;
            this.PersonID = PersonID;
            this.userName = userName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsPeopleBusiness.Find(PersonID);
            
            Mode = enMode.Update;
        }
       
      
     
      
        private bool _AddNewUser() {
            this.userID = clsUserData.AddNewUser( this. PersonID, this.userName, this. Password,
              this. IsActive);
            return (this.userID != -1);
        }
      private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.userID, this.PersonID, this.userName, this.Password, this.IsActive);
        }
        public static clsUser FindByUserID(int userID)
        {
            int PersonID = -1;
            string password = "", userName = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByUserID(userID, ref PersonID, ref userName, ref password,
                ref IsActive);
            if (IsFound)
            {
                return new clsUser(userID, PersonID, userName, password, IsActive);
            }
            else
            {
                return null;
            }
        }
            public static clsUser FindByPersonID(int personID)
        {
            int userID = -1;
            string password = "", userName = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByPersonID(personID, ref userID, ref userName, ref password,
                ref IsActive);
            if (IsFound)
            {
                return new clsUser(personID,userID, userName, password, IsActive);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindByUserNameAndPassword(string userName , string password)
        {
            int personID = -1, userID = -1;
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByUserNamAndPassword(userName, password, ref userID, ref personID,
                ref IsActive);
            if (IsFound)
            {
                return new clsUser(personID, userID, userName, password, IsActive);
            }
            else
            {
                return null;
            }
        }
        public bool Save()
        {
            try
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewUser())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case enMode.Update:
                        return _UpdateUser();
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
        public static DataTable GetAllUser()
        {
            return clsUserData.GetAllUsers();
        }
        public static  bool DeleteUser(int userID)
        {
            return clsUserData.DeleteUser(userID);
        }
        public static bool IsExistForPersonID(int personID)
        {
            return clsUserData.IsUserExistForPersonID(personID);
        }
        public static bool IsUserExist(int userID)
        {
            return clsUserData.IsUserExist(userID);
        }
        public static bool IsUserExist(string userName)
        {
            return clsUserData.IsUserExist(userName);
        }
    }
}
