using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceImportAutomator._02_Domain
{
    public interface ICategorizeRepository
    {
        string GetCategoryByDescription(string description);
    }
}
