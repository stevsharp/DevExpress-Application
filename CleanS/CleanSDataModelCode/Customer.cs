using DevExpress.Xpo;
namespace CleanS.CleanS
{

    public partial class Customer
    {
        public Customer(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
