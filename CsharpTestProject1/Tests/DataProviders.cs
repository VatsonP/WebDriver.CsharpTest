using System;
using System.Collections;

namespace CsharpTestProject1
{
    internal class DataProviders
    {
        private static DateTime curDate;

        public static IEnumerable ValidCustomers
        {
            get
            {
                // читаем текущее время - добавляем его к фамилии и имеем уникальный e-mail и пароль каждый раз
                curDate = DateTime.Now;
                int yyyy = curDate.Year;
                int mm = curDate.Month;
                int dd = curDate.Day;
                int h = curDate.Hour;
                int m = curDate.Minute;
                int s = curDate.Second;

                var firstName = "Ivan";
                var lastName = "Tankist";
                var eMailName = firstName + WebDriverExtensions.PaddingLeft(h) + WebDriverExtensions.PaddingLeft(m) + WebDriverExtensions.PaddingLeft(s);
                var taxId     = WebDriverExtensions.PaddingLeft(yyyy, 4) + "-" + WebDriverExtensions.PaddingLeft(mm) + "-" + 
                                WebDriverExtensions.PaddingLeft(dd)      + "_" + h.ToString() + m.ToString() + s.ToString();

                yield return new Customer()
                {
                    Tax_id = taxId,
                    Firstname = firstName,
                    Lastname = lastName,
                    Phone = "+012-345-67-89",
                    Address1 = "Hidden Place",
                    Postcode = "12345",
                    City = "Kyiv",
                    Country = "UA",
                    //Zone = "",
                    Company = "MMM",
                    Email = eMailName + "@gmail.com",
                    Password = "eMailName"
                };
                /*
                 ... 
                */
            }
        }
    }
}