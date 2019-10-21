using Newtonsoft.Json;
using NLog;
using PatientProject.Providers;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PatientProjectControllers
{

    /// <summary>
    /// This is a patient controller
    /// </summary>
    public class PatientController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// this gets all patients
        /// </summary>
        /// <returns>listofPatients</returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {

            try
            {
                var thisDataAccessprovider = new DataAccessProvider();
                var thisPatient = string.Empty;

                thisPatient = JsonConvert.SerializeObject(new DataAccessProvider().GetPatients());

                return new HttpResponseMessage()
                {
                    Content = new StringContent(thisPatient, Encoding.UTF8, "text/plain")
                };
            }
            catch (Exception ex)
            {
                logger.Info($"Exception in PatientProjectControllers.PatientController.Get {Environment.NewLine} Exception: {DateTime.Now}: {JsonConvert.SerializeObject(ex)} {Environment.NewLine}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }   
    }
}
