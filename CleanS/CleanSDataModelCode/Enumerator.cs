﻿using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace CleanS.CleanS
{

    public partial class Enumerator
    {
        public Enumerator(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
