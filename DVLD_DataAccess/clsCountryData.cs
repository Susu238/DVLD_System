using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
   public class clsCountryData

    {
        public static DataTable GetAllCountries()
        {

            DataTable datatable = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * from Countries ORDER By CountryName ";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    datatable.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error fetching countries: " + ex.Message);
                throw new Exception("Error failed: " + ex.Message, ex);

            }
            finally
            {
                connection.Close();
            }
            return datatable;
        }
      
        
        public static bool GetAllCountriesByID(int ID, ref string CountryName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Select * from Countries Where CountryID = @CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    CountryName = (string)reader["CountryName"];

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
                throw new Exception("Error failed: " + ex.Message, ex);

            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }
        public static bool GetAllCountriesByName(ref int ID, string CountryName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Select * from Countries Where CountryName = @CountryName ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ID = (int)reader["CountryID"];
                    CountryName = (string)reader["CountryName"];

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
                throw new Exception("Error failed: " + ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }





    }
}
