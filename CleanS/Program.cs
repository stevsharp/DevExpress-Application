using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Skins;

namespace CleanS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(MyCommonExceptionHandlingMethod);
            SkinManager.EnableFormSkins();
            DevExpress.Xpo.XpoDefault.DataLayer = new DevExpress.Xpo.SimpleDataLayer(new DevExpress.Xpo.DB.InMemoryDataStore());
            Application.Run(new FrmMain());
        }

        private static void MyCommonExceptionHandlingMethod(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                DisplayErrorBox(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DisplayErrorBox(string stackTrace)
        {
            var err = new ErrorForm
            {
                ExceptionMessage = stackTrace
            };
            err.ShowDialog();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var message = String.Format("Sorry, something went wrong.\r\n" +
                "{0}\r\n" +
                "Please contact support.",
                ((Exception)e.ExceptionObject).Message);

            Console.WriteLine("ERROR {0}: {1}", DateTimeOffset.Now, e.ExceptionObject);
            MessageBox.Show(message, "Unexpected Error");
        }
    }
}


            //AppDomain currentDomain = AppDomain.CurrentDomain;
            //AssemblyName name = new AssemblyName("MyEnum");
            //AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
            //ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, name.Name + ".dll");

            //EnumBuilder myEnum = moduleBuilder.DefineEnum(
            //        "MyEnum.ProductCategory",
            //        TypeAttributes.Public,
            //        typeof(int)
            //);

            //myEnum.DefineLiteral("Beverages", 1);
            //myEnum.DefineLiteral("Fruit", 2);
            //myEnum.CreateType();

            //assemblyBuilder.Save(name.Name + ".dll");

            //Array values = Enum.GetValues(typeof(ProductCategory));

            //foreach (var val in values)
            //{
            //    Console.WriteLine(String.Format("{0}: {1}",
            //            Enum.GetName(typeof(ProductCategory), val),
            //            val));
            //}

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
