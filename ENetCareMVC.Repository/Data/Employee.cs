//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENetCareMVC.Repository.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public Employee()
        {
            this.Audit = new HashSet<Audit>();
            this.Package = new HashSet<Package>();
        }
    
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public System.Guid UserId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public int LocationCentreId { get; set; }
    
        public virtual ICollection<Audit> Audit { get; set; }
        public virtual DistributionCentre Location { get; set; }
        public virtual ICollection<Package> Package { get; set; }
        public override string ToString() { return "Id:" + EmployeeId + " / " + FullName + " (" + UserName + ") / " + EmailAddress + " / Location:" + LocationCentreId; }
    }
}
