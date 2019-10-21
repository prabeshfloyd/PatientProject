using System.Collections.Generic;

namespace PatientProjectModels
{
    /// <summary>
    /// Patient
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Global Identifier
        /// </summary>
        internal int EnterpriseId { get; set; }

        /// <summary>
        /// Collection of Patient Member Records
        /// </summary>
        internal List<PatientMemberRecord> MemberRecords;
    }
}
