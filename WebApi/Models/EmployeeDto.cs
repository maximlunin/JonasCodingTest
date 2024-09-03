using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
	public class EmployeeDto : EmployeeBaseDto
	{
        //public string EmployeeName { get; set; }
        //public string Occupation { get; set; }
        //public string EmployeeStatus { get; set; }
        //public string EmailAddress { get; set; }
        //public string Phone { get; set; }
        //public DateTime LastModified { get; set; }

        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string OccupationName { get; set; }
        public string EmployeeStatus { get; set; }       
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}

