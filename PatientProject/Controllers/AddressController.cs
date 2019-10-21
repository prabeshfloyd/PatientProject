using NLog;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PatientProjectControllers
{
    /// <summary>
    /// This is an Address Controller
    /// </summary>
    public class AddressController : ApiController
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Get is not allowed, use Patient Member Controller to get addresses
        /// </summary>
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Get is not allowed, use Patient Member Controller to get address
        /// </summary>
        public HttpResponseMessage Get(int id)
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// post is not allowed, use Patient Member Controller to add address with a patient
        /// </summary>
        public HttpResponseMessage Post()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Put is not allowed, use Patient Member Controller to update address with a patient
        /// </summary>
        public HttpResponseMessage Put()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Delete is not allowed, use Patient Member Controller to delete address with a patient
        /// </summary>
        public HttpResponseMessage Delete()
        {
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }
    }
}
