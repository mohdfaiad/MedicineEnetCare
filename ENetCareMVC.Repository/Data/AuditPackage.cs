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
    
    public partial class AuditPackage
    {
        public int AuditPackageId { get; set; }
        public int AuditId { get; set; }
        public int PackageId { get; set; }
    
        public virtual Audit Audit { get; set; }
        public virtual Package Package { get; set; }
    }
}