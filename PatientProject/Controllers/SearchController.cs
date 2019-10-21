using PatientProject.Providers;
using PatientProjectModels;
using System.Net;
using System;
using System.Net.Http;
using System.Web.Http;
using NLog;
using Newtonsoft.Json;
using System.Text;

namespace PatientProjectControllers
{
    /// <summary>
    /// This is a Patient Search
    /// </summary>
    public class SearchController : ApiController
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This searches patient with source and medical record number
        /// </summary>
        /// <param name="source"></param>
        /// <param name="medicalRecordNumber"></param>
        /// <returns></returns>
        public HttpResponseMessage GetPatient(string source, string medicalRecordNumber)
        {
            try
            {
                var thisDataAccessprovider = new DataAccessProvider();
                var thisPatient = string.Empty;

                thisPatient = JsonConvert.SerializeObject(new DataAccessProvider().SearchPatient(source, medicalRecordNumber));
                
                return new HttpResponseMessage()
                {
                    Content = new StringContent(thisPatient, Encoding.UTF8, "text/plain")
                };
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProjectControllers.SearchController.GetPatient {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters {DateTime.Now}: Source: {source} Mrn:{medicalRecordNumber}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Post is not allowed
        /// </summary>
        public HttpResponseMessage Post()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Put is not allowed
        /// </summary>
        public HttpResponseMessage Put()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Delete is not allowed
        /// </summary>
        public HttpResponseMessage Delete()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }
    }
}
