//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMSCustomerPortal.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class X_PICK
    {
        public int rec_no { get; set; }
        public string OrderId { get; set; }
        public string PickState { get; set; }
        public string ProdCode { get; set; }
        public Nullable<int> PickQty { get; set; }
        public Nullable<int> LineNumber { get; set; }
        public string LineText { get; set; }
        public string LineType { get; set; }
        public string PickLocation { get; set; }
        public Nullable<int> TmId { get; set; }
    }
}
