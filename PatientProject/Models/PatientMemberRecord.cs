namespace PatientProjectModels
{
    /// <summary>
    /// Patient
    /// </summary>
    public class PatientMemberRecord
    {
        /// <summary>
        /// Patient Source: EX: A Great Hospital, Awesomesauce HIE
        /// </summary>
        internal string source { get; set; }

        /// <summary>
        ///Medical Record Number
        /// </summary>
        internal string medicalRecordNumber { get; set; }

        /// <summary>
        /// Patient First Name
        /// </summary>
        internal string firstName { get; set; }

        /// <summary>
        /// Patient Last Name
        /// </summary>
        internal string lastName { get; set; }

        /// <summary>
        /// Patient Social Security Number
        /// </summary>
        internal string socialSecurityNumber { get; set; }

        /// <summary>
        /// Patient Address
        /// </summary>
        internal Address address { get; set; }
    }
}
