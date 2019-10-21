using Newtonsoft.Json;
using NLog;
using PatientProjectModels;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PatientProject.Providers
{
    /// <summary>
    /// This is the Data Access Provider
    /// </summary>
    public class DataAccessProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// This determines the connection string.
        /// </summary>
        /// <returns>This returns the connection string</returns>
        private string GetConnectionString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["Patient"].ConnectionString ?? string.Empty;
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.GetConnectionString {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine}");
                return string.Empty;                
            }
        }


        /// <summary>
        /// This gets Patient
        /// </summary>
        /// <returns></returns>
        internal DataTable GetPatient(string patientID)
        {
            DataTable dataTable = new DataTable();

            string qry = "[Patient].[usp_getPatient]";

            try
            {

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(qry, conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.Add(new SqlParameter("@patientID", patientID));
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dataTable.Load(rdr);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.GetPatients {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters: {DateTime.Now}: patientID: {patientID}");
                return dataTable;
            }
        }


        /// <summary>
        /// This gets Patients
        /// </summary>
        /// <returns></returns>
        internal DataTable GetPatients()
        {
            DataTable dataTable = new DataTable();

            string qry = "[Patient].[usp_getPatients]";

            try
            {

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(qry, conn) { CommandType = CommandType.StoredProcedure };
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dataTable.Load(rdr);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.GetPatients {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine}");
                return dataTable;
            }
        }

        /// <summary>
        /// This determines the patient search.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="medicalRecordNumber"></param>
        /// <returns>This returns patient with matching criteria.</returns>
        internal DataTable SearchPatient(string source, string medicalRecordNumber)
        {

            DataTable dataTable = new DataTable();

            string qry = "[Patient].[usp_searchPatient]";

            try
            {

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(qry, conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.Add(new SqlParameter("@source", source));
                    cmd.Parameters.Add(new SqlParameter("@mrn", medicalRecordNumber));
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        dataTable.Load(rdr);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.SearchPatient {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters: {DateTime.Now}: Source: {source}, MRN: {medicalRecordNumber}");
                return dataTable;
            }
       }

        internal void AddPatient(PatientMemberRecord patientMemberRecord)
        {

                string storedProc = "[Patient].[usp_addPatient]";

                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(storedProc, conn) { CommandType = CommandType.StoredProcedure };

                        cmd.Parameters.Add(new SqlParameter("@Source", patientMemberRecord.source));
                        cmd.Parameters.Add(new SqlParameter("@MedicalRecordNumber", patientMemberRecord.medicalRecordNumber));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", patientMemberRecord.firstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", patientMemberRecord.lastName));
                        cmd.Parameters.Add(new SqlParameter("@SocialSecurityNumber", patientMemberRecord.socialSecurityNumber));
                        cmd.Parameters.Add(new SqlParameter("@Address1", patientMemberRecord.address.AddressLine1));
                        cmd.Parameters.Add(new SqlParameter("@Address2", patientMemberRecord.address.AddressLine2));
                        cmd.Parameters.Add(new SqlParameter("@City", patientMemberRecord.address.City));
                        cmd.Parameters.Add(new SqlParameter("@State", patientMemberRecord.address.State));
                        cmd.Parameters.Add(new SqlParameter("@ZipCode", patientMemberRecord.address.ZipCode));

                        cmd.ExecuteScalar();
                    }

                return;
                }
                catch (Exception ex)
                {
                    logger.Info($"Exception in PatientProject.DataAccessProvider.AddPatient {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters: {DateTime.Now}: {JsonConvert.SerializeObject(patientMemberRecord)}");
                    return;
                }

        }

        internal void UpdatePatient(PatientMemberRecord patientMemberRecord)
        {

            string storedProc = "[Patient].[usp_updatePatient]";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(storedProc, conn) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@Source", patientMemberRecord.source));
                    cmd.Parameters.Add(new SqlParameter("@MedicalRecordNumber", patientMemberRecord.medicalRecordNumber));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", patientMemberRecord.firstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", patientMemberRecord.lastName));
                    cmd.Parameters.Add(new SqlParameter("@SocialSecurityNumber", patientMemberRecord.socialSecurityNumber));
                    cmd.Parameters.Add(new SqlParameter("@Address1", patientMemberRecord.address.AddressLine1));
                    cmd.Parameters.Add(new SqlParameter("@Address2", patientMemberRecord.address.AddressLine2));
                    cmd.Parameters.Add(new SqlParameter("@City", patientMemberRecord.address.City));
                    cmd.Parameters.Add(new SqlParameter("@State", patientMemberRecord.address.State));
                    cmd.Parameters.Add(new SqlParameter("@ZipCode", patientMemberRecord.address.ZipCode));

                    cmd.ExecuteScalar();
                }

                return;
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.UpdatePatient {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters: {DateTime.Now}: {JsonConvert.SerializeObject(patientMemberRecord)}");
                return;
            }

        }


        internal void DeletePatient(PatientMemberRecord patientMemberRecord)
        {

            string storedProc = "[Patient].[usp_DeletePatient]";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(storedProc, conn) { CommandType = CommandType.StoredProcedure };

                    cmd.Parameters.Add(new SqlParameter("@Source", patientMemberRecord.source));
                    cmd.Parameters.Add(new SqlParameter("@MedicalRecordNumber", patientMemberRecord.medicalRecordNumber));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", patientMemberRecord.firstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", patientMemberRecord.lastName));
                    cmd.Parameters.Add(new SqlParameter("@SocialSecurityNumber", patientMemberRecord.socialSecurityNumber));
                    cmd.Parameters.Add(new SqlParameter("@Address1", patientMemberRecord.address.AddressLine1));
                    cmd.Parameters.Add(new SqlParameter("@Address2", patientMemberRecord.address.AddressLine2));
                    cmd.Parameters.Add(new SqlParameter("@City", patientMemberRecord.address.City));
                    cmd.Parameters.Add(new SqlParameter("@State", patientMemberRecord.address.State));
                    cmd.Parameters.Add(new SqlParameter("@ZipCode", patientMemberRecord.address.ZipCode));

                    cmd.ExecuteScalar();
                }

                return;
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProject.DataAccessProvider.DeletePatient {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters: {DateTime.Now}: {JsonConvert.SerializeObject(patientMemberRecord)}");
                return;
            }

        }
    }    
}