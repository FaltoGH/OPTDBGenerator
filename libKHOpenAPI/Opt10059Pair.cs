using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    public class Opt10059Pair
    {
        public string Date;
        public long Close;
        public long Symbol;
        public long Diff;
        public string Rate;
        public long Volume;
        public long Money;

        public long PersonB;
        public long ForeignerB;
        public long OrgB;
        public long FinanceB;
        public long InsuranceB;
        public long InvestB;
        public long MiscFinanceB;
        public long BankB;
        public long PensionB;
        public long PEFB;
        public long NationB;
        public long MiscCorpB;
        public long LocalForeignerB;

        public long PersonS;
        public long ForeignerS;
        public long OrgS;
        public long FinanceS;
        public long InsuranceS;
        public long InvestS;
        public long MiscFinanceS;
        public long BankS;
        public long PensionS;
        public long PEFS;
        public long NationS;
        public long MiscCorpS;
        public long LocalForeignerS;

        public Opt10059Pair(KeyValuePair<Opt10059Row,Opt10059Row> pair)
        {
            Date = pair.Key.Date;
            Close = pair.Key.Close;
            Symbol = pair.Key.Symbol;
            Diff = pair.Key.Diff;
            Rate = pair.Key.Rate;
            Volume = pair.Key.Volume;
            Money = pair.Key.Money;

            PersonB = pair.Key.Person;
            ForeignerB = pair.Key.Foreigner;
            OrgB = pair.Key.Org;
            FinanceB = pair.Key.Finance;
            InsuranceB = pair.Key.Insurance;
            InvestB = pair.Key.Invest;
            MiscFinanceB = pair.Key.MiscFinance;
            BankB = pair.Key.Bank;
            PensionB = pair.Key.Pension;
            PEFB = pair.Key.PEF;
            NationB = pair.Key.Nation;
            MiscCorpB = pair.Key.MiscCorp;
            LocalForeignerB = pair.Key.LocalForeigner;

            PersonS = pair.Value.Person;
            ForeignerS = pair.Value.Foreigner;
            OrgS = pair.Value.Org;
            FinanceS = pair.Value.Finance;
            InsuranceS = pair.Value.Insurance;
            InvestS = pair.Value.Invest;
            MiscFinanceS = pair.Value.MiscFinance;
            BankS = pair.Value.Bank;
            PensionS = pair.Value.Pension;
            PEFS = pair.Value.PEF;
            NationS = pair.Value.Nation;
            MiscCorpS = pair.Value.MiscCorp;
            LocalForeignerS = pair.Value.LocalForeigner;
        }

        public string ToValue()
        {
            return $"({Date},{Close},{Symbol},{Diff},{Rate},{Volume},{Money}," +
$"{PersonB},{ForeignerB},{OrgB},{FinanceB},{InsuranceB},{InvestB},{MiscFinanceB},{BankB},{PensionB},{PEFB},{NationB},{MiscCorpB},{LocalForeignerB}," +
$"{PersonS},{ForeignerS},{OrgS},{FinanceS},{InsuranceS},{InvestS},{MiscFinanceS},{BankS},{PensionS},{PEFS},{NationS},{MiscCorpS},{LocalForeignerS})";
        }

        public static string ToValues(IEnumerable<Opt10059Pair> pairs)
        {
            return string.Join(",", pairs.Select(x=>x.ToValue()));
        }

    }
}
