namespace PatientProjectModels
{
    /// <summary>
    /// Address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Internal ID
        /// </summary>
        internal string AddressId { get; set; }

        /// <summary>
        /// Address Line 1
        /// </summary>
        internal string AddressLine1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// </summary>
        internal string AddressLine2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        internal string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        internal string State { get; set; }

        /// <summary>
        /// zipcode
        /// </summary>
        internal string ZipCode { get; set; }
    }
}
