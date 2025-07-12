using System.Runtime.CompilerServices;

namespace Hrms.Core.Utilities
{
    public class Constants
    {
        #region constants

        public const string GraphUrl = "https://graph.microsoft.com/v1.0/me";

        public struct UserType
        {
            public const string Admin = "Admin";
            public const string HRManager = "HRManager";
            public const string HRExecutive = "HRExecutive";
            public const string ReportingManager = "ReportingManager";
            public const string Interviewer = "Interviewer";
            public const string Employee = "Employee";
        }

        public struct employeeCode
        {
            public const string code = "LM/";

        }
        public struct EmployeeMail 
        {
            public const string mail = "@logimonk.com";
        }

        public struct ClaimType
        {
            public const string CompanyId = "company_id";

        }

        #endregion

        #region enum
        public enum RecordStatus
        {
            Created = 1,
            Active = 2,
            Inactive = 3,
            Rejected = 4,
            Deleted = 5,
            Pending = 6,
            Approved = 7,
            Scheduled = 8
        }

        public enum Half
        {
            First = 1,
            Second = 2
        }

        public enum EmployeeType
        {
            FullTime = 1,
            Probation = 2
        }

        public enum ExityType
        {
            Termination = 1,
            Resignation = 2
        }

        public enum AddressType
        {
            Current = 1,
            Permanent = 2,
            Corporate = 3,
            Registered = 4
        }

        public enum Gender
        {
            Male = 1,
            Female = 2
        }

        public enum MaritalStatus
        {
            Married = 1,
            Unmarried = 2
        }

        public enum AccrualFrequency
        {
            Yearly = 1,
            HalfYearly = 2,
            Quarterly = 3,
            Monthly = 4
        }

        public enum AccrualPeriod
        {
            Start = 1,
            End = 2
        }

        public enum DocumentType
        {
            Cv = 1,
            ExpereinceLetter = 2,
            SalarySlip = 3,
            IdentificationDoc = 4,
            PaymentReceipt = 5,
            ProfileImage = 6
        }

        public enum InterviewMode
        {
            Online = 1,
            Offline = 2

        }

        public enum InterviewType
        {
            Technical = 1,
            HR = 2,
        }
        
        public enum DaysOfWeek
        {
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            ThursDay = 4,
            Friday = 5,
            Saturday = 6,
            Sunday = 7
        }

        #endregion


    }
}
