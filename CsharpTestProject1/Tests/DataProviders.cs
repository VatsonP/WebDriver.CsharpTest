using System;
using System.Collections;
using CsharpWebDriverLib;
using CsharpTestProject1.Model;


namespace CsharpTestProject1.Test
{
    internal class DataProviders
    {
        internal struct CurDateTime
        {
            public DateTime curDate;
            public int yyyy;
            public int mm;
            public int dd;
            public int h;
            public int m;
            public int s;

            public CurDateTime(DateTime date)
            {
                curDate = date;
                yyyy = curDate.Year;
                mm = curDate.Month;
                dd = curDate.Day;
                h = curDate.Hour;
                m = curDate.Minute;
                s = curDate.Second;
            }
        }

        internal static string GetTimePrefix(CurDateTime curDateTime)
        {
            return WebDriverExtensions.PaddingLeft(curDateTime.h) +
                   WebDriverExtensions.PaddingLeft(curDateTime.m) +
                   WebDriverExtensions.PaddingLeft(curDateTime.s);
        }
        internal static string GetTimeId(CurDateTime curDateTime)
        {
            return WebDriverExtensions.PaddingLeft(curDateTime.yyyy, 4) + "-" + WebDriverExtensions.PaddingLeft(curDateTime.mm) + "-" +
                   WebDriverExtensions.PaddingLeft(curDateTime.dd) + "_" + 
                   curDateTime.h.ToString() + curDateTime.m.ToString() + curDateTime.s.ToString(); ;
        }

        internal static IEnumerable ValidCustomers
        {
            get
            {
                // читаем текущее время - добавляем его к фамилии и имеем уникальный e-mail и пароль каждый раз
                var curDateTime = new CurDateTime(DateTime.Now);

                var firstName = "Ivan";
                var lastName = "Tankist";
                var eMailName = firstName + GetTimePrefix(curDateTime);
                var taxId     = GetTimeId(curDateTime);

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