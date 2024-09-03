using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.Model.Models;

namespace WebApi.Models
{
    public class CompanyDto : BaseDto
    {

        [Required]
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostalZipCode { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EquipmentCompanyCode { get; set; }
        public List<ArSubledgerInfo> ArSubledgers { get; set; }
        public string Country { get; set; }
        //public List<ArSubledgerDto> ArSubledgers { get; set; }
        public DateTime LastModified { get; set; }
    }
}