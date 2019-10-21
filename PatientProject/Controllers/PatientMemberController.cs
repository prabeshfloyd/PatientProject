using Newtonsoft.Json;
using NLog;
using PatientProject.Providers;
using PatientProjectModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PatientProjectControllers
{
    /// <summary>
    /// This is a patient controller
    /// </summary>
    public class PatientMemberController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This is not allowed, use patient controller to get patients.
        /// </summary>
        /// <returns>listofPatients</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// This gets the patient
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get(int id)
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Add a patient
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        public void Post([FromBody]string value)
        {
            try
            {
                var thisDataAccessprovider = new DataAccessProvider();
                var thisPatient = JsonConvert.DeserializeObject<PatientMemberRecord>(value);

                new DataAccessProvider().AddPatient(thisPatient);
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProjectControllers.PatientMemberController.Post {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters {DateTime.Now}: Patient: {value} ");
            }
        }

        /// <summary>
        /// This updates the patient.
        /// </summary>
        /// <param name="value"></param>
        public void Put([FromBody]string value)
        {
            try
            {
                var thisDataAccessprovider = new DataAccessProvider();
                var thisPatient = JsonConvert.DeserializeObject<PatientMemberRecord>(value);

                new DataAccessProvider().UpdatePatient(thisPatient);
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProjectControllers.PatientMemberController.Put {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters {DateTime.Now}: Patient: {value} ");
            }
        }

        /// <summary>
        /// This deletes the patient
        /// </summary>
        /// <param name="value"></param>
        public void Delete([FromBody]string value)
        {
            try
            {
                var thisDataAccessprovider = new DataAccessProvider();
                var thisPatient = JsonConvert.DeserializeObject<PatientMemberRecord>(value);

                new DataAccessProvider().DeletePatient(thisPatient);
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProjectControllers.PatientMemberController.Delete {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine} Parameters {DateTime.Now}: Patient: {value} ");
            }
        }
    }
}
