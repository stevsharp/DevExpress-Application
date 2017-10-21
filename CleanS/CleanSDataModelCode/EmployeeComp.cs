using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace CleanS.CleanS
{

    public partial class EmployeeComp
    {
        public EmployeeComp(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
